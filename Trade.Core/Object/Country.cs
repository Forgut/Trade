using System;
using System.Collections.Generic;
using System.Text;

namespace Trade.Core.Object
{
    public class Country
    {
        public string Name { get; set; }
        public List<Province> Provinces { get; set; }
    }
}
