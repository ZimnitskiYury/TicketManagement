using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using TicketManagement.DesktopUI.Helper;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    public class EditEventViewModel : BindableBase, IDialogAware
    {
        private readonly IEventApiService apiService;
        private DelegateCommand<string> _closeDialogCommand;

        public EditEventViewModel(EventApiService eventApi)
        {
            apiService = eventApi;
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

        public IEnumerable<KeyValuePair<string, string>> CategoryList => EnumHelper.GetAllValuesAndDescriptions<Category>();

        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public string Title => "Edit Event";

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
                _ = apiService.UpdateEventAsync(eve).Result;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        #endregion Dialog Functionality
    }
}