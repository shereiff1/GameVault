using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.DAL.Entites;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Mappers
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Game, GameVM>().ReverseMap();

            CreateMap<InventoryItem, InventoryItemVM>()
                .ForMember(dest => dest.GameTitle, opt => opt.MapFrom(src => src.Game.Title));

            CreateMap<Inventory, InventoryVM>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<Company, CompanyVM>().ReverseMap();
            
            CreateMap<User, UpdateUserProfile>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash)).ReverseMap();
            CreateMap<User, UserPrivateProfile>().ReverseMap();
            CreateMap<User, UserPublicProfile>().ReverseMap();
        }
    }
}
