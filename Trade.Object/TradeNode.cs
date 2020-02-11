using System;
using System.Collections.Generic;
using System.Text;

namespace Trade.Object
{
    public class TradeNode
    {
        public List<TradeTransfer> Incoming { get; set; }
        public List<TradeTransfer> Outgoing { get; set; }
        public List<Province> Provinces { get; set; }
        public decimal Value { get; set; }
        public List<Merchant> Merchants { get; set; }
    }
}
