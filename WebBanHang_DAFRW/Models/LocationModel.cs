using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang_DAFRW.Models
{
    public partial class LocationModel
    {
        public ProvinceModel? Province { get; set; }
        public DistrictModel? District { get; set; }
        public WardModel? Ward { get; set; }
    }
}