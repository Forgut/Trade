using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trade.Core.Object
{
    public class TradeNode
    {
        public IEnumerable<TradeNode> Incoming { get; set; }
        public IEnumerable<TradeNode> Outgoing { get; set; }
        public IEnumerable<Province> Provinces
        {
            get
            {
                return GameInfo.Provinces.Where(p => p.TradeNode == this);
            }
        }
        public decimal Value { get; set; }
        public List<Merchant> Merchants { get; set; }
        public decimal Retained { get; set; }
        public decimal Tranfered { get; set; }
    }
}
