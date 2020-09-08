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
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.ViewModels
{
    public class ProductsViewModel : Screen
    {
        private IProductEndPoint _productEndPoint;
        private readonly IEventAggregator _events;

        public ProductsViewModel(IProductEndPoint productEndPoint, IEventAggregator events)
        {
            _productEndPoint = productEndPoint;
            _events = events;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
            
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            Products.AddRange(productList.Select(x => CreateProductViewModel(x)));
        }

        private ProductViewModel CreateProductViewModel(ProductModel product)
        {
         
            var productViewModel =  new ProductViewModel(product);
            productViewModel.AddTCart += OnProductAdd;
            productViewModel.RemoveFCart += OnProductRemove;
            return productViewModel;
        }

        private void OnProductRemove(object sender, EventArgs e)
        {
            var productToAdd = (ProductViewModel)sender;

            Cart.Remove(productToAdd);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        private void OnProductAdd(object sender, EventArgs e)
        {
            var productToAdd = (ProductViewModel)sender;

            Cart.Add(productToAdd);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
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

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += item.SubTotal;
            }

            return subTotal;
        }
        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
          
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            foreach (var item in Cart)
            {
                taxAmount += item.Tax;
            }

            return taxAmount;
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();

                return total.ToString("C");
            }
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
