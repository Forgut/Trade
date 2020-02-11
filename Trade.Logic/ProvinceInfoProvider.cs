using System;
using System.Collections.Generic;
using System.Text;
using Trade.Object;

namespace Trade.Logic
{
    public class ProvinceInfoProvider
    {
        public List<Province> Provinces { get; set; }

        public ProvinceInfoProvider(List<Province> provinces)
        {
            Provinces = provinces;
        }
    }
}
