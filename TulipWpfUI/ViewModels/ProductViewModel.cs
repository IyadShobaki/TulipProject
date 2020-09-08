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
        private readonly ProductModel _product;

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

        public event EventHandler AddTCart;

        public bool CanAddToCart
        {          
            get
            {

                bool output = false;
               
                if (ItemQuantity > 0 && ItemQuantity <= QuantityInStock)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            AddTCart?.Invoke(this, EventArgs.Empty);
        }
    }
}
