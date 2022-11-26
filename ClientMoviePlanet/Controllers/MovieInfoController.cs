using ClientMoviePlanet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ClientMoviePlanet.Controllers
{
    public class MovieInfoController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private HttpResponseMessage response;

        //Hosted web API REST Service base url
        string Baseurl = "http://movieplanetapi-766262829.us-east-1.elb.amazonaws.com/";

        public MovieInfoController()
        {
            _httpClient.BaseAddress = new Uri(Baseurl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> MoviesByCompany(int companyId, string companyName, int eidr)
        {
            List<MovieInfo> movieInfoList = new List<MovieInfo>();

            if (eidr == null || eidr == 0)
            {
                //Sending request to find web api REST service resource GetAllCompanies using HttpClient
                response = await _httpClient.GetAsync("api/companyinfoes/" + companyId + "/movies");
                //Checking the response is successful or not which is sent using HttpClient
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var movieResponse = response.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Companies list
                    movieInfoList = JsonConvert.DeserializeObject<List<MovieInfo>>(movieResponse);
                }
                //returning the company list to view
                ViewBag.CompanyName = companyName;
                ViewBag.CompanyId = companyId;
                return View(movieInfoList);
            }

            Trace.WriteLine("EIDR of selected movie: " + eidr.ToString());

            //Sending request to find web api REST service resource GetAllCompanies using HttpClient
            response = await _httpClient.GetAsync("api/companyinfoes/" + companyId + "/movies/" + eidr);

            //Checking the response is successful or not which is sent using HttpClient
            if (response.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var movieResponse = response.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Company list
                MovieInfo movieInfo = JsonConvert.DeserializeObject<MovieInfo>(movieResponse);
                movieInfoList.Add(movieInfo);
            }

            //returning the company list to view
            ViewBag.CompanyName = companyName;
            ViewBag.CompanyId = companyId;
            return View(movieInfoList);
        }

        // GET: MovieInfoController/Create
        public ActionResult Create(int companyId, string companyName)
        {
            ViewBag.CompanyName = companyName;
            ViewBag.CompanyId = companyId;
            return View();
        }

        // POST: MovieInfoController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MovieInfo movieInfo, string companyName)
        {
            Debug.WriteLine("Company Name: " + companyName);
            try
            {
                MovieInfo newMovieInfo = new MovieInfo()
                {
                    companyId = movieInfo.companyId,
                    movieEidr = movieInfo.movieEidr,
                    movieTitle = movieInfo.movieTitle,
                    director = movieInfo.director,
                    description = movieInfo.description,
                    genre = movieInfo.genre,
                    releaseDate = movieInfo.releaseDate,
                    worldwideProfit = Math.Round(movieInfo.worldwideProfit),
                    imdbRating = movieInfo.imdbRating
                };

                string json = JsonConvert.SerializeObject(newMovieInfo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync("/api/companyinfoes/" + movieInfo.companyId + "/movies", content);
                return RedirectToAction("MoviesByCompany", new { eidr = 0, companyName = companyName, companyId = movieInfo.companyId });
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: MovieInfo
        public async Task<ActionResult> Delete(int companyId, int movieEidr, string companyName)
        {
            Debug.WriteLine("Company ID: " + companyId);
            Debug.WriteLine("Company Name: " + companyName);
            try
            {
                response = await _httpClient.DeleteAsync($"/api/companyinfoes/{companyId}/movies?movieEidr={movieEidr}");
                return RedirectToAction("MoviesByCompany", new { eidr = 0, companyName = companyName, companyId = companyId });
            }
            catch
            {
                return BadRequest();
            }
        }

        /*      public IActionResult Index()
              {
                  return View();
              }*/
    }
}
