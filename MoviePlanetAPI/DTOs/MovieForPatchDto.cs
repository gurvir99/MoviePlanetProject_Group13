using System.ComponentModel.DataAnnotations;

namespace MoviePlanetAPI.DTOs
{
    public class MovieForPatchDto
    {
        [Required(ErrorMessage = "You should provide the movie EIDR.")]
        public int MovieEidr { get; set; }
        [MaxLength(100)]
        public string? MovieTitle { get; set; }
        [MaxLength(150)]
        public string? Director { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [MaxLength(150)]
        public string? Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal WorldwideProfit { get; set; }
        public double ImdbRating { get; set; }
    }
}
