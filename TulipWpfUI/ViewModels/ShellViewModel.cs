using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TulipWpfUI.EventModels;

namespace TulipWpfUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<RegisterEvent>,
        IHandle<LogInEvent>, IHandle<InsertProductsEvent>
    {
       
        private IEventAggregator _events;
        private ProductsViewModel _productsVM;
        //private SimpleContainer _container; using IoC instead from Caliburn.Micro

        public ShellViewModel( IEventAggregator events,
            ProductsViewModel productsVM)
        {
    

            _events = events;
            _events.Subscribe(this);

            _productsVM = productsVM;
            //ActivateItem(_container.GetInstance<LoginViewModel>());
            // Simpler way 
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_productsVM);
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
    }
}
