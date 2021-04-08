using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Windows;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    internal class LoginViewModel : BindableBase
    {
        private readonly IUserApiService _userApi;
        private string login = "";
        private string password = "";

        public LoginViewModel(IUserApiService userApi, AuthenticatedUserModel authenticatedUser)
        {
            AuthenticatedUser = authenticatedUser;
            _userApi = userApi;
            LoginApplicationCommand = new DelegateCommand(OnLoginApplicationCommandExecutedAsync, CanLoginApplicationCommand).ObservesProperty<object>(() => Login).ObservesProperty<object>(() => Password);
            CloseApplicationCommand = new DelegateCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        }

        public string Login
        {
            get => login;
            set
            {
                SetProperty(ref login, value);
            }
        }

        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
            }
        }

        private AuthenticatedUserModel AuthenticatedUser { get; set; }

        #region CloseApplicationCommand

        public DelegateCommand CloseApplicationCommand { get; private set; }

        private bool CanCloseApplicationCommandExecute() => true;

        private void OnCloseApplicationCommandExecuted()
        {
            Application.Current.Shutdown();
        }

        #endregion CloseApplicationCommand

        #region EventHandler for closing login window

        public static event EventHandler LoginCompleted;

        private void RaiseLoginCompletedEvent() => LoginCompleted?.Invoke(this, EventArgs.Empty);

        #endregion EventHandler for closing login window

        #region Delegate for work with Enter button on login view

        public DelegateCommand LoginApplicationCommand { get; set; }

        private bool CanLoginApplicationCommand()
        {
            if (Login.Length >= 4 && Password.Length >= 4)
            {
                return true;
            }

            return false;
        }

        private async void OnLoginApplicationCommandExecutedAsync()
        {
            try
            {
                var user = await _userApi.AuthenticateAsync(Login, Password);
                AuthenticatedUser.Id = user.Id;
                AuthenticatedUser.FirstName = user.FirstName;
                AuthenticatedUser.SurName = user.SurName;
                AuthenticatedUser.Email = user.Email;
                AuthenticatedUser.Token = user.Token;
                AuthenticatedUser.Login = user.Login;
                AuthenticatedUser.Roles = user.Roles;
                if (user.Roles.Contains("Admin") || user.Roles.Contains("Event") || user.Roles.Contains("Venue"))
                {
                    RaiseLoginCompletedEvent();
                }
                else
                {
                    MessageBox.Show("You do not have permission to open the application.", "Authorization failed");
                }
            }
            catch
            {
                MessageBox.Show("Wrong credentials. Please check your login and password.", "Authentication failed");
            }
        }

        #endregion Delegate for work with Enter button on login view
    }
}