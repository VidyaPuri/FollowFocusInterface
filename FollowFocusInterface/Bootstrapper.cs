using Caliburn.Micro;
using FollowFocusInterface.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FollowFocusInterface
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer simpleContainer = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            simpleContainer.Instance(simpleContainer);
            simpleContainer.Singleton<ShellViewModel>();
            simpleContainer.Singleton<LoggerViewModel>();
            simpleContainer.Singleton<NetworkingViewModel>();

            simpleContainer
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return simpleContainer.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return simpleContainer.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            simpleContainer.BuildUp(instance);
        }
    }
}
