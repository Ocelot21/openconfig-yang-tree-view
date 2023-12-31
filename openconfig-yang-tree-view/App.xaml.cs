﻿using openconfig_yang_tree_view.Mappings;
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
using System.Windows.Threading;

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
            DispatcherUnhandledException += App_DispatcherUnhandledException;
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
            services.AddSingleton<ISettingsService, SettingsService>();

            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider => viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));


            _serviceProvider = services.BuildServiceProvider();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred:\n" + e.Exception.Message + "\n" + e.Exception.Source, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
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
