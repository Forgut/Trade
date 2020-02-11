using System;
using System.Collections.Generic;
using System.Text;

namespace Trade.Core.Object
{
    public class Province
    {
        public decimal TradeValue { get; set; }
        public decimal TradePower { get; set; }
        public Country Owner { get; set; }
        public TradeNode TradeNode { get; set; }
    }
}
