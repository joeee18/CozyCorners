using AutoMapper;
using CozyCorners.Core.Models;
using CozyCorners.Core.Models.Identity;
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CozyCorners.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            //CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<UserFormViewModel, AppUser>().ReverseMap();
            CreateMap<RoleUserViewModel, AppUser>().ReverseMap();
            CreateMap<RoleUserViewModel, IdentityRole>().ReverseMap();
            CreateMap<AppUser, UserViewModel>().ReverseMap();
            CreateMap<AppUser, UserRoleEdit>().ReverseMap();
            CreateMap<Category, CategoryVM>().ForMember(dest => dest.photo, opt => opt.MapFrom(src => src.PhotoPath)) 
                                                                        .ReverseMap();
            CreateMap< CategoryVM,Category>().ForMember(dest => dest.PhotoPath, opt => opt.MapFrom(src => src.photo)) 
                                                                .ReverseMap();
            CreateMap<Product, ProductVM>().ForMember(dest => dest.PhotoPath, opt => opt.MapFrom(src => src.PhotoPath)).ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                                                                       .ReverseMap();
        //    CreateMap<ProductVM, Product>().ForMember(dest => dest.PhotoPath, opt => opt.MapFrom(src => src.PhotoPath)).ForMember(dest => dest.Category.Name, opt => opt.MapFrom(src => src.CategoryName))
        //                                                        .ReverseMap();
        }
    }
}
