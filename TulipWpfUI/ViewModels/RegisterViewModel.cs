using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipWpfUI.EventModels;

namespace TulipWpfUI.ViewModels
{
    public class RegisterViewModel : Screen
    {
        private readonly IEventAggregator _events;

        public RegisterViewModel(IEventAggregator events)
        {
            _events = events;
        }
        public void LogIn()
        {
            _events.PublishOnUIThread(new LogInEvent());
        }
    }
}
