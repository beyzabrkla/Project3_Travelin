using AutoMapper;
using DTOLayer.DTOs.CommentDTOs;
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
                .ReverseMap();
            CreateMap<Reservation, UpdateReservationStatusDTO>().ReverseMap();

            //Comment
            CreateMap<Comment, CreateCommentDTO>().ReverseMap();
            CreateMap<Comment, ResultCommentDTO>()
                .ForMember(dest => dest.TourName, opt => opt.Ignore())
                .ReverseMap(); CreateMap<Comment, GetCommentByIdDTO>().ReverseMap();
            CreateMap<Comment, UpdateCommentDTO>().ReverseMap();
            CreateMap<Comment, ResultCommentListByTourIdDTO>().ReverseMap();
            CreateMap<ResultCommentDTO, ResultCommentListByTourIdDTO>().ReverseMap();
        }
    }
}
