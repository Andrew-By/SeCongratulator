using AddUtil.Db;
using AddUtil.Models;
using AddUtil.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AddUtil.ViewModels
{
    /// <summary>
    /// Инкапсулирует логику для вызова добавления новой модели в БД.
    /// </summary>
    public class NewCongratulationViewModel : BindableBase, IInteractionRequestAware
    {
        //
        // Static consts.
        //


        private const string male = "Муж.";
        private const string female = "Жен.";
        private const string bothSex = "Муж./Жен.";

        //
        // Props and fields.
        //

        private readonly CongratulationDbContext dbContext = new CongratulationDbContext();


        private List<string> sexChooser;
        public List<string> SexChooser
        {
            get => sexChooser;
            set => SetProperty(ref sexChooser, value);
        }

        private string selectedSex;
        public string SelectedSex
        {
            get => selectedSex;
            set
            {
                SetProperty(ref selectedSex, value);
            }
        }

        private CongratulationModel congratulationModel;
        public CongratulationModel CongratulationModel
        {
            get => congratulationModel;
            set
            {
                SetProperty(ref congratulationModel, value);
            }
        }

        //
        // Commands.
        //

        private ICommand appendCommand;
        public ICommand AppendCommand
        {
            get
            {
                return appendCommand ??
                    (appendCommand = new DelegateCommand(SaveCongratulation));
            }
        }

        public ICommand chooseFirstContentTypeCommand;
        public ICommand ChooseFirstContentTypeCommand
        {
            get => chooseFirstContentTypeCommand ?? (chooseFirstContentTypeCommand = new DelegateCommand(() => ChooseContentTypeInModel("LOL")));
        }

        private ICommand chooseSecondContentTypeCommand;
        public ICommand ChooseSecondContentTypeCommand
        {
            get => chooseSecondContentTypeCommand ?? (chooseSecondContentTypeCommand = new DelegateCommand(() => ChooseContentTypeInModel("VLAD")));
        }

        private ICommand chooseThirdContentTypeCommand;
        public ICommand ChooseThirdContentTypeCommand
        {
            get => chooseThirdContentTypeCommand ?? (chooseThirdContentTypeCommand = new DelegateCommand(() => ChooseContentTypeInModel("SUSI")));
        }

        private ICommand abortCommand;
        public ICommand AbortCommand
        {
            get => abortCommand ?? (abortCommand = new DelegateCommand(AbortAppending));
        }

        private CongratulationConfirmation _notification;
        public INotification Notification
        {
            get { return _notification; }
            set
            {
                _notification = (CongratulationConfirmation)value;
                CopyCongratulation(_notification.Congratulation);
            }
        }
        public Action FinishInteraction { get; set; }

        public NewCongratulationViewModel()
        {
            InitSexChooser();
        }

        //
        // Private methods.
        //

        private void CopyCongratulation(CongratulationModel congratulationModel)
        {
            this.CongratulationModel = new CongratulationModel();

            this.CongratulationModel.Id = congratulationModel.Id;
            this.CongratulationModel.Kind = congratulationModel.Kind;
            this.CongratulationModel.Content = congratulationModel.Content;
            this.CongratulationModel.Holiday = congratulationModel.Holiday;
            this.CongratulationModel.Interest = congratulationModel.Interest;

            this.CongratulationModel.Sex = congratulationModel.Sex;

            if (congratulationModel.Sex == 1)
                SelectedSex = male;
            else if (congratulationModel.Sex == 0)
                SelectedSex = female;
            else if (congratulationModel.Sex == 2)
                SelectedSex = bothSex;
            else
                throw new ArgumentException("WTF?!!");


            this.CongratulationModel.Age = congratulationModel.Age;
        }

        private void InitSexChooser()
        {
            this.SexChooser = new List<string>()
            {
                male,
                female,
                bothSex
            };
        }

        private void SaveCongratulation()
        {
            var errors = new List<string>();
            bool hasNoErrorsInModel = this.CongratulationModel.IsAllFilledCorrect(out errors);

            if (!hasNoErrorsInModel)
            {
                string errorStrings = string.Empty;

                foreach (var error in errors)
                    errorStrings += (error + "\r\n");

                MessageBox.Show(errorStrings);

                return;
            }

            var congratulation =
                dbContext.CongratulationsDbModel.
                    FirstOrDefault(
                        congrat =>
                            congrat.Id == this.CongratulationModel.Id);

            if (congratulation != null)
            {
                var entry = dbContext.Entry(congratulation);

                entry.CurrentValues.
                    SetValues(
                        CongratulationModel);
            }
            else
            {
                var allCongrats = dbContext.CongratulationsDbModel.ToList();
                dbContext.CongratulationsDbModel.Add(CongratulationModel);
            }

            dbContext.SaveChanges();
            _notification.Confirmed = true;
            FinishInteraction?.Invoke();
        }

        private int GetCongratulationModelSex()
        {
            if (SelectedSex.SequenceEqual(female))
                return 0;
            else if (SelectedSex.SequenceEqual(male))
                return 1;
            else if (SelectedSex.SequenceEqual(bothSex))
                return 2;
            else
                throw new ArgumentOutOfRangeException("WTF?!!! Only two sex in this world!");
        }

        private bool IsCongratulationCorrectlyFilled()
        {
            bool isSexChosen = (SelectedSex != null && SelectedSex.Length != 0);

            var errors = new List<string>();
            bool hasNoErrorsInModel = this.CongratulationModel.IsAllFilledCorrect(out errors);

            return hasNoErrorsInModel && isSexChosen;
        }

        private void AbortAppending()
        {
            FinishInteraction?.Invoke();
        }

        private void ChooseContentTypeInModel(string contentType)
        {
            this.CongratulationModel.Kind = contentType.ToString(); // DEBUG - сделать нормальный класс для вида контента
        }
    }
}
