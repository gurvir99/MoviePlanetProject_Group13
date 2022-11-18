using System.Runtime;
using MoviePlanetLibrary.Models;

namespace MoviePlanetAPI.Services
{
    public interface IMoviePlanetRepository
    {
        Task<bool> CompanyExists(int companyId);
        Task<IEnumerable<CompanyInfo>> GetCompanyInfos();
        Task<CompanyInfo> GetCompanyById(int companyId, bool includeMovies);

        Task<IEnumerable<Movies>> GetMoviesForCompany(int companyId);
        Task<Movies> GetMovieForCompany(int companyId, int movieEidr);

        Task AddMovieForCompany(int companyId, Movies movie);
        void DeleteMovie(Movies movie);

        Task<bool> Save();
    }
}
