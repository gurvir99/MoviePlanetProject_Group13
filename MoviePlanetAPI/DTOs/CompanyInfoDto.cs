using MoviePlanetLibrary.Models;

namespace MoviePlanetAPI.DTOs
{
    public class CompanyInfoDto
    {
        public int CompanyId { get; set; }

        public string? CompanyName { get; set; }

        public string? Headquarters { get; set; }

        public string? Description { get; set; }

        public int YearFounded { get; set; }

        public int NumberOfMovies
        {
            get
            {
                return Movies.Count;
            }
        }

        public virtual ICollection<MoviesDto> Movies { get; } = new List<MoviesDto>();
    }
}
