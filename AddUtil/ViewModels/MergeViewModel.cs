using AddUtil.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AddUtil.ViewModels
{
    /// <summary>
    /// Прослойка для перекопирования старой базы в новую.
    /// </summary>
    public class MergeViewModel : BindableBase
    {
        private RelayCommand abortCommand;
        public RelayCommand AbortCommand
        {
            get => abortCommand ?? (abortCommand = new RelayCommand((obj) => this.Abort()));
        }

        private void Abort()
        {
            //var displayRoot = (Application.Current as App).DisplayRootRegistry;
            //displayRoot.HidePresentation(this);
        }
    }
}
