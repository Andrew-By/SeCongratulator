using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AddUtil.ViewModels
{
    /// <summary>
    /// Прослойка для перекопирования старой базы в новую.
    /// </summary>
    public class MergeViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MergeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private ICommand abortCommand;
        public ICommand AbortCommand
        {
            get => abortCommand ?? (abortCommand = new DelegateCommand(Abort));
        }

        private void Abort()
        {
            var navService = _regionManager.Regions["ContentRegion"].NavigationService;
            if (navService.Journal.CanGoBack)
                navService.Journal.GoBack();
        }
    }
}
