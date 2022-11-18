using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviePlanetAPI.DTOs;
using MoviePlanetAPI.Services;
using MoviePlanetLibrary.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviePlanetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyInfoController : ControllerBase
    {
        private IMoviePlanetRepository _moviePlanetRepository;
        private readonly IMapper _mapper;

        public CompanyInfoController(IMoviePlanetRepository moviePlanetRepository, IMapper mapper)
        {
            _moviePlanetRepository = moviePlanetRepository;
            _mapper = mapper;
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("/api/companyInfos")]
        public async Task<ActionResult<CompanyInfo>> GetCompanyInfos()
        {
            var companyInfos = await _moviePlanetRepository.GetCompanyInfos();
            var results = _mapper.Map<IEnumerable<CompanyWithoutMoviesDto>>(companyInfos);
            return Ok(results);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyInfo>> GetCompanyById(int id, bool includeMovies = false)
        {
            var company = await _moviePlanetRepository.GetCompanyById(id, includeMovies);

            if (company == null)
            {
                return NotFound();
            }

            if (includeMovies)
            {
                var companyResult = _mapper.Map<CompanyInfoDto>(company);

                return Ok(companyResult);
            }
            var companyWithoutMoviesResult = _mapper.Map<CompanyWithoutMoviesDto>(company);

            return Ok(companyWithoutMoviesResult);
        }


        // POST api/<CompanyInfoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CompanyInfoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CompanyInfoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
