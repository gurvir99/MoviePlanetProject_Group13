using System.Diagnostics;
using ClientMoviePlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;

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
                Debug.WriteLine(response);

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
            Debug.WriteLine(response);

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
            CompanyInfo newCompanyInfo = new CompanyInfo()
            {
                CompanyName = companyInfo.CompanyName,
                Headquarters = companyInfo.Headquarters,
                Description = companyInfo.Description,
                YearFounded = companyInfo.YearFounded
            };
            string json = JsonConvert.SerializeObject(newCompanyInfo);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await _httpClient.PostAsync("/api/companyInfo", content);
            Debug.WriteLine(response);

            return RedirectToAction(nameof(Index));

        }
        // GET: CompanyInfoController/Edit
        public ActionResult Edit(int companyId, string companyName, string headquarters, string description, int yearFounded)
        {
            ViewBag.CompanyName = companyName;
            ViewBag.CompanyId = companyId;
            ViewBag.Headquarters = headquarters;
            ViewBag.Description = description;
            ViewBag.yearFounded = yearFounded;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int companyId, CompanyInfo companyInfo)
        {
            CompanyInfo putCompany = new CompanyInfo()
            {
                CompanyName = companyInfo.CompanyName,
                Headquarters = companyInfo.Headquarters,
                Description = companyInfo.Description,
                YearFounded = companyInfo.YearFounded
            };
            string json = JsonConvert.SerializeObject(putCompany);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await _httpClient.PutAsync("api/CompanyInfo/" + companyId, content);
            Debug.WriteLine(response);

            return RedirectToAction(nameof(Index));
        }
        // Patch GET
        [HttpGet]
        public ActionResult Patch(int companyId, string companyName, string description)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = companyName;
            ViewBag.description = description;
            return View();
        }
        // Patch
        public async Task<ActionResult> Patch(int companyId, string description)
        {
            var patchDoc = new JsonPatchDocument<CompanyInfo>();
            //operation replace
            patchDoc.Replace(e => e.Description, description);

            var json = JsonConvert.SerializeObject(patchDoc);
            Debug.WriteLine(patchDoc);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            response = await _httpClient.PatchAsync("api/CompanyInfo/" + companyId, requestContent);
            Debug.WriteLine(response);

            return RedirectToAction(nameof(Index));
        }
        // DELETE: CompanyInfo
        public async Task<ActionResult> Delete(int companyId)
        {
            response = await _httpClient.DeleteAsync($"/api/CompanyInfo?companyId={companyId}");
            Debug.WriteLine(response);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}