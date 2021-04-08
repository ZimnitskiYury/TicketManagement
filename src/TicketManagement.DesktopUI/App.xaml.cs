using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using System.IO;
using System.Windows;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;
using TicketManagement.DesktopUI.ViewModels;
using TicketManagement.DesktopUI.Views;

namespace TicketManagement.DesktopUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        private static IConfiguration Configuration
        {
            get
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                return configurationBuilder.Build();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IConfiguration>(Configuration);

            containerRegistry.RegisterDialog<AddEventView, AddEventViewModel>();
            containerRegistry.RegisterDialog<AddVenueView, AddVenueViewModel>();

            containerRegistry.RegisterDialog<DeleteUserView, DeleteUserViewModel>();
            containerRegistry.RegisterDialog<DeleteEventView, DeleteEventViewModel>();
            containerRegistry.RegisterDialog<DeleteVenueView, DeleteVenueViewModel>();

            containerRegistry.RegisterDialog<EditEventView, EditEventViewModel>();
            containerRegistry.RegisterDialog<EditVenueView, EditVenueViewModel>();
            containerRegistry.RegisterDialog<EditUserView, EditUserViewModel>();
            containerRegistry.RegisterDialog<EditRolesView, EditRolesViewModel>();

            containerRegistry.RegisterSingleton<IUserApiService, UserApiService>();
            containerRegistry.RegisterSingleton<IEventApiService, EventApiService>();
            containerRegistry.RegisterSingleton<IVenueApiService, VenueApiService>();

            containerRegistry.RegisterSingleton<AuthenticatedUserModel>();
        }

        protected override void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg3ODgyQDMxMzgyZTM0MmUzMFU2SUdwWGRHQjFORjlZSk1nZmo1U3B5RStYZ3V2K0NaS3M2QW56aUJyVVE9");

            //base.OnInitialized();
            var login = Container.Resolve<LoginView>();
            LoginViewModel.LoginCompleted += (sender, args) =>
            {
                login.Hide();
                this.MainWindow.Show();
            };
            login.Show();
        }
    }
}
