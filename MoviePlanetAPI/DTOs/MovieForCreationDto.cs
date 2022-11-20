using System.ComponentModel.DataAnnotations;

namespace MoviePlanetAPI.DTOs
{
    public class MovieForCreationDto
    {
        [Required(ErrorMessage = "You should provide the movie EIDR.")]
        public int MovieEidr { get; set; }
        [Required(ErrorMessage = "You should provide the movie title.")]
        [MaxLength(100)]
        public string? MovieTitle { get; set; }
        [Required(ErrorMessage = "You should provide the movie director.")]
        [MaxLength(150)]
        public string? Director { get; set; }
        [Required(ErrorMessage = "You should provide the movie description.")]
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "You should provide the movie genre.")]
        [MaxLength(150)]
        public string? Genre { get; set; }
        [Required(ErrorMessage = "You should provide the movie release date.")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "You should provide the movie worldwide profit.")]
        public decimal WorldwideProfit { get; set; }
        [Required(ErrorMessage = "You should provide the movie imdb rating.")]
        public double ImdbRating { get; set; }
    }
}
