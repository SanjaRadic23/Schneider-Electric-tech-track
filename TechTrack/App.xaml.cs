using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TechTrack.Domain.IRepository;
using TechTrack.Repository;
using TechTrack.Service;
using TechTrack.ViewModel;

namespace TechTrack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceCollection _services;
        public static IServiceProvider _serviceProvider;

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            _services = new ServiceCollection();
            _services.AddSingleton<UserService>();
            _services.AddSingleton<IUserRepository, UserRepository>();
            _services.AddSingleton<UserAccountService>();
            _services.AddSingleton<IUserAccountRepository, UserAccountRepository>();

            _serviceProvider = _services.BuildServiceProvider();

            SignInForm signInForm = new SignInForm();
            signInForm.Show();

        }

    }
}
