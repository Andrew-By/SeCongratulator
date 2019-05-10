using Prism.Commands;
using Prism.Mvvm;
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
        private ICommand abortCommand;
        public ICommand AbortCommand
        {
            get => abortCommand ?? (abortCommand = new DelegateCommand(Abort));
        }

        private void Abort()
        {
            //var displayRoot = (Application.Current as App).DisplayRootRegistry;
            //displayRoot.HidePresentation(this);
        }
    }
}
