using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Category;
using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.ModelVM.Review;
using GameVault.BLL.ModelVM.User;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Mappers
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Game, ModelVM.GameVM>().ReverseMap();
            CreateMap<GameDTO, GameDetails>().ReverseMap();
            CreateMap<Category, UpdateCategory>().ReverseMap();
            CreateMap<Category, CreateCategory>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, CreateReview>().ReverseMap();
            CreateMap<Review, UpdateReview>().ReverseMap();
            CreateMap<Game, EditGame>().ReverseMap();
            CreateMap<CompanyVM, Company>()
                .ConstructUsing(src => new Company(src.CompanyName, src.CompanyInfo, src.CreatedBy))
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore()) 
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()) 
                .ForMember(dest => dest.Games, opt => opt.Ignore()); 
            CreateMap<Company, CompanyVM>().ReverseMap();


            CreateMap<Company, CompanyVM>().ReverseMap();

            CreateMap<User, UpdateUserProfile>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash)).ReverseMap();
            CreateMap<User, UserPrivateProfile>().ReverseMap();
            CreateMap<User, UserPublicProfile>().ReverseMap();


            CreateMap<CreateCategory, Category>()
    .ForMember(dest => dest.Category_Name, opt => opt.MapFrom(src => src.Category_Name));

        }
    }
}
