using System.Diagnostics;
using ClientMoviePlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ClientMoviePlanet.Controllers
{
    public class CompanyInfoController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        //Hosted web API REST Service base url
        private string uri = "http://movieplanetapi-766262829.us-east-1.elb.amazonaws.com/";
        private HttpResponseMessage response;

        public CompanyInfoController()
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
                return View(Tuple.Create<CompanyInfo, IEnumerable<CompanyInfo>>(new CompanyInfo(), companyInfoList.ToList()));
            }

            // if no company id is selected or 0 (show all companies)
            //Sending request to find web api REST service resource GetAllCompanies using HttpClient
            response = await _httpClient.GetAsync("api/companyInfos");
            //Checking the response is successful or not which is sent using HttpClient
            if (response.IsSuccessStatusCode)
            {
                //Storing the response details received from web api
                var companyResponse = response.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Company list
                companyInfoList = JsonConvert.DeserializeObject<List<CompanyInfo>>(companyResponse);
            }
            //returning the company list to view
            return View(Tuple.Create<CompanyInfo, IEnumerable<CompanyInfo>>(new CompanyInfo(), companyInfoList.ToList()));
        }
        // GET: CompanyInfoController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: CompanyInfoController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CompanyInfo companyInfo)
        {
            try
            {
                CompanyInfo newCompanyInfo = new CompanyInfo()
                {
                    companyName = companyInfo.companyName,
                    headquarters = companyInfo.headquarters,
                    description = companyInfo.description,
                    yearFounded = companyInfo.yearFounded
                };
                string json = JsonConvert.SerializeObject(newCompanyInfo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync("/api/companyInfo", content);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }
        // DELETE: CompanyInfo
        public async Task<ActionResult> Delete(int companyId)
        {
            try
            {
                response = await _httpClient.DeleteAsync($"/api/CompanyInfo?companyId={companyId}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}