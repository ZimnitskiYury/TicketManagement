using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    public class DeleteVenueViewModel : BindableBase, IDialogAware
    {
        private readonly IVenueApiService apiService;
        private DelegateCommand<string> _closeDialogCommand;

        public DeleteVenueViewModel(VenueApiService apiService)
        {
            this.apiService = apiService;
        }

        #region VenueModel for view

        private VenueModel venue = new VenueModel();

        public VenueModel Venue
        {
            get => venue;
            set => SetProperty(ref venue, value);
        }

        #endregion VenueModel for view

        #region Dialog Functionality

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public string Title => "Delete Venue";

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Venue = parameters.GetValue<VenueModel>("venue");
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
                _ = apiService.DeleteVenueAsync(venue.Id).Result;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        #endregion Dialog Functionality
    }
}