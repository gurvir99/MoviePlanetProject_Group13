using ClientMoviePlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ClientMoviePlanet.Controllers
{
    public class MovieInfoController : Controller
    {
        //Hosted web API REST Service base url
        string Baseurl = "http://movieplanetapi-766262829.us-east-1.elb.amazonaws.com/";

        public async Task<ActionResult> MoviesByCompany(int companyId, string companyName, int eidr)
        {
            List<MovieInfo> movieInfoList = new List<MovieInfo>();

            using (var client = new HttpClient())
            {
                if (eidr == null || eidr == 0)
                {
                    Trace.WriteLine("HERE: " + companyId);
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllCompanies using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("api/companyinfoes/" + companyId + "/movies");
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var movieResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Companies list
                        movieInfoList = JsonConvert.DeserializeObject<List<MovieInfo>>(movieResponse);
                    }
                    //returning the company list to view
                    ViewBag.CompanyName = companyName;
                    ViewBag.CompanyId = companyId;
                    return View(movieInfoList);
                }

                Trace.WriteLine("EIDR: " + eidr.ToString());
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllCompanies using HttpClient
                HttpResponseMessage Res2 = await client.GetAsync("api/companyinfoes/" + companyId + "/movies/" + eidr);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res2.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var movieResponse = Res2.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Company list
                    MovieInfo movieInfo = JsonConvert.DeserializeObject<MovieInfo>(movieResponse);
                    movieInfoList.Add(movieInfo);
                }
                //returning the company list to view
                ViewBag.CompanyName = companyName;
                ViewBag.CompanyId = companyId;
                return View(movieInfoList);

            }
        }

        /*      public IActionResult Index()
              {
                  return View();
              }*/
    }
}
