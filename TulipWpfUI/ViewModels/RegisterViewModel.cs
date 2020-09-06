using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipWpfUI.EventModels;
using TulipWpfUI.Library.Api;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.ViewModels
{
    public class RegisterViewModel : Screen
    {
        private readonly IAPIHelper _apiHelper;
        private readonly IEventAggregator _events;

        public RegisterViewModel(IAPIHelper apiHelper,IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            { 
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => CanSubmit);
            }
        }


        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(() => CanSubmit);
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set 
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanSubmit);
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanSubmit);
            }
        }

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                NotifyOfPropertyChange(() => ConfirmPassword);
                NotifyOfPropertyChange(() => CanSubmit);
            }
        }



        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output;
            }

        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {

                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);



            }
        }

        public bool CanSubmit
        {
            get
            {

                bool output = false;

                if (Email?.Length > 0 && Password?.Length > 0 && ConfirmPassword?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }
        public async Task Submit()
        {
            try
            {
                ErrorMessage = "New Account has been created!";
                string createSuccess = await _apiHelper.RegisterUser(Email, Password, ConfirmPassword);

                if (createSuccess == "success")
                {
                    var result = await _apiHelper.Authenticate(Email, Password);

                    UserModel user = new UserModel();
                    user.Id = await _apiHelper.GetUserId(result.Access_Token);
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.EmailAddress = Email;

                    await _apiHelper.PostUserInfo(user);

                    _events.PublishOnUIThread(new LogInEvent());
                }
                
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }




        public void LogIn()
        {
            _events.PublishOnUIThread(new LogInEvent());
        }

        public void ResetFields()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Password = "";
            ConfirmPassword = "";
        }
    }
}
