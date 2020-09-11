using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TulipWpfUI.EventModels;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<RegisterEvent>,
        IHandle<LogInEvent>, IHandle<InsertProductsEvent>
    {
       
        private IEventAggregator _events;
        private ProductsViewModel _productsVM;
        private readonly ILoggedInUserModel _user;

        //private SimpleContainer _container; using IoC instead from Caliburn.Micro

        public ShellViewModel( IEventAggregator events,
            ProductsViewModel productsVM, ILoggedInUserModel user)
        {
    

            _events = events;
            _events.Subscribe(this);

            _productsVM = productsVM;
            _user = user;
            //ActivateItem(_container.GetInstance<LoginViewModel>());
            // Simpler way 
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_productsVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(RegisterEvent message)
        {
            ActivateItem(IoC.Get<RegisterViewModel>());
        }

        public void Handle(LogInEvent message)
        {
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(InsertProductsEvent message)
        {
            ActivateItem(IoC.Get<InsertProductsViewModel>());
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }

        }

        
    }
}
