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

        public async Task<bool> CompanyExists(int companyId)
        {
            return await _context.CompanyInfos.AnyAsync<CompanyInfo>(c => c.CompanyId == companyId);
        }

        public async Task<IEnumerable<CompanyInfo>> GetCompanyInfos()
        {
            var result = _context.CompanyInfos.OrderBy(c => c.CompanyName);
            return await result.ToListAsync();
        }

        public async Task<CompanyInfo> GetCompanyById(int companyId, bool includeMovies)
        {
            IQueryable<CompanyInfo> result;

            if (includeMovies)
            {
                result = _context.CompanyInfos.Include(c => c.Movies)
                    .Where(c => c.CompanyId == companyId);
            }
            else result = _context.CompanyInfos.Where(c => c.CompanyId == companyId);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<Movies> GetMovieForCompany(int companyId, int movieEidr)
        {
            IQueryable<Movies> result = _context.MovieInfos.Where(p => p.CompanyId == companyId && p.MovieEidr == movieEidr);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Movies>> GetMoviesForCompany(int companyId)
        {
            IQueryable<Movies> result = _context.MovieInfos.Where(p => p.CompanyId == companyId);
            return await result.ToListAsync();
        }

        public async Task AddMovieForCompany(int companyId, Movies movie)
        {
            var city = await GetCompanyById(companyId, false);
            city.Movies.Add(movie);
        }

        public void DeleteMovie(Movies movie)
        {
            _context.MovieInfos.Remove(movie);
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
