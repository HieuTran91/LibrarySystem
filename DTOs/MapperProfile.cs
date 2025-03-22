using AutoMapper;
using LibraryProject.Models;
using LibraryProject.DTOs;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Book
        CreateMap<Book, BookDTO>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.GenreName))
            .ReverseMap();

        // User -> UserDTO
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName)) // Lấy RoleName từ bảng Role
            .ReverseMap();

        // User -> RegisterDTO
        CreateMap<User, RegisterDTO>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId)) // Đăng ký bằng RoleId
            .ReverseMap();

        // Borrowing -> BorrowingDTO
        CreateMap<Borrowing, BorrowingDTO>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.ReaderName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.Book.BorrowingPrice))
            .ReverseMap();

        // Notification -> NotificationDTO
        CreateMap<Notification, NotificationDTO>()
            .ForMember(dest => dest.RecipientUserName, opt => opt.MapFrom(src => src.RecipientUser.Username)) 
            .ReverseMap();

        // Payment -> PaymentDTO
        CreateMap<Payment, PaymentDTO>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.MethodName))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Borrowing.Book.Title))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Borrowing.User.Username))
            .ReverseMap();

        // Review -> ReviewDTO
        CreateMap<Review, ReviewDTO>()
            //.ForMember(dest => dest., opt => opt.MapFrom(src => src.PaymentMethod.MethodName))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Borrowing.Book.Title))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Borrowing.User.Username))
            .ReverseMap();
    }
}
