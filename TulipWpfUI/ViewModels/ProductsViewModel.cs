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
        private readonly IProductEndPoint _productEndPoint;
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _loggedInUserModel;
        private readonly IConfigHelper _configHelper;
        private readonly IOrderEndPoint _orderEndPoint;

        public ProductsViewModel(IProductEndPoint productEndPoint, IEventAggregator events,
            ILoggedInUserModel loggedInUserModel, IConfigHelper configHelper,
            IOrderEndPoint orderEndPoint)
        {
            _productEndPoint = productEndPoint;
            _events = events;
            _loggedInUserModel = loggedInUserModel;
            _configHelper = configHelper;
            _orderEndPoint = orderEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {

            base.OnViewLoaded(view);
            await LoadProducts();
 
        }

        private async Task LoadProducts()
        {

            var productList = await _productEndPoint.GetAll();
            Products = new BindableCollection<ProductViewModel>(productList.Select(x => CreateProductViewModel(x)));
            NotifyOfPropertyChange(() => Products);
        }

        private ProductViewModel CreateProductViewModel(ProductModel product)
        {

            var productViewModel = new ProductViewModel(product, _configHelper);
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



        public BindableCollection<ProductViewModel> Products { get; set; } = new BindableCollection<ProductViewModel>();

        private BindingList<ProductViewModel> _cart = new BindingList<ProductViewModel>();

        public BindingList<ProductViewModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
                NotifyOfPropertyChange(() => TotalSubTotal);
                NotifyOfPropertyChange(() => TotalTax);
                NotifyOfPropertyChange(() => TotalTotal);
                NotifyOfPropertyChange(() => CanCheckOut);
            }
        }

        public decimal TotalSubTotal
        {
            get
            {
                return CalculateTotalSubTotal();
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
        public decimal TotalTax
        {
            get
            {
                return CalculateTotalTax();
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

        public decimal TotalTotal
        {
            get
            {
                decimal total = TotalSubTotal + TotalTax;

                return total;
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

        public async Task CheckOut()
        {
            try
            {

                OrderModel orderModel = new OrderModel();
                orderModel.UserId = _loggedInUserModel.Id;
                orderModel.SubTotal = TotalSubTotal;
                orderModel.Tax = TotalTax;
                orderModel.Total = TotalTotal;


                int orderId = await _orderEndPoint.PostOrderInfo(orderModel);

                foreach (var item in Cart)
                {
                    OrderDetailModel orderDetailModel = new OrderDetailModel();
                    orderDetailModel.OrderId = orderId;
                    orderDetailModel.ProductId = item.Id;
                    orderDetailModel.Quantity = item.ItemQuantity;
                    orderDetailModel.PurchasePrice = item.SubTotal;
                    orderDetailModel.Tax = item.Tax;

                    await _orderEndPoint.PostOrderDetailInfo(orderDetailModel);

                    UpdatedQtyProductModel updatedQtyProduct = new UpdatedQtyProductModel();
                    updatedQtyProduct.Id = item.Id;
                    updatedQtyProduct.QuantityInStock = item.QuantityInStock - item.ItemQuantity;

                    await _productEndPoint.UpdateProductQuantity(updatedQtyProduct);

                }


                MessageBox.Show($@"Your order is in the way{Environment.NewLine}{_loggedInUserModel.FirstName}, Thank you for shopping with us!");
                await ResetCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task ResetCart()
        {
            await LoadProducts();
            Cart = new BindingList<ProductViewModel>();
            NotifyOfPropertyChange(() => Cart);
         

        }

        public string LoggedInUser
        {
            get
            {
                return _loggedInUserModel.FirstName;
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
