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
    public class DisplayInventoryViewModel : Screen
    {
        private readonly IInventoryEndPoint _inventoryEndPoint;
        private readonly IEventAggregator _events;

        public DisplayInventoryViewModel(IInventoryEndPoint inventoryEndPoint, IEventAggregator events)
        {
            _inventoryEndPoint = inventoryEndPoint;
            _events = events;
        }

        protected override async void OnViewLoaded(object view)
        {

            base.OnViewLoaded(view);
            await LoadReports();

        }

        private async Task LoadReports()
        {

            var inventoryReport = await _inventoryEndPoint.GetInventoryReport();
            InventoryReport = inventoryReport;
        }

        private List<InventoryDisplayModel> _inventoryReport;

        public List<InventoryDisplayModel> InventoryReport
        {
            get { return _inventoryReport; }
            set
            {
                _inventoryReport = value;
                NotifyOfPropertyChange(() => InventoryReport);
            }
        }

        public void BackToProduct()
        {
            _events.PublishOnUIThread(new LogOnEvent());
        }
    }
}
