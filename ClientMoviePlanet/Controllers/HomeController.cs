using ClientMoviePlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClientMoviePlanet.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        //Hosted web API REST Service base url
        private string uri = "http://movieplanetapi-766262829.us-east-1.elb.amazonaws.com/";
        private HttpResponseMessage response;

        public HomeController()
        {
            _httpClient.BaseAddress = new Uri(uri);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> Index(int companyId)
        {
            List<CompanyInfo> companyInfoList = new List<CompanyInfo>();

            // if company id is selected (show company with that id)
            if (companyId != null && companyId != 0)
            {
                //Sending request to find web api REST service resource GetAllCompanies using HttpClient
                response = await _httpClient.GetAsync("api/CompanyInfo/" + companyId);

                //Checking the response is successful or not which is sent using HttpClient
                if (response.IsSuccessStatusCode)
                {
                    CompanyInfo companyInfo = new CompanyInfo();

                    //Storing the response details received from web api
                    var companyResponse = response.Content.ReadAsStringAsync().Result;

                    // Deserializing the response received from web api and storing into the Company list
                    companyInfo = JsonConvert.DeserializeObject<CompanyInfo>(companyResponse);
                    companyInfoList.Add(companyInfo);
                }
                //returning the company list to view
                return View(companyInfoList);
            }

            // if no company id is selected or 0 (show all companies)
            //Sending request to find web api REST service resource GetAllCompanies using HttpClient
            response = await _httpClient.GetAsync("api/companyInfos");
            //Checking the response is successful or not which is sent using HttpClient
            if (response.IsSuccessStatusCode)
            {
                //Storing the response details received from web api
                var companyResponse = response.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                companyInfoList = JsonConvert.DeserializeObject<List<CompanyInfo>>(companyResponse);
            }
            //returning the employee list to view
            return View(companyInfoList);
        }
        public IActionResult Privacy()
        {
           return View();
        }
    }
}