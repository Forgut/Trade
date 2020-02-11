using System;
using System.Collections.Generic;
using System.Text;
using Trade.Core.Object;

namespace Trade.Core
{
    /// <summary>
    /// This class contains every object that exists in the game.
    /// </summary>
    public static class GameInfo
    {
        #region Countries
        public static Country Poland => new Country()
        {
            Name = "Poland"
        };
        public static Country TeutonicOrder => new Country()
        {
            Name = "Teutonic Order"
        };
        public static Country Denmark => new Country()
        {
            Name = "Denmark"
        };
        public static Country Sweden => new Country()
        {
            Name = "Sweden"
        };
        #endregion

        #region TradeNodes
        public static TradeNode Lubeck => new TradeNode()
        {
            Incoming = new List<TradeNode>() { BalticSea, Krakow }
        };
        public static TradeNode Krakow => new TradeNode()
        {
            Incoming = new List<TradeNode>() { BalticSea, Novgorod },
            Outgoing = new List<TradeNode>() { Lubeck }
        };
        public static TradeNode BalticSea => new TradeNode()
        {
            Incoming = new List<TradeNode>() { Novgorod },
            Outgoing = new List<TradeNode>() { Krakow, Lubeck }
        };
        public static TradeNode Novgorod => new TradeNode()
        {
            Outgoing = new List<TradeNode>() { BalticSea, Krakow }
        };
        #endregion

        #region Provinces
        public static Province Poznan => new Province()
        {
            Owner = Poland,
            TradeNode = Krakow,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        public static Province Warsaw => new Province()
        {
            Owner = Poland,
            TradeNode = Krakow,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        public static Province Gdansk => new Province()
        {
            Owner = TeutonicOrder,
            TradeNode = BalticSea,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        public static Province Riga => new Province()
        {
            Owner = TeutonicOrder,
            TradeNode = BalticSea,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        public static Province Meckelburg => new Province()
        {
            Owner = Denmark,
            TradeNode = Lubeck,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        public static Province Sjaelland => new Province()
        {
            Owner = Denmark,
            TradeNode = Lubeck,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        public static Province Stockholm => new Province()
        {
            Owner = Sweden,
            TradeNode = BalticSea,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        public static Province Neva => new Province()
        {
            Owner = Sweden,
            TradeNode = Novgorod,
            TradePower = 1.0m,
            TradeValue = 1.0m
        };
        #endregion
    }
}
