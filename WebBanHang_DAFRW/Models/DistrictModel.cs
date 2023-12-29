using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebBanHang_DAFRW.Models
{
    public partial class DistrictModel
    {
        public DistrictModel()
        {
            Wards = new HashSet<WardModel>();
        }
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string CodeName { get; set; }
        public string ProvinceCode { get; set; }
        public int? AdministrativeUnitId { get; set; }

        public virtual ProvinceModel ProvinceCodeNavigation { get; set; }
        public virtual ICollection<WardModel> Wards { get; set; }
    }
}
