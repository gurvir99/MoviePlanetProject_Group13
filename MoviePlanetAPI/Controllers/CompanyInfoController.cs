using System.Diagnostics;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpPost("/api/companyInfo")]
        public async Task<ActionResult<CompanyInfoDto>> CreateCompany([FromBody] CompanyInfoForCreationDto companyInfoForCreation)
        {
            if (companyInfoForCreation == null) return BadRequest();

            if (companyInfoForCreation.Description == companyInfoForCreation.CompanyName)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (companyInfoForCreation.CompanyName != null && await _moviePlanetRepository.CompanyExists(companyInfoForCreation.CompanyName))
            {
                ModelState.AddModelError("Company", "The provided company already exists.");
            }

            var finalCompany = _mapper.Map<CompanyInfo>(companyInfoForCreation);

            await _moviePlanetRepository.AddCompany(finalCompany);

            if (!await _moviePlanetRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdCompanyToReturn = _mapper.Map<CompanyInfoDto>(finalCompany);

            return Ok(createdCompanyToReturn);

        }




        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<CompanyInfoController>/5
        [HttpPut("{companyId}")]
        public async Task<ActionResult> UpdateCompany(int companyId,
            [FromBody] CompanyInfoForUpdateDto companyInfoForUpdate)
        {
            if (companyInfoForUpdate == null) return BadRequest();

            if (companyInfoForUpdate.Description == companyInfoForUpdate.CompanyName)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            //if(!await _moviePlanetRepository.CompanyExists(companyInfoForUpdate.CompanyName)) return NotFound();

            CompanyInfo oldCompqanyCompanyInfoEntity = await _moviePlanetRepository.GetCompanyById(companyId, false);

            if (oldCompqanyCompanyInfoEntity == null) return NotFound();

            _mapper.Map(companyInfoForUpdate, oldCompqanyCompanyInfoEntity);

            if (!await _moviePlanetRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }


        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<CompanyInfoController>/5
        [HttpDelete]
        public async Task<ActionResult> DeleteCompanyInfo(int companyId,
            [FromBody] string companyName)
        {
            if (!await _moviePlanetRepository.CompanyExists(companyName)) return NotFound();

            CompanyInfo companyInfoEntity2Delete = await _moviePlanetRepository.GetCompanyById(companyId, false);

            if(companyInfoEntity2Delete == null) return NotFound();

            _moviePlanetRepository.DeleteCompanyInfo(companyInfoEntity2Delete);

            if (!await _moviePlanetRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        [HttpPatch]
        public async Task<ActionResult> PartiallyUpdateCompanyInfo(int companyId,
            JsonPatchDocument<CompanyInfoForUpdateDto> patchDocument)
        {
            //if (!await _moviePlanetRepository.CompanyExists(companyId))
            //{
            //    return NotFound();
            //}
            CompanyInfo companyEntity = await _moviePlanetRepository.GetCompanyById(companyId, false);
            if (companyEntity == null)
            {
                return NotFound();
            }

            Debug.WriteLine("works");
            Debug.WriteLine(companyEntity.CompanyId);
            Trace.WriteLine(companyEntity.CompanyName);

            var companyToPatch = _mapper.Map<CompanyInfoForUpdateDto>(companyEntity);

            patchDocument.ApplyTo(companyToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(companyToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(companyToPatch, companyEntity);
            await _moviePlanetRepository.Save();

            return NoContent();
        }
    }
}
