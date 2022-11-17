using System;
using System.Collections.Generic;

namespace MoviePlanetLibrary.Models;

public partial class MovieInfo
{
    public int MovieEidr { get; set; }

    public string MovieTitle { get; set; } = null!;

    public string Director { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public decimal WorldwideProfit { get; set; }

    public double ImdbRating { get; set; }

    public int CompanyId { get; set; }

    public virtual CompanyInfo Company { get; set; } = null!;
}
