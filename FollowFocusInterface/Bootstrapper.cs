using Caliburn.Micro;
using FollowFocusInterface.ViewModels;
using System.Windows;

namespace FollowFocusInterface
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
