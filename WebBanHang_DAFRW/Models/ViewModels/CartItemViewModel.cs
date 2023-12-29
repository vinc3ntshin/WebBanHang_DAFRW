using System;
using System.Collections.Generic;

#nullable disable
namespace WebBanHang_DAFRW.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
