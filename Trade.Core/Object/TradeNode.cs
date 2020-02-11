using System;
using System.Collections.Generic;
using System.Text;

namespace Trade.Core.Object
{
    public class TradeNode
    {
        public List<TradeNode> Incoming { get; set; }
        public List<TradeNode> Outgoing { get; set; }
        public List<Province> Provinces { get; set; }
        public decimal Value { get; set; }
        public List<Merchant> Merchants { get; set; }
        public decimal Retained { get; set; }
        public decimal Tranfered { get; set; }
    }
}
