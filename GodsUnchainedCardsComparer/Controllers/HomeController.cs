using GodsUnchainedCardsComparer.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GodsUnchainedCardsComparer.Controllers
{
    public class HomeController : Controller
    {
        const string apiAddress = "https://api.x.immutable.com/v1";
        public ActionResult Index()
        {
            HomeModel homeModel = new HomeModel();

            var yesterday = DateTime.Now.AddDays(-1);

            double ETHprice = CheckTokenPrice("ethereum").current_price;
            double GODSprice = CheckTokenPrice("gods-unchained").current_price;

            var cards = GetCardsList().records;

            for (int i = 0; i < cards.Count; i++)
            {
                var ordersForThisCard = GetCardsMarketList(cards[i].id).result;

                if (ordersForThisCard != null)
                {
                    if (ordersForThisCard.Any())
                    {
                        cards[i].imageUrl = ordersForThisCard[0].sell.data.properties.image_url;
                        //card.lastSold = ordersForThisCard[0].timestamp;

                        var ordersinETH = ordersForThisCard.Where(e => e.buy.type == "ETH").ToList();
                        var ordersinGODS = ordersForThisCard.Where(e => e.buy.data.token_address == "0xccc8cb5229b0ac8069c51fd58367fd1e622afd97").ToList();

                        var ordersinETHlastDay = ordersinETH.Where(e => e.timestamp > yesterday && e.buy.data.quantity != null).ToList();
                        var ordersinGODSlastDay = ordersinGODS.Where(e => e.timestamp > yesterday && e.buy.data.quantity != null).ToList();

                        if (ordersinETHlastDay.Any())
                        {
                            cards[i].numberOfETHsales = ordersinETHlastDay.Count;
                            cards[i].ETHprice = ordersinETHlastDay[0].buy.data.price;
                            cards[i].ETHpriceAverage24 = ordersinETHlastDay.Sum(e => e.buy.data.price) / ordersinETHlastDay.Count;
                            cards[i].ETHpriceUSD = cards[i].ETHprice * ETHprice;
                        }
                        if (ordersinGODSlastDay.Any())
                        {
                            cards[i].numberOfGODSsales = ordersinGODSlastDay.Count;
                            cards[i].GODSprice = ordersinGODSlastDay[0].buy.data.price;
                            cards[i].GODSpriceAverage24 = ordersinGODSlastDay.Sum(e => e.buy.data.price) / ordersinGODSlastDay.Count;
                            cards[i].GODSpriceUSD = cards[i].GODSprice * GODSprice;
                        }

                        if (cards[i].ETHpriceUSD != null && cards[i].GODSpriceUSD != null && cards[i].ETHpriceUSD!=0)
                        {
                            cards[i].diffrenceInPrice = cards[i].GODSpriceUSD / cards[i].ETHpriceUSD * 100;
                        }
                    }
                }
            }

            //foreach (var card in cardsResult)
            //{
                
                
            //}

            homeModel.CardsLowerETH = cards.Where(e => e.diffrenceInPrice != null).OrderByDescending(e => e.diffrenceInPrice).Take(90).ToList();
            homeModel.CardsLowerGods = cards.Where(e => e.diffrenceInPrice != null).OrderBy(e => e.diffrenceInPrice).Take(90).ToList();
            homeModel.CardsLowerETHExpensive = cards.Where(e => e.diffrenceInPrice != null && e.ETHpriceUSD > 0.5).OrderByDescending(e => e.diffrenceInPrice).Take(90).ToList();
            homeModel.CardsLowerGodsExpensive = cards.Where(e => e.diffrenceInPrice != null && e.GODSpriceUSD > 0.5).OrderBy(e => e.diffrenceInPrice).Take(90).ToList();

            return View(homeModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private ProtoResponseModel GetCardsList()
        {
            var client = new RestClient("https://api.godsunchained.com/v0");

            var request = new RestRequest("/proto?perPage=10000", Method.GET);

            var responseData = client.Execute(request);

            var res = JsonConvert.DeserializeObject<ProtoResponseModel>(responseData.Content);

            //foreach (var item in res.result)
            //{
            //    item.buy.data.price = 
            //}

            return res;
        }
        private CardsResponseModel GetCardsMarketList(int proto)
        {
            var client = new RestClient("https://api.x.immutable.com/v1");

            //var request = new RestRequest("/orders?status=filled&page_size=500&sell_token_address=0xacb3c6a43d15b907e8433077b6d38ae40936fe2c", Method.GET);
            var request = new RestRequest("/orders?status=filled&page_size=500&sell_metadata={\"proto\":[\"" + proto+"\"],\"quality\":[\"Meteorite\"]}", Method.GET);

            var responseData = client.Execute(request);

            var res = JsonConvert.DeserializeObject<CardsResponseModel>(responseData.Content);

            //foreach (var item in res.result)
            //{
            //    item.buy.data.price = 
            //}

            return res;
        }

        private CoinGeckoPriceCheckResult CheckTokenPrice(string coinId)
        {
            var client = new RestClient("https://api.coingecko.com/api/v3");

            //var request = new RestRequest("/orders?status=filled&page_size=500&sell_token_address=0xacb3c6a43d15b907e8433077b6d38ae40936fe2c", Method.GET);
            var request = new RestRequest($"/coins/markets?vs_currency=usd&ids={coinId}", Method.GET);

            var responseData = client.Execute(request);

            var res = JsonConvert.DeserializeObject<List<CoinGeckoPriceCheckResult>>(responseData.Content);

            //foreach (var item in res.result)
            //{
            //    item.buy.data.price = 
            //}

            return res[0];
        }
    }
}