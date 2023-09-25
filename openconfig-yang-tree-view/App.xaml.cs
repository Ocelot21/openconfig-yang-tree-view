using openconfig_yang_tree_view.Mappings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using openconfig_yang_tree_view.MVVM.ViewModels;
using openconfig_yang_tree_view.Core;
using openconfig_yang_tree_view.Services;
using openconfig_yang_tree_view.MVVM.Views;

namespace openconfig_yang_tree_view
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<FilesViewModel>();
            services.AddSingleton<TreeViewModel>();
            services.AddSingleton<SettingsViewModel>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IFilesService, FilesService>();

            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider => viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));


            _serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
            MappingConfig.RegisterMappings();
        }
    }
}
