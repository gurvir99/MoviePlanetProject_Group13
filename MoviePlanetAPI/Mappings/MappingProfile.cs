using AutoMapper;
using MoviePlanetAPI.DTOs;
using MoviePlanetLibrary.Models;

namespace MoviePlanetAPI.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyInfo, CompanyWithoutMoviesDto>(); //map from CompanyInfo to CompanyWithoutMoviesDto
            CreateMap<CompanyInfoForCreationDto, CompanyInfo>(); //map from CompanyInfoForCreationDto to CompanyInfo
            CreateMap<CompanyInfoForUpdateDto, CompanyInfo>(); //map from CompanyInfoForUpdateDto to CompanyInfo
            CreateMap<CompanyInfo, CompanyInfoDto>();
            CreateMap<Movies, MoviesDto>();
            //CreateMap<MovieForCreationDto, Movies>(); // to add a new movie
            //CreateMap<MovieForUpdateDto, Movies>(); // to update existing movie
        }
    }
}
