using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GodsUnchainedCardsComparer.Models
{
    public class HomeModel
    {
        public List<Card> CardsLowerETH { get; set; }
        public List<Card> CardsLowerGods { get; set; }
        public List<Card> CardsLowerETHExpensive { get; set; }
        public List<Card> CardsLowerGodsExpensive { get; set; }
    }
}