using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TicketManagement.DesktopUI.Models;
using TicketManagement.DesktopUI.Services;
using TicketManagement.DesktopUI.Services.Interfaces;

namespace TicketManagement.DesktopUI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        #region Services

        private readonly IDialogService _dialogService;
        private readonly IEventApiService eventApiService;
        private readonly IUserApiService userApiService;
        private readonly IVenueApiService venueApiService;

        #endregion Services

        #region Property for Collections

        private IEnumerable<EventModel> events;
        private IEnumerable<ProfileModel> users;
        private IEnumerable<VenueModel> venues;
        public IEnumerable<EventModel> Eve { get => events; set => SetProperty(ref events, value); }
        public IEnumerable<ProfileModel> Users { get => users; set => SetProperty(ref users, value); }
        public IEnumerable<VenueModel> Venues { get => venues; set => SetProperty(ref venues, value); }

        #endregion Property for Collections

        #region Event url property

        private string eventUrl;

        public string EventUrl
        {
            get => eventUrl;
            set
            {
                var uri = @"localhost:44356/event/" + value;
                SetProperty(ref eventUrl, uri);
            }
        }

        #endregion Event url property

        #region Property for selected objects

        private EventModel selectedEvent;
        private int selectedTab;
        private ProfileModel selectedUser;
        private VenueModel selectedVenue;

        public EventModel SelectedEvent
        {
            get => selectedEvent;
            set
            {
                SetProperty(ref selectedEvent, value);
                EventUrl = value.Id.ToString();
            }
        }

        public int SelectedTab
        {
            get => selectedTab;
            set
            {
                SetProperty(ref selectedTab, value);
            }
        }

        public ProfileModel SelectedUser
        {
            get => selectedUser; set
            {
                SetProperty(ref selectedUser, value);
            }
        }

        public VenueModel SelectedVenue
        {
            get => selectedVenue;
            set
            {
                SetProperty(ref selectedVenue, value);
            }
        }

        #endregion Property for selected objects

        #region User data

        private AuthenticatedUserModel authenticatedUser;

        public AuthenticatedUserModel AuthenticatedUser
        {
            get => authenticatedUser;
            set => SetProperty(ref authenticatedUser, value);
        }

        #endregion User data

        #region IsActive Tabs

        private bool isActiveEventManagement;
        private bool isActiveUserManagement;
        private bool isActiveVenueManagement;

        public bool IsActiveEventManagement
        {
            get
            {
                return isActiveEventManagement;
            }
            set => SetProperty(ref isActiveEventManagement, value);
        }

        public bool IsActiveUserManagement
        {
            get
            {
                return isActiveUserManagement;
            }
            set => SetProperty(ref isActiveUserManagement, value);
        }

        public bool IsActiveVenueManagement
        {
            get
            {
                return isActiveVenueManagement;
            }
            set => SetProperty(ref isActiveVenueManagement, value);
        }

        #endregion IsActive Tabs

        #region CloseApplicationCommand

        public DelegateCommand CloseApplicationCommand { get; private set; }

        private bool CanCloseApplicationCommandExecute() => true;

        private void OnCloseApplicationCommandExecuted()
        {
            Application.Current.Shutdown();
        }

        #endregion CloseApplicationCommand

        #region AddEventCommand

        public DelegateCommand AddEventCommand { get; private set; }

        private bool CanAddEventCommandExecute() => true;

        private void OnAddEventCommandExecuted()
        {
            _dialogService.ShowDialog("AddEventView", new DialogParameters(), r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You are succesfully added new event. \n List of events updated.", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("All unsaved changes are lost", "Operation cancelled");
                GetEventsAsync();
            });
        }

        #endregion AddEventCommand

        #region DeleteEventCommand

        public DelegateCommand DeleteEventCommand { get; private set; }

        private bool CanDeleteEventCommandExecute()
        {
            return SelectedEvent != null;
        }

        private void OnDeleteEventCommandExecuted()
        {
            var dialogParameters = new DialogParameters
            {
                { "event", SelectedEvent }
            };

            _dialogService.ShowDialog("DeleteEventView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You have successfully deleted the event. \nList of events updated.", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("You canceled the delete operation", "Operation cancelled");
                GetEventsAsync();
            });
        }

        #endregion DeleteEventCommand

        #region EditEventCommand

        public DelegateCommand EditEventCommand { get; private set; }

        private bool CanEditEventCommandExecute()
        {
            return SelectedEvent != null;
        }

        private void OnEditEventCommandExecuted()
        {
            var dialogParameters = new DialogParameters
            {
                { "event", SelectedEvent }
            };

            _dialogService.ShowDialog("EditEventView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You have successfully edit the event. \n List of events updated. ", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("All unsaved changes are lost", "Operation cancelled");
                GetEventsAsync();
            });
        }

        #endregion EditEventCommand

        #region AddVenueCommand

        public DelegateCommand AddVenueCommand { get; private set; }

        private bool CanAddVenueCommandExecute() => true;

        private void OnAddVenueCommandExecuted()
        {
            _dialogService.ShowDialog("AddVenueView", new DialogParameters(), r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You are succesfully added new Venue. \n List of Venues updated. ", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("All unsaved changes are lost", "Operation cancelled");
                GetVenuesAsync();
            });
        }

        #endregion AddVenueCommand

        #region DeleteVenueCommand

        public DelegateCommand DeleteVenueCommand { get; private set; }

        private bool CanDeleteVenueCommandExecute()
        {
            return SelectedVenue != null;
        }

        private void OnDeleteVenueCommandExecuted()
        {
            var dialogParameters = new DialogParameters
            {
                { "venue", SelectedVenue }
            };

            _dialogService.ShowDialog("DeleteVenueView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You have successfully deleted the Venue. \n List of Venues updated. ", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("You canceled the delete operation", "Operation cancelled");
                GetVenuesAsync();
            });
        }

        #endregion DeleteVenueCommand

        #region EditVenueCommand

        public DelegateCommand EditVenueCommand { get; private set; }

        private bool CanEditVenueCommandExecute()
        {
            return SelectedVenue != null;
        }

        private void OnEditVenueCommandExecuted()
        {
            var dialogParameters = new DialogParameters
            {
                { "venue", SelectedVenue }
            };

            _dialogService.ShowDialog("EditVenueView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You have successfully edit the Venue. \n List of Venues updated. ", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("All unsaved changes are lost", "Operation cancelled");
                GetVenuesAsync();
            });
        }

        #endregion EditVenueCommand

        #region EditRolesCommand

        public DelegateCommand EditRolesCommand { get; private set; }

        private bool CanEditRolesCommandExecute() => SelectedUser != null;

        private void OnEditRolesCommandExecuted()
        {
            var dialogParameters = new DialogParameters
            {
                { "user", SelectedUser }
            };

            _dialogService.ShowDialog("EditRolesView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You are succesfully edited roles for user. \n List of Users updated. ", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("All unsaved changes are lost", "Operation cancelled");
                GetUsersAsync();
            });
        }

        #endregion EditRolesCommand

        #region DeleteUserCommand

        public DelegateCommand DeleteUserCommand { get; private set; }

        private bool CanDeleteUserCommandExecute()
        {
            return SelectedUser != null;
        }

        private void OnDeleteUserCommandExecuted()
        {
            var dialogParameters = new DialogParameters
            {
                { "user", SelectedUser }
            };

            _dialogService.ShowDialog("DeleteUserView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You have successfully deleted the User. \n List of Users updated. ", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("You canceled the delete operation", "Operation cancelled");
                GetUsersAsync();
            });
        }

        #endregion DeleteUserCommand

        #region EditUserCommand

        public DelegateCommand EditUserCommand { get; private set; }

        private bool CanEditUserCommandExecute()
        {
            return SelectedUser != null;
        }

        private void OnEditUserCommandExecuted()
        {
            var dialogParameters = new DialogParameters
            {
                { "user", SelectedUser }
            };

            _dialogService.ShowDialog("EditUserView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                    MessageBox.Show("You have successfully edit the User. \n List of Users updated. ", "Changes saved");
                else if (r.Result == ButtonResult.Cancel)
                    MessageBox.Show("All unsaved changes are lost", "Operation cancelled");
                GetUsersAsync();
            });
        }

        #endregion EditUserCommand

        public ShellViewModel(IDialogService dialogService, EventApiService eventApiService, VenueApiService venueApiService, UserApiService userApiService, AuthenticatedUserModel authenticatedUser)
        {
            _dialogService = dialogService;
            AuthenticatedUser = authenticatedUser;
            this.venueApiService = venueApiService;
            this.eventApiService = eventApiService;
            this.userApiService = userApiService;
            DeleteEventCommand = new DelegateCommand(OnDeleteEventCommandExecuted, CanDeleteEventCommandExecute).ObservesProperty<object>(() => SelectedEvent);
            AddEventCommand = new DelegateCommand(OnAddEventCommandExecuted, CanAddEventCommandExecute);
            EditEventCommand = new DelegateCommand(OnEditEventCommandExecuted, CanEditEventCommandExecute).ObservesProperty<object>(() => SelectedEvent);
            DeleteVenueCommand = new DelegateCommand(OnDeleteVenueCommandExecuted, CanDeleteVenueCommandExecute).ObservesProperty<object>(() => SelectedVenue);
            AddVenueCommand = new DelegateCommand(OnAddVenueCommandExecuted, CanAddVenueCommandExecute);
            EditVenueCommand = new DelegateCommand(OnEditVenueCommandExecuted, CanEditVenueCommandExecute).ObservesProperty<object>(() => SelectedVenue);
            EditUserCommand = new DelegateCommand(OnEditUserCommandExecuted, CanEditUserCommandExecute).ObservesProperty<object>(() => SelectedUser);
            DeleteUserCommand = new DelegateCommand(OnDeleteUserCommandExecuted, CanDeleteUserCommandExecute).ObservesProperty<object>(() => SelectedUser);
            EditRolesCommand = new DelegateCommand(OnEditRolesCommandExecuted, CanEditRolesCommandExecute).ObservesProperty<object>(() => SelectedUser);
            CloseApplicationCommand = new DelegateCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            LoginViewModel.LoginCompleted += (sender, args) =>
            {
                if (AuthenticatedUser.Roles.Contains("Admin"))
                {
                    IsActiveVenueManagement = true;
                    IsActiveEventManagement = true;
                    IsActiveUserManagement = true;
                    SelectedTab = 2;
                }
                else if (AuthenticatedUser.Roles.Contains("Venue") && AuthenticatedUser.Roles.Contains("Event"))
                {
                    IsActiveVenueManagement = true;
                    IsActiveEventManagement = true;
                    SelectedTab = 1;
                }
                else if (AuthenticatedUser.Roles.Contains("Venue"))
                {
                    IsActiveVenueManagement = true;
                    SelectedTab = 1;
                }
                else
                {
                    IsActiveEventManagement = true;
                    SelectedTab = 0;
                }
                if (IsActiveUserManagement)
                {
                    GetUsersAsync();
                }
                if (IsActiveVenueManagement)
                {
                    GetVenuesAsync();
                }
                if (IsActiveEventManagement)
                {
                    GetEventsAsync();
                }
            };
        }

        private async void GetEventsAsync()
        {
            Eve = await eventApiService.GetAllAsync();
        }

        private async void GetUsersAsync()
        {
            Users = await userApiService.GetAllAsync();
        }

        private async void GetVenuesAsync()
        {
            Venues = await venueApiService.GetAllAsync();
        }
    }
}