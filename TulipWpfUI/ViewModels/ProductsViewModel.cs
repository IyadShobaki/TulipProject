using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TulipWpfUI.EventModels;
using TulipWpfUI.Library.Api;
using TulipWpfUI.Library.Helpers;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.ViewModels
{
    public class ProductsViewModel : Screen
    {
        private IProductEndPoint _productEndPoint;
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _loggedInUserModel;
        private readonly IConfigHelper _configHelper;

        public ProductsViewModel(IProductEndPoint productEndPoint, IEventAggregator events,
            ILoggedInUserModel loggedInUserModel, IConfigHelper configHelper)
        {
            _productEndPoint = productEndPoint;
            _events = events;
            _loggedInUserModel = loggedInUserModel;
            _configHelper = configHelper;
        }
        
        protected override async void OnViewLoaded(object view)
        {
            
            base.OnViewLoaded(view);
            if (Products.Count > 0)
            {

            }
            else
            {
                await LoadProducts();
            }
            

        }

        private async Task LoadProducts()
        {
            
            var productList = await _productEndPoint.GetAll();
            Products.AddRange(productList.Select(x => CreateProductViewModel(x)));
        }

        private ProductViewModel CreateProductViewModel(ProductModel product)
        {
         
            var productViewModel =  new ProductViewModel(product, _configHelper);
            productViewModel.AddTCart += OnProductAdd;
            productViewModel.RemoveFCart += OnProductRemove;
            return productViewModel;
        }

        private void OnProductRemove(object sender, EventArgs e)
        {
            var productToAdd = (ProductViewModel)sender;

            Cart.Remove(productToAdd);
            NotifyOfPropertyChange(() => TotalSubTotal);
            NotifyOfPropertyChange(() => TotalTax);
            NotifyOfPropertyChange(() => TotalTotal);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        private void OnProductAdd(object sender, EventArgs e)
        {
            var productToAdd = (ProductViewModel)sender;

            Cart.Add(productToAdd);
            NotifyOfPropertyChange(() => TotalSubTotal);
            NotifyOfPropertyChange(() => TotalTax);
            NotifyOfPropertyChange(() => TotalTotal);
            NotifyOfPropertyChange(() => CanCheckOut);
        }  

        

        public BindableCollection<ProductViewModel> Products { get; } = new BindableCollection<ProductViewModel>();

        private BindingList<ProductViewModel> _cart = new BindingList<ProductViewModel>();

        public BindingList<ProductViewModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public string TotalSubTotal
        {
            get
            {
                return CalculateTotalSubTotal().ToString("C");
            }
        }

        private decimal CalculateTotalSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += item.SubTotal;
            }

            return subTotal;
        }
        public string TotalTax
        {
            get
            {
                return CalculateTotalTax().ToString("C");
            }
          
        }

        private decimal CalculateTotalTax()
        {
            decimal taxAmount = 0;
            foreach (var item in Cart)
            {
                taxAmount += item.Tax;
            }

            return taxAmount;
        }

        public string TotalTotal
        {
            get
            {
                decimal total = CalculateTotalSubTotal() + CalculateTotalTax();

                return total.ToString("C");
            }
        }


        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public void CheckOut()
        {     
            
           
        }

        public bool IsAdmin
        {
            get
            {
                //bool output = false;
                //if (ErrorMessage?.Length > 0)
                //{
                //    output = true;
                //}
                //return output;
                return true;
            }

        }

        public void Admin()
        { 
            
            _events.PublishOnUIThread(new InsertProductsEvent());
        }

    }
}
