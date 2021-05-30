using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using PSMDesktopApp.Library.Helpers;
using PSMDesktopApp.Library.Models;
using PSMDesktopApp.Utils;
using PSMDesktopApp.ViewModels;

namespace PSMDesktopApp
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        // This static constructor is called before any Caliburn.Micro code is called.
        static Bootstrapper()
        {
            // NLog doesn't automatically create directories for some reason.
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            LogManager.GetLog = type => new NLogLogger(type);
        }

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container
                .Instance(_container)
                .PerRequest<ITechnicianEndpoint, TechnicianEndpoint>()
                .PerRequest<IServiceEndpoint, ServiceEndpoint>()
                .PerRequest<ISparepartEndpoint, SparepartEndpoint>()
                .PerRequest<ISalesEndpoint, SalesEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IApiHelper, ApiHelper>()
                .Singleton<ISettingsHelper, SettingsHelper>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
