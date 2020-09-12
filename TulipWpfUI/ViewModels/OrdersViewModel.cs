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
    public class OrdersViewModel : Screen
    {
        private readonly IOrderEndPoint _orderEndPoint;
        private readonly IEventAggregator _events;

        public OrdersViewModel(IOrderEndPoint orderEndPoint, IEventAggregator events)
        {
            _orderEndPoint = orderEndPoint;
            _events = events;
        }

        protected override async void OnViewLoaded(object view)
        {

            base.OnViewLoaded(view);
            await LoadReports();

        }

        private async Task LoadReports()
        {

            var ordersReportList = await _orderEndPoint.GetOrdersReport();
            Orders = ordersReportList;
        }

        private List<OrdersReportModel> _orders;

        public List<OrdersReportModel> Orders
        {
            get { return _orders; }
            set 
            {
                _orders = value;
                NotifyOfPropertyChange(() => Orders);
            }
        }

       public void BackToProduct()
        {
            _events.PublishOnUIThread(new LogOnEvent());
        }
    }
}
