using System;
using System.Collections.Generic;

namespace MoviePlanetLibrary.Models;

public partial class CompanyInfo
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Headquarters { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int YearFounded { get; set; }

    public virtual ICollection<MovieInfo> MovieInfos { get; } = new List<MovieInfo>();
}
