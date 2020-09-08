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
            Products.AddRange(productList.Select(x => CreateProduct(x)));
        }

        private ProductViewModel CreateProduct(ProductModel product)
        {
         
            var productViewModel =  new ProductViewModel(product);
            productViewModel.AddTCart += OnProductAdd;
            return productViewModel;
        }

        private void OnProductAdd(object sender, EventArgs e)
        {
            var productToAdd = (ProductViewModel)sender;
            //Products.Remove(productToAdd);
            Cart.Add(productToAdd);
            
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
