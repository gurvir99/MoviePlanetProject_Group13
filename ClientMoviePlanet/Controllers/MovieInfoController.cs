using ClientMoviePlanet.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        // GET: MovieInfoController/Edit
        public ActionResult Edit(int companyId, int movieEidr, string movieTitle, string description, string genre,
            decimal worldwideProfit, float imdbRating, string director, DateTime releaseDate, string companyName)
        {
            ViewBag.movieTitle = movieTitle;
            ViewBag.companyId = companyId;
            ViewBag.description = description;
            ViewBag.worldwideProfit = worldwideProfit;
            ViewBag.genre = genre;
            ViewBag.movieEidr = movieEidr;
            ViewBag.imdbRating = imdbRating;
            ViewBag.director = director;
            ViewBag.releaseDate = releaseDate;
            ViewBag.companyName = companyName;
   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string companyName, MovieInfo movieInfo)
        {
            Debug.WriteLine("Company ID: " + movieInfo.companyId);
            Debug.WriteLine("Company Name: " + companyName);

            MovieInfo putCompany = new MovieInfo()
            {
                movieTitle = movieInfo.movieTitle,
                companyId = movieInfo.companyId,
                description = movieInfo.description,
                worldwideProfit = movieInfo.worldwideProfit,
                genre = movieInfo.genre,
                movieEidr = movieInfo.movieEidr,
                imdbRating = movieInfo.imdbRating,
                director = movieInfo.director,
                releaseDate = movieInfo.releaseDate,
            };
            string json = JsonConvert.SerializeObject(putCompany);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await _httpClient.PutAsync("api/companyinfoes/" + movieInfo.companyId + "/movies/" + movieInfo.movieEidr, content);
            Debug.WriteLine(response);

            return RedirectToAction("MoviesByCompany", new { eidr = 0, companyName = companyName, companyId = movieInfo.companyId });
        }

        // Patch GET
        [HttpGet]
        public ActionResult Patch(int companyId, string movieEidr, string description, string movieTitle, string companyName)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.MovieEidr = movieEidr;
            ViewBag.MovieTitle = movieTitle;
            ViewBag.Description = description;
            ViewBag.CompanyName = companyName;
            return View();
        }

        // Patch
        public async Task<ActionResult> Patch(int companyId, string movieEidr, string description, string companyName)
        {
            var patchDoc = new JsonPatchDocument<MovieInfo>();
            //operation replace
            patchDoc.Replace(e => e.description, description);

            var json = JsonConvert.SerializeObject(patchDoc);
            Debug.WriteLine(patchDoc);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            response = await _httpClient.PatchAsync("api/companyinfoes/" + companyId + "/movies/" + movieEidr, requestContent);
            Debug.WriteLine(response);

            return RedirectToAction("MoviesByCompany", new { eidr = 0, companyName = companyName, companyId = companyId });
        }
    }
}
