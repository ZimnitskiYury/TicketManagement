using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    public class EditRolesViewModel : BindableBase, IDialogAware
    {
        private readonly IUserApiService apiService;
        private DelegateCommand<string> _closeDialogCommand;

        public EditRolesViewModel(UserApiService userApi)
        {
            apiService = userApi;
        }

        #region Collection of RoleModel's for view

        private List<RoleModel> roles = new List<RoleModel>();

        public List<RoleModel> Roles
        {
            get => roles;
            set => SetProperty(ref roles, value);
        }

        #endregion Collection of RoleModel's for view

        #region ProfileModel for view

        private ProfileModel user;

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

        public string Title => "Edit User's role";

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
            var availableRoles = apiService.GetRolesAsync().Result;
            foreach (var role in availableRoles)
            {
                if (User.Roles.Contains(role.Name))
                {
                    Roles.Add(new RoleModel
                    {
                        Name = role.Name,
                        IsSelected = true,
                    });
                }
                else
                {
                    Roles.Add(new RoleModel
                    {
                        Name = role.Name,
                        IsSelected = false
                    });
                }
            }
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
                _ = ChangesUserRolesAsync();
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        #endregion Dialog Functionality

        private async Task ChangesUserRolesAsync()
        {
            foreach (var role in Roles)
            {
                if (role.IsSelected == false && User.Roles.Contains(role.Name))
                {
                    await apiService.DeleteRoleAsync(User.Login, role.Name);
                }
                else if (role.IsSelected == true && !(User.Roles.Contains(role.Name)))
                {
                    await apiService.AddRoleAsync(User.Login, role.Name);
                }
            }
        }
    }
}