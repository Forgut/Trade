using System;
using System.Collections.Generic;
using System.Text;

namespace Trade.Object
{
    public class TradeTransfer
    {
        public decimal Value { get; set; }
        public TradeNode Sender { get; set; }
        public TradeNode Receiver { get; set; }
    }
}
