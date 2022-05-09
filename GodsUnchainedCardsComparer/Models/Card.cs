using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GodsUnchainedCardsComparer.Models
{
    public class ProtoResponseModel
    {
        public int total { get; set; }
        public int page { get; set; }
        public int perPage { get; set; }
        public List<Card> records { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Attack
    {
        public int Int64 { get; set; }
        public bool Valid { get; set; }
    }

    public class Health
    {
        public int Int64 { get; set; }
        public bool Valid { get; set; }
    }

    public class Card
    {
        public int id { get; set; }
        public string name { get; set; }
        public string effect { get; set; }
        public string god { get; set; }
        public string rarity { get; set; }
        public Tribe tribe { get; set; }
        public int mana { get; set; }
        public Attack attack { get; set; }
        public Health health { get; set; }
        public string type { get; set; }
        public string set { get; set; }
        public bool collectable { get; set; }
        public string live { get; set; }
        public string art_id { get; set; }
        public string lib_id { get; set; }
    }

    public class Tribe
    {
        public string String { get; set; }
        public bool Valid { get; set; }
    }


}