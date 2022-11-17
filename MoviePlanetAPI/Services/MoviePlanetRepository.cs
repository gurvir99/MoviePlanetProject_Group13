using Microsoft.EntityFrameworkCore;
using MoviePlanetLibrary.Models;
using System.Runtime;

namespace MoviePlanetAPI.Services
{
    public class MoviePlanetRepository : IMoviePlanetRepository
    {
        private MoviePlanetDbContext _context;

        public MoviePlanetRepository(MoviePlanetDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CopmanyExists(int companyId)
        {
            return await _context.CompanyInfos.AnyAsync<CompanyInfo>(c => c.CompanyId == companyId);
        }

        public async Task<IEnumerable<CompanyInfo>> GetCompanyInfoes()
        {
            var result = _context.CompanyInfos.OrderBy(c => c.CompanyName);
            return await result.ToListAsync();
        }

        public async Task<CompanyInfo> GetCompanyById(int companyId, bool includeMovies)
        {
            IQueryable<CompanyInfo> result;

            if (includeMovies)
            {
                result = _context.CompanyInfos.Include(c => c.MovieInfos)
                    .Where(c => c.CompanyId == companyId);
            }
            else result = _context.CompanyInfos.Where(c => c.CompanyId == companyId);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<MovieInfo> GetMovieForCompany(int companyId, int movieEidr)
        {
            IQueryable<MovieInfo> result = _context.MovieInfos.Where(p => p.CompanyId == companyId && p.MovieEidr == movieEidr);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovieInfo>> GetMoviesForCompany(int companyId)
        {
            IQueryable<MovieInfo> result = _context.MovieInfos.Where(p => p.CompanyId == companyId);
            return await result.ToListAsync();
        }

        public async Task AddMovieForCity(int companyId, MovieInfo movie)
        {
            var city = await GetCompanyById(companyId, false);
            city.MovieInfos.Add(movie);
        }

        public void DeleteMovie(MovieInfo movie)
        {
            _context.MovieInfos.Remove(movie);
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
