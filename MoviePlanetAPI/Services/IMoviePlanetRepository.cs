using System.Runtime;
using MoviePlanetLibrary.Models;

namespace MoviePlanetAPI.Services
{
    public interface IMoviePlanetRepository
    {
        Task<bool> CopmanyExists(int companyId);
        Task<IEnumerable<CompanyInfo>> GetCompanyInfoes();
        Task<CompanyInfo> GetCompanyById(int companyId, bool includeMovies);

        Task<IEnumerable<MovieInfo>> GetMoviesForCompany(int companyId);
        Task<MovieInfo> GetMovieForCompany(int companyId, int movieEidr);

        Task AddMovieForCity(int companyId, MovieInfo movie);
        void DeleteMovie(MovieInfo movie);

        Task<bool> Save();
    }
}
