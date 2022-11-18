using MoviePlanetLibrary.Models;

namespace MoviePlanetAPI.DTOs
{
    public class MoviesDto
    {
        public int MovieEidr { get; set; }

        public string? MovieTitle { get; set; }

        public string? Director { get; set; }

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal WorldwideProfit { get; set; }

        public double ImdbRating { get; set; }

    }
}
