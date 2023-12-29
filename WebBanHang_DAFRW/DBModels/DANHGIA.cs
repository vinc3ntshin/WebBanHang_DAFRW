using DAFW_IS220.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBanHang_DAFRW.Models;

namespace App.DBModels
{
    public class DANHGIA 
    {
        [Key]
        public int MADANHGIA {set;get;}

        public string MATK {set;get;}
        [ForeignKey("MATK")]
        public AppUser TAIKHOAN {set;get;}

        public int MASP {set;get;}
        [ForeignKey("MASP")]
        public SANPHAM SANPHAM {set;get;}

        [Column(TypeName = "int")]
        public int SOSAO {set;get;}

        [Column(TypeName = "text")]
        public string NOIDUNG {set;get;}

        public int MADH {set;get;}
        [ForeignKey("MADH")]
        public DONHANG DONHANG {set;get;}
    }
}