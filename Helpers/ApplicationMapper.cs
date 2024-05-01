using AutoMapper;
using RoadReady.Models;
using RoadReady.DTO;

namespace RoadReady.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Usertype, UserTypeDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<State, StateDTO>().ReverseMap();
            CreateMap<City, CityDTO>().ReverseMap();
            CreateMap<Reservation, ReservationDTO>().ReverseMap();
            CreateMap<PaymentDetail, PaymentDetailDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<CarReview, CarReviewDTO>().ReverseMap();
            CreateMap<CarImage, CarImageDTO>().ReverseMap();
            CreateMap<CarDetail, CarDetailDTO>().ReverseMap();
            CreateMap<Car, CarDTO>()
                .ForMember(dest => dest.CarDetails, opt => opt.MapFrom(src => src.CarDetails))
                .ForMember(dest => dest.CarImages, opt => opt.MapFrom(src => src.CarImages))
                .ReverseMap();

          
        }
    }
}
