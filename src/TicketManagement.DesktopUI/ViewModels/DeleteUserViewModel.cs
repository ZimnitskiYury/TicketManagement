using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    public class DeleteUserViewModel : BindableBase, IDialogAware
    {
        private readonly IUserApiService apiService;
        private DelegateCommand<string> _closeDialogCommand;

        public DeleteUserViewModel(UserApiService userService)
        {
            apiService = userService;
        }

        #region ProfileModel for view

        private ProfileModel user = new ProfileModel();

        public ProfileModel User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        #endregion ProfileModel for view

        #region Dialog Functionality

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public string Title => "Delete User";

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            User = parameters.GetValue<ProfileModel>("user");
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.OK;
                _ = apiService.DeleteUserAsync(user.Login).Result;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        #endregion Dialog Functionality
    }
}