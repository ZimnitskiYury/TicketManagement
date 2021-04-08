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
    public class AddEventViewModel : BindableBase, IDialogAware
    {
        private readonly IEventApiService apiService;

        public AddEventViewModel(EventApiService eventApi)
        {
            apiService = eventApi;
        }

        /// <summary>
        /// Helper for enum to Combobox
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> CategoryList => EnumHelper.GetAllValuesAndDescriptions<Category>();

        #region Property of Event
        private Category category = Category.Cinema;
        private string description = "";
        private DateTime endDate = DateTime.Now.AddDays(1);
        private int layoutId;
        private string name = "";

        private DateTime startDate = DateTime.Now;

        public Category Category
        {
            get { return category; }
            set { SetProperty(ref category, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        public int LayoutId
        {
            get { return layoutId; }
            set { SetProperty(ref layoutId, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }
        #endregion

        #region Dialog Functionality
        private DelegateCommand<string> _closeDialogCommand;

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public string Title => "Add Event";

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
                _ = apiService.AddEventAsync(new EventModel
                {
                    Category = this.Category,
                    LayoutId = this.LayoutId,
                    StartDateTime = this.StartDate,
                    EndDateTime = this.EndDate,
                    Name = this.Name,
                    Description = this.Description,
                }).Result;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }
        #endregion
    }
}
