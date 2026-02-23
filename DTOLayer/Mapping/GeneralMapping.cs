using AutoMapper;
using DTOLayer.DTOs.CategoryDTOs;
using DTOLayer.DTOs.GuideDTOs;
using DTOLayer.DTOs.TourDTOs;
using EntityLayer;


namespace DTOLayer.Mapping
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

            CreateMap<GetTourByIdDTO, ResultTourDTO>().ReverseMap();


            CreateMap<Guide, CreateGuideDTO>().ReverseMap();    
            CreateMap<Guide, ResultGuideDTO>().ReverseMap();
            CreateMap<Guide, UpdateGuideDTO>().ReverseMap();
            CreateMap<Guide, GetGuideByIdDTO>().ReverseMap();
        }
    }
}
