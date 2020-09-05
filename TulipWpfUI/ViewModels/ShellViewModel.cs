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
        IHandle<LogInEvent>
    {
       
        private IEventAggregator _events;
        private ProductsViewModel _productsVM;
        private SimpleContainer _container;

        public ShellViewModel( IEventAggregator events,
            ProductsViewModel productsVM, SimpleContainer container)
        {
    

            _events = events;
            _events.Subscribe(this);

            _productsVM = productsVM;
            _container = container;
            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_productsVM);
        }

        public void Handle(RegisterEvent message)
        {
            ActivateItem(_container.GetInstance<RegisterViewModel>());
        }

        public void Handle(LogInEvent message)
        {
            ActivateItem(_container.GetInstance<LoginViewModel>());
        }
    }
}
