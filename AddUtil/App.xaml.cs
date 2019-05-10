using AddUtil.ViewModels;
using AddUtil.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism;
using Prism.Unity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AddUtil
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        //public DisplayRootRegistry DisplayRootRegistry = new DisplayRootRegistry();
        //CongratulationsViewModel congratulationsViewModel;

        private IRegionManager _regionManager;

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _regionManager = Container.Resolve<IRegionManager>();
            _regionManager.Regions["ContentRegion"].RequestNavigate(new Uri("CongratulationsView", UriKind.RelativeOrAbsolute));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CongratulationsView>();
            containerRegistry.RegisterForNavigation<MergeView>();
            containerRegistry.RegisterForNavigation<NewCongratulationView>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        //protected override async void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    congratulationsViewModel = new CongratulationsViewModel();

        //    await DisplayRootRegistry.ShowModalPresentation(congratulationsViewModel);

        //}
    }
}
