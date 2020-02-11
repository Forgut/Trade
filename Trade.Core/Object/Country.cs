using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trade.Core.Object
{
    public class Country
    {
        public string Name { get; set; }
        public IEnumerable<Province> Provinces
        {
            get
            {
                return GameInfo.Provinces.Where(province => province.Owner == this);
            }

        }

    }
}
