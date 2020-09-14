using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TulipWpfUI.EventModels;
using TulipWpfUI.Library.Api;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly IAPIHelper _apiHelper;
        private readonly IEventAggregator _events;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        public UserDisplayViewModel(IAPIHelper apiHelper, IEventAggregator events,
            StatusInfoViewModel status, IWindowManager window)
        { 
            _apiHelper = apiHelper;
            _events = events;
            _status = status;
            _window = window;
        }

        protected override async void OnViewLoaded(object view)
        {

            base.OnViewLoaded(view);
            try
            {

                await LoadUsers();

            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocationLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Order Form.");
                    _window.ShowDialog(_status, null, settings);
                    _events.PublishOnUIThread(new LogOnEvent());
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _window.ShowDialog(_status, null, settings);
                    _events.PublishOnUIThread(new LogOnEvent());
                }
            }

        }


        private async Task LoadUsers()
        {

            var usersList = await _apiHelper.GetAllUsersInfo();
            Users = new BindingList<ApplicationUserModel>(usersList);
        }

        private BindingList<ApplicationUserModel> _users;

        public BindingList<ApplicationUserModel> Users
        {
            get 
            { 
                return _users; 
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
                NotifyOfPropertyChange(() => IsAdmin);
                NotifyOfPropertyChange(() => IsEmpty);
            }

        }



        public bool IsAdmin
        {
            get
            {
                if (Users?.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (Users?.Count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        public void BackToProduct()
        {
            _events.PublishOnUIThread(new LogOnEvent());
        }
    }
}
