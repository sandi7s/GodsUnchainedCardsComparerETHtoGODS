using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GodsUnchainedCardsComparer.Models
{
    public class CardsResponseModel
    {
        public List<Result> result { get; set; }
        public string cursor { get; set; }
        public int remaining { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Buy
    {
        public string type { get; set; }
        public Data data { get; set; }
    }

    public class Collection
    {
        public string name { get; set; }
        public string icon_url { get; set; }
    }

    public class Data
    {
        public string token_id { get; set; }
        public string id { get; set; }
        public string token_address { get; set; }
        public double? quantity { get; set; }
        public double? quantity_with_fees { get; set; }
        public Properties properties { get; set; }
        public int decimals { get; set; }

        //USER ADDED
        public double? price
        {
            get
            {
                if (quantity == null)
                {
                    return 0;
                }
                return quantity / Math.Pow(10, decimals);
            }
        }
    }

    public class Properties
    {
        public string name { get; set; }
        public string image_url { get; set; }
        public Collection collection { get; set; }
    }

    public class Result
    {
        public int order_id { get; set; }
        public string status { get; set; }
        public string user { get; set; }
        public Sell sell { get; set; }
        public Buy buy { get; set; }
        public object amount_sold { get; set; }
        public DateTime expiration_timestamp { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime updated_timestamp { get; set; }
    }

    public class Sell
    {
        public string type { get; set; }
        public Data data { get; set; }
    }


}