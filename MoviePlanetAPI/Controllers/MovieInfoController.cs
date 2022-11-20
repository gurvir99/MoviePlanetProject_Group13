using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviePlanetAPI.DTOs;
using MoviePlanetAPI.Services;
using MoviePlanetLibrary.Models;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviePlanetAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/companyinfoes/{companyId}/movies")]
    [ApiController]
    public class MovieInfoController : ControllerBase
    {
        private readonly IMoviePlanetRepository _moviePlanetRepository;
        private readonly IMapper _mapper;

        public MovieInfoController(IMoviePlanetRepository moviePlanetRepository, IMapper mapper)
        {
            _moviePlanetRepository = moviePlanetRepository ??
                throw new ArgumentNullException(nameof(moviePlanetRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoviesDto>>> GetMovies(int companyId)
        {
            if (!await _moviePlanetRepository.CompanyExistsById(companyId))
            {
                return NotFound();
            }

            var moviesForCity = await _moviePlanetRepository.GetMoviesForCompany(companyId);
            return Ok(_mapper.Map<IEnumerable<MoviesDto>>(moviesForCity));
        }

        [HttpGet("{movieEidr}")]
        public async Task<ActionResult<MoviesDto>> GetMovie(int companyId, int movieEidr)
        {
            if (!await _moviePlanetRepository.CompanyExistsById(companyId))
            {
                return NotFound();
            }

            var movie = await _moviePlanetRepository.GetMovieForCompany(companyId, movieEidr);

            if (movie == null)
            {
                return NotFound();
            }

            Console.WriteLine("Found Movie: ", movie.MovieTitle.ToString());

            return Ok(_mapper.Map<MoviesDto>(movie));
        }

        [HttpPost]
        public async Task<ActionResult<MoviesDto>> CreateMovie(int companyId, [FromBody] MovieForCreationDto movie)
        {
            if (movie == null) return BadRequest();

            if (movie.Description == movie.MovieTitle)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the title.");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _moviePlanetRepository.CompanyExistsById(companyId)) return NotFound();

            var finalMovie = _mapper.Map<Movies>(movie);

            await _moviePlanetRepository.AddMovieForCompany(companyId, finalMovie);

            if (!await _moviePlanetRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdMovieToReturn = _mapper.Map<MoviesDto>(finalMovie);

            //return CreatedAtAction("GetMovie", new { companyId = companyId, id = createdMovieToReturn.MovieEidr}, createdMovieToReturn);
            return Ok(createdMovieToReturn);
        }

        [HttpPut("{movieEidr}")]
        public async Task<ActionResult> UpdateMovie (int companyId, int movieEidr,
            [FromBody] MovieForUpdateDto movie)
        {
            if (movie == null) return BadRequest();

            if (movie.Description == movie.MovieTitle)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the title.");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _moviePlanetRepository.CompanyExistsById(companyId)) return NotFound();

            Movies oldMovieEntity = await _moviePlanetRepository.GetMovieForCompany(companyId, movieEidr);

            if (oldMovieEntity == null) return NotFound();

            _mapper.Map(movie, oldMovieEntity);

            if (!await _moviePlanetRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMovie(int companyId, int movieEidr)
        {
            if (!await _moviePlanetRepository.CompanyExistsById(companyId)) return NotFound();

            Movies movieEntity2Delete = await _moviePlanetRepository.GetMovieForCompany(companyId, movieEidr);

            _moviePlanetRepository.DeleteMovie(movieEntity2Delete);

            if (!await _moviePlanetRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPatch]
        public async Task<ActionResult> PartiallyUpdateMovieInfo(int companyId, int movieEidr,
            JsonPatchDocument<MovieForPatchDto> patchDocument)
        {
            if (!await _moviePlanetRepository.CompanyExistsById(companyId)) return NotFound();
            var movieEntity = await _moviePlanetRepository.GetMovieForCompany(companyId, movieEidr);
            if (movieEntity == null)
            {
                return NotFound();
            }

            var movieToPatch = _mapper.Map<MovieForPatchDto>(movieEntity);

            patchDocument.ApplyTo(movieToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(movieToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(movieToPatch, movieEntity);
            await _moviePlanetRepository.Save();

            return NoContent();
        }
    }
}
