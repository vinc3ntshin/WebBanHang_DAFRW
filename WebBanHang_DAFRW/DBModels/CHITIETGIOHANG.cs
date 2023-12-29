using DAFW_IS220.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBanHang_DAFRW.Models;

namespace App.DBModels
{
    public class CHITIETGIOHANG 
    {
        public string MATK {set;get;}
        [ForeignKey("MATK")]
        public AppUser TAIKHOAN {set;get;}

        public int MASP {set;get;}
        [ForeignKey("MASP")]
        public SANPHAM SANPHAM {set;get;}

        public int MACTSP {set;get;}
        [ForeignKey("MACTSP")]
        public CHITIETSANPHAM CHITIETSANPHAM {set;get;}

        [Column(TypeName = "int")]
        public int SOLUONGMUA {set;get;}

        [Column(TypeName = "decimal(10,2)")]
        public decimal TONGGIA {set;get;}
    }
}