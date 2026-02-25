using AutoMapper;
using DTOLayer.DTOs.CategoryDTOs;
using DTOLayer.DTOs.GuideDTOs;
using DTOLayer.DTOs.ReservationDTOs;
using DTOLayer.DTOs.TourDTOs;
using EntityLayer;


namespace DTOLayer.Mapping
{
    public class GeneralMapping :Profile
    {
        public GeneralMapping() //mapleme işlemi için constructor
        {
            //Category
            CreateMap<Category,CreateCategoryDTO>().ReverseMap(); 
            CreateMap<Category,ResultCategoryDTO>().ReverseMap();
            CreateMap<Category,UpdateCategoryDTO>().ReverseMap();
            CreateMap<Category,GetCategoryByIdDTO>().ReverseMap();

            //Tour
            CreateMap<Tour, CreateTourDTO>().ReverseMap();
            CreateMap<Tour, ResultTourDTO>().ReverseMap();
            CreateMap<Tour, UpdateTourDTO>().ReverseMap();
            CreateMap<Tour, GetTourByIdDTO>().ReverseMap();
            CreateMap<GetTourByIdDTO, ResultTourDTO>().ReverseMap();
            CreateMap<GetTourByIdDTO, UpdateTourDTO>().ReverseMap();

            //Guide
            CreateMap<Guide, CreateGuideDTO>().ReverseMap();    
            CreateMap<Guide, ResultGuideDTO>().ReverseMap();
            CreateMap<Guide, UpdateGuideDTO>().ReverseMap();
            CreateMap<Guide, GetGuideByIdDTO>().ReverseMap();
            CreateMap<GetGuideByIdDTO, UpdateGuideDTO>().ReverseMap();
            CreateMap<GetGuideByIdDTO, ResultGuideDTO>().ReverseMap();

            // Reservation
            CreateMap<Reservation, CreateReservationDTO>().ReverseMap();
            CreateMap<Reservation, ResultReservationDTO>()
                .ForMember(dest => dest.TourTitle, opt => opt.MapFrom(src => src.TourTitle))
                // Eğer Tour nesnesi Reservation içinde yüklü geliyorsa:
                // .ForMember(dest => dest.GuideName, opt => opt.MapFrom(src => src.Tour.GuideName))
                .ReverseMap();
            CreateMap<Reservation, UpdateReservationStatusDTO>().ReverseMap();

        }
    }
}
