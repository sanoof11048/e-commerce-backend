using AutoMapper;
using Plashoe.DTOs;
using Plashoe.DTOs.Products;
using Plashoe.Model;

namespace Plashoe.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, User>();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<ProductViewDTO, Product>(); 
            CreateMap<Product, ProductViewDTO>().ReverseMap();
            CreateMap<WishlistDTO, Wishlist>().ReverseMap();
            CreateMap<AddressDTO,Address>().ReverseMap();
            CreateMap<CartItems, CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product.Image));
            CreateMap<Cart, CartViewDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Cartitems));

        }
    }
}
