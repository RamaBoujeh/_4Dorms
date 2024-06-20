using AutoMapper;
using _4Dorms.Resources;
using _4Dorms.Models;

namespace _4Dorms.Mapping   
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dormitory, DormitoryDTO>().ReverseMap();
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<DormitoryOwner, DormitoryOwnerDTO>().ReverseMap();
            CreateMap<PaymentGate, PaymentGateDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<SignUp, SignUpDTO>().ReverseMap();
            CreateMap<LogIn, SignInDTO>().ReverseMap();
        }
    }
}
