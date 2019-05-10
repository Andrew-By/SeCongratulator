using AddUtil.Commands;
using AddUtil.Db;
using AddUtil.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private RelayCommand goToCongratulationAppendingCommand;
        public RelayCommand GoToCongratulationAppendingCommand
        {
            get
            {
                return 
                    goToCongratulationAppendingCommand ?? 
                        (goToCongratulationAppendingCommand = 
                            new RelayCommand(
                                obj => this.GoToCongratulationAppending()));
            }
        }

        private RelayCommand removeRecordCommand;
        public RelayCommand RemoveRecordCommand
        {
            get
            {
                return removeRecordCommand ??
                    (removeRecordCommand =
                        new RelayCommand(
                            obj => this.DeleteCongratulation(),
                            a => this.IsCongratulationSelected()));
            }
        }

        private RelayCommand goToMergeCommand;
        public RelayCommand GoToMergeCommand
        {
            get => goToMergeCommand ?? (goToMergeCommand = new RelayCommand(obj => this.GoToMergeWithOldDb()));
        }

        private RelayCommand goToCongratulationEditCommand;
        public RelayCommand GoToCongratulationEditCommand
        {
            get
            {
                return
                    goToCongratulationEditCommand ??
                        (goToCongratulationEditCommand =
                            new RelayCommand(
                                obj => this.GoToCongratulationEdit(), 
                                (a) => this.IsCongratulationSelected()));
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
