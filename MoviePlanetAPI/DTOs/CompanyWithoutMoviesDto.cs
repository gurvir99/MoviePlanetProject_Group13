namespace MoviePlanetAPI.DTOs
{
    public class CompanyWithoutMoviesDto
    {
        public int CompanyId { get; set; }

        public string? CompanyName { get; set; }

        public string? Headquarters { get; set; }

        public string? Description { get; set; }

        public int YearFounded { get; set; }
    }
}
