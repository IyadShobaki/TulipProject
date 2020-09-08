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
        private BindingList<ProductModel> _products;
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
            Products = new BindingList<ProductModel>(productList);
        }

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set 
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private int _itemQuantity;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }




        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                if (ItemQuantity > 0 )
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart(ProductModel product)
        {

            MessageBox.Show($"{product.Id}");
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
