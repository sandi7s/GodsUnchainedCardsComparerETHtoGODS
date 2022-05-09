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

            homeModel.ApiResults = GetCardsMarketList();

            var cards = GetCardsList();

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
        private CardsResponseModel GetCardsMarketList()
        {
            var client = new RestClient("https://api.x.immutable.com/v1");

            //var request = new RestRequest("/orders?status=filled&page_size=500&sell_token_address=0xacb3c6a43d15b907e8433077b6d38ae40936fe2c", Method.GET);
            var request = new RestRequest("/orders?status=filled&page_size=500&sell_metadata={\"proto\":[\"1333\"],\"quality\":[\"Meteorite\"]}", Method.GET);

            var responseData = client.Execute(request);

            var res = JsonConvert.DeserializeObject<CardsResponseModel>(responseData.Content);

            //foreach (var item in res.result)
            //{
            //    item.buy.data.price = 
            //}

            return res;
        }
    }
}