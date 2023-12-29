﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebBanHang_DAFRW.Models
{
    public partial class ProvinceModel
    {
        public ProvinceModel()
        {
            Districts = new HashSet<DistrictModel>();
        }
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string CodeName { get; set; }
        public int? AdministrativeUnitId { get; set; }
        public int? AdministrativeRegionId { get; set; }

        public virtual ICollection<DistrictModel> Districts { get; set; }
    }
}
