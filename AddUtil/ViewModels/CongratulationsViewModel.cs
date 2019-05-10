using AddUtil.Db;
using AddUtil.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AddUtil.ViewModels
{
    public class CongratulationsViewModel : BindableBase
    {
        //
        // Fields and properties.
        //

        private List<CongratulationModel> congratulations;
        public List<CongratulationModel> Congratulations
        {
            get => congratulations;
            set => SetProperty(ref congratulations, value);
        }

        private CongratulationModel selectedCongratulation;
        public CongratulationModel SelectedCongratulation
        {
            get => selectedCongratulation;
            set
            {
                SetProperty(ref selectedCongratulation, value);
                GoToCongratulationEditCommand.RaiseCanExecuteChanged();
                RemoveRecordCommand.RaiseCanExecuteChanged();
            }
        }

        //
        // Commands.
        //

        private ICommand goToCongratulationAppendingCommand;
        public ICommand GoToCongratulationAppendingCommand
        {
            get
            {
                return
                    goToCongratulationAppendingCommand ??
                        (goToCongratulationAppendingCommand =
                            new DelegateCommand(GoToCongratulationAppending));
            }
        }

        private DelegateCommand removeRecordCommand;
        public DelegateCommand RemoveRecordCommand
        {
            get
            {
                return removeRecordCommand ??
                    (removeRecordCommand =
                        new DelegateCommand(DeleteCongratulation, IsCongratulationSelected));
            }
        }

        private ICommand goToMergeCommand;
        public ICommand GoToMergeCommand
        {
            get => goToMergeCommand ?? (goToMergeCommand = new DelegateCommand(GoToMergeWithOldDb));
        }

        private DelegateCommand goToCongratulationEditCommand;
        public DelegateCommand GoToCongratulationEditCommand
        {
            get
            {
                return
                    goToCongratulationEditCommand ??
                        (goToCongratulationEditCommand =
                            new DelegateCommand(GoToCongratulationEdit, IsCongratulationSelected));
            }
        }

        //
        // Constructors.
        //

        public CongratulationsViewModel()
        {
            this.InitCongratulationsCollection();
        }

        //
        // Private zone.
        //
        private void InitCongratulationsCollection()
        {
            using (CongratulationDbContext db = new CongratulationDbContext())
            {
                Congratulations = new List<CongratulationModel>();
                Congratulations = db.CongratulationsDbModel.ToList();
            }
        }

        private void DeleteCongratulation()
        {
            if (SelectedCongratulation == null)
                return;

            using (var db = new CongratulationDbContext())
            {
                var deletingCongratulation = db.CongratulationsDbModel.FirstOrDefault(congrat => congrat.Id == SelectedCongratulation.Id);
                db.CongratulationsDbModel.Remove(deletingCongratulation);
                SelectedCongratulation = null;
                db.SaveChanges();
            }
            this.UpdateContext();
        }

        private async void GoToCongratulationAppending()
        {
            //var displayRootRegistry = (Application.Current as App).DisplayRootRegistry;

            //var newCongratulationViewModel = new NewCongratulationViewModel();
            //await displayRootRegistry.ShowModalPresentation(newCongratulationViewModel);

            this.UpdateContext();
        }

        private async void GoToCongratulationEdit()
        {
            //if (SelectedCongratulation == null)
            //{
            //    MessageBox.Show(
            //        "Выберите поздравление для редактирования.",
            //        "Ошибка редактирования");

            //    return;
            //}

            //var displayRootRegistry = (Application.Current as App).DisplayRootRegistry;

            //await displayRootRegistry.ShowModalPresentation(
            //    new NewCongratulationViewModel(
            //        this.SelectedCongratulation));

            this.UpdateContext();
        }

        private bool IsCongratulationSelected() => SelectedCongratulation != null;

        private void UpdateContext() => this.InitCongratulationsCollection();

        // Перенос из одной базы в другую - старая база должна подаваться аргументом, на выходе новая база, заполненная значениями со старой.
        private async void GoToMergeWithOldDb()
        {
            //var displayRootRegistry = (Application.Current as App).DisplayRootRegistry;

            //await displayRootRegistry.ShowModalPresentation(new MergeViewModel());

            //this.UpdateContext();
        }
    }
}
