using DAFW_IS220.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang_DAFRW.Models
{
    public class OrderDetails
    {
        public string OrderCode { get; set; }
        [ForeignKey("OrderCode")]
        public OrderModel Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
