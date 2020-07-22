using Caliburn.Micro;
using FollowFocusInterface.Models;
using FollowFocusInterface.Networking;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace FollowFocusInterface.ViewModels
{
    public class ShellViewModel : Conductor<Screen>.Collection.AllActive
    {

        #region Window Control

        private WindowState windowState;
        public WindowState WindowState
        {
            get { return windowState; }
            set
            {
                windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }

        public void MaximizeWindow()
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        public void MinimizeWindow()
        {
            WindowState = WindowState.Minimized;
        }

        public bool myCondition { get { return (false); } }

        public void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Private Members
        public NetworkingViewModel NetworkingViewModel { get; set; }
        public LoggerViewModel LoggerViewModel { get; set; }

        #endregion

        #region Constructor

        public ShellViewModel(
            NetworkingViewModel networkingViewModel,
            LoggerViewModel loggerViewModel
            )
        {
            // View model screens initialisation
            NetworkingViewModel = networkingViewModel;
            LoggerViewModel = loggerViewModel;
        }

        #endregion

    }
}
