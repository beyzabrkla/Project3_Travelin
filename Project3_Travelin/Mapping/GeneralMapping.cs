using AutoMapper;
using Project3_Travelin.DTOs.CategoryDTOs;
using Project3_Travelin.DTOs.TourDTOs;
using Project3_Travelin.Entities;

namespace Project3_Travelin.Mapping
{
    public class GeneralMapping :Profile
    {
        public GeneralMapping() //mapleme işlemi için constructor
        {
            CreateMap<Category,CreateCategoryDTO>().ReverseMap(); 
            CreateMap<Category,ResultCategoryDTO>().ReverseMap();
            CreateMap<Category,UpdateCategoryDTO>().ReverseMap();
            CreateMap<Category,GetCategoryByIdDTO>().ReverseMap();

            CreateMap<Tour, CreateTourDTO>().ReverseMap();
            CreateMap<Tour, ResultTourDTO>().ReverseMap();
            CreateMap<Tour, UpdateTourDTO>().ReverseMap();
            CreateMap<Tour, GetTourByIdDTO>().ReverseMap();
        }
    }
}
