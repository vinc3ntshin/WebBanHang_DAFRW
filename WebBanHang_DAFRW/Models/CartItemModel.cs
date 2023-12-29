using DAFW_IS220.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static NuGet.Packaging.PackagingConstants;

namespace WebBanHang_DAFRW.Models
{
    public class CartItemModel
    { 
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Quantity * Price; }
        }
        public CartItemModel() // Parameterless constructor
        {
        }
        public CartItemModel(ProductModel product, string userid)
        {
            this.Product = product;
            ProductId = product.Id;
            Quantity = 1;
            Price = product.Price;
            UserId = userid;
        }
    }
}
