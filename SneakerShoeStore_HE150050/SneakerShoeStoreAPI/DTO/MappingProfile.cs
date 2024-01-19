using AutoMapper;
using BusinessObject.Models;

namespace SneakerShoeStoreAPI.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();
            CreateMap<Cart, CartDTO>();
            CreateMap<CartDTO, Cart>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderDetail>();
        }
    }
}
