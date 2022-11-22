﻿namespace ClientMoviePlanet.Models
{
    public class MovieInfo
    {
        public int movieEidr { get; set; }
        public string movieTitle { get; set; }
        public string director { get; set; }
        public string description { get; set; }
        public string genre { get; set; }
        public DateTime releaseDate { get; set; }
        public string worldwideProfit { get; set; }
        public float imdbRating { get; set; }
    }
}