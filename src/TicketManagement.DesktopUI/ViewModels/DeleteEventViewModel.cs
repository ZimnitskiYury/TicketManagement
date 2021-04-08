using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    public class DeleteEventViewModel : BindableBase, IDialogAware
    {
        private readonly IEventApiService apiService;
        private DelegateCommand<string> _closeDialogCommand;

        public DeleteEventViewModel(EventApiService apiService)
        {
            this.apiService = apiService;
        }

        #region EventModel for view

        private EventModel eve = new EventModel();

        public EventModel Eve
        {
            get => eve;
            set => SetProperty(ref eve, value);
        }

        #endregion EventModel for view

        #region Dialog Functionality

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public string Title => "Delete Event";

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Eve = parameters.GetValue<EventModel>("event");
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
                _ = apiService.DeleteEventAsync(eve.Id).Result;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        #endregion Dialog Functionality
    }
}