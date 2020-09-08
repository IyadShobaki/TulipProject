using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipWpfUI.Library.Models;

namespace TulipWpfUI.ViewModels
{
    public class ProductViewModel : Screen
    {
        private ProductModel _product;

        public ProductViewModel(ProductModel product)
        {
            _product = product;
        }

        public int Id => _product.Id;
        public string ProductName => _product.ProductName;
        public string Description => _product.Description;
        public string ProductImage => _product.ProductImage;
        public decimal RetailPrice => _product.RetailPrice;
        public int QuantityInStock => _product.QuantityInStock;
        public bool Sex => _product.Sex;

        private int _itemQuantity = 1;

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

        public decimal SubTotal
        {
            get
            {
                return CalculateSubTotal();
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            subTotal += (RetailPrice * ItemQuantity);

            return subTotal;
        }

        public decimal Tax
        {
            get
            {
                return CalculateTax();
            }
        }

        private decimal CalculateTax()
        {
            //decimal taxRate = _configHelper.GetTaxRate() / 100;  TODO
            decimal taxRate = (decimal)8.75 / 100;

            decimal taxAmount = RetailPrice * ItemQuantity * taxRate;

            return taxAmount;
        }

        public decimal Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();

                return total;
            }
        }

        public event EventHandler AddTCart;

        public event EventHandler RemoveFCart;

        private bool _isAdded = true;

        public bool IsAdded
        {
            get { return _isAdded; }
            set 
            {
                _isAdded = value;
                NotifyOfPropertyChange(() => IsAdded);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public bool CanAddToCart
        {          
            get
            {

                bool output = false;
               
                if (ItemQuantity > 0 && ItemQuantity <= QuantityInStock && IsAdded)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            AddTCart?.Invoke(this, EventArgs.Empty);
            IsAdded = false;
        }

        public bool CanRemoveFromCart
        {
            get
            {

                bool output = false;

                if (Total > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public void RemoveFromCart()
        {
            RemoveFCart?.Invoke(this, EventArgs.Empty);
            IsAdded = true;
        }
    }
}
