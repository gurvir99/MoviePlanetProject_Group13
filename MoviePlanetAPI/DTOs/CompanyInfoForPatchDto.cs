using MoviePlanetLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviePlanetAPI.DTOs
{
    public class CompanyInfoForPatchDto
    {
        //[Required(ErrorMessage = "You should provide a Company ID.")]
        public int CompanyId { get; set; }

        //[Required(ErrorMessage = "You should provide a Company Name.")]
        [MaxLength(50)]
        public string? CompanyName { get; set; }

        //[Required(ErrorMessage = "You should provide the Headquarters.")]
        [MaxLength(150)]
        public string? Headquarters { get; set; }

        //[Required(ErrorMessage = "You should provide a Description.")]
        [MaxLength(500)]
        public string? Description { get; set; }

        //[Required(ErrorMessage = "You should provide the Year Founded.")]
        public int YearFounded { get; set; }
    }
}
