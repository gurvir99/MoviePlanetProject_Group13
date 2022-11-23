using ClientMoviePlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ClientMoviePlanet.Controllers
{
    public class HomeController : Controller
    {
        //Hosted web API REST Service base url
        string Baseurl = "http://movieplanetapi-766262829.us-east-1.elb.amazonaws.com/";

        public async Task<ActionResult> Index(int companyId)
        {
            List<CompanyInfo> companyInfoList = new List<CompanyInfo>();

            // if company id is selected (show company with that id)
            if (companyId != null && companyId != 0)
            {
                Trace.WriteLine("Company ID: " + companyId);
  
                CompanyInfo companyInfo = new CompanyInfo();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("api/CompanyInfo/" + companyId);
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var companyResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        companyInfo = JsonConvert.DeserializeObject<CompanyInfo>(companyResponse);
                        companyInfoList.Add(companyInfo);
                    }
                    //returning the employee list to view
                    return View(companyInfoList);
                }
            }

            // if no company id is selected (show all companies)
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/companyInfos");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var companyResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    companyInfoList = JsonConvert.DeserializeObject<List<CompanyInfo>>(companyResponse);
                }
                //returning the employee list to view
                return View(companyInfoList);
            }

            //private readonly ILogger<HomeController> _logger;

            //public HomeController(ILogger<HomeController> logger)
            //{
            //    _logger = logger;
            //}

            //public IActionResult Index()
            //{
            //    return View();
            //}
        }
        public IActionResult Privacy()
        {
           return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}