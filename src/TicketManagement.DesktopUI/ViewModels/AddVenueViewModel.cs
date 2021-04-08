using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    public class AddVenueViewModel : BindableBase, IDialogAware
    {
        private readonly IVenueApiService apiService;

        #region Property of Event

        private string address = "";

        private string description = "";

        private string phone = "";

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }

        #endregion Property of Event

        #region Dialog Functionality

        private DelegateCommand<string> _closeDialogCommand;

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public string Title => "Add Venue";

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
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
                _ = apiService.AddVenueAsync(new VenueModel
                {
                    Description = this.Description,
                    Address = this.Address,
                    Phone = this.Phone,
                }).Result;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        #endregion Dialog Functionality

        public AddVenueViewModel(VenueApiService venueApi)
        {
            apiService = venueApi;
        }
    }
}