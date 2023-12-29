using DAFW_IS220.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static NuGet.Packaging.PackagingConstants;

#nullable disable
namespace WebBanHang_DAFRW.Models
{
    public class WishListModel
    {
        [Key]
        public int WishListId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]

        
        public virtual ProductModel Product { get; set; }
    }
}
