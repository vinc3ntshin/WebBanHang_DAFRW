using DAFW_IS220.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static NuGet.Packaging.PackagingConstants;

#nullable disable
namespace WebBanHang_DAFRW.Models
{
    public class OrderModel
    {
        [Key]
        public string OrderCode { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public decimal? TotalMoney { get; set; }
        public OrderModel()
        {
            CreateDate = DateTime.Now;
            OrderDetails = new HashSet<OrderDetails>();
        }
    }
}
