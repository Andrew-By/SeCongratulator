using AddUtil.Db;
using AddUtil.Models;
using AddUtil.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
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

        private readonly IRegionManager _regionManager;

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

        public InteractionRequest<CongratulationConfirmation> AppendInteractionRequest { get; private set; }
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

        public InteractionRequest<CongratulationConfirmation> EditCongratulationRequest { get; private set; }
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

        public CongratulationsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            AppendInteractionRequest = new InteractionRequest<CongratulationConfirmation>();
            EditCongratulationRequest = new InteractionRequest<CongratulationConfirmation>();
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

        private void GoToCongratulationAppending()
        {
            AppendInteractionRequest?.Raise(new CongratulationConfirmation
            {
                Title = "Добавление поздравления",
                Congratulation = new CongratulationModel()
            },
            r =>
            {
                if (r.Confirmed)
                {
                    using (var context = new CongratulationDbContext())
                    {
                        context.CongratulationsDbModel.Add(r.Congratulation);
                        context.SaveChanges();
                    }
                }
            });
            this.UpdateContext();
        }

        private void GoToCongratulationEdit()
        {
            EditCongratulationRequest?.Raise(new CongratulationConfirmation
            {
                Title = "Редактирование поздравления",
                Congratulation = new CongratulationModel(SelectedCongratulation)
            },
            r =>
            {
                if (r.Confirmed)
                {
                    using (var context = new CongratulationDbContext())
                    {
                        context.CongratulationsDbModel.Add(r.Congratulation);
                        context.SaveChanges();
                    }
                }
            });
            this.UpdateContext();
        }

        private bool IsCongratulationSelected() => SelectedCongratulation != null;

        private void UpdateContext() => this.InitCongratulationsCollection();

        // Перенос из одной базы в другую - старая база должна подаваться аргументом, на выходе новая база, заполненная значениями со старой.
        private async void GoToMergeWithOldDb()
        {
            _regionManager.Regions["ContentRegion"].RequestNavigate(new Uri("MergeView", UriKind.RelativeOrAbsolute));
        }
    }
}
