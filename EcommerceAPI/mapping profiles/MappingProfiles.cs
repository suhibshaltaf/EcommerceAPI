using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;

namespace Ecommerce.API.mapping_profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Products, ProductDTO>()
                .ForMember(c => c.Category_Name, opt => opt.MapFrom(c => c.Category != null ? c.Category.Name : null));


            CreateMap<Orders, OrderDTO>();
            CreateMap<OrderDetails, OrderDetailsDTO>()
                .ForMember(o => o.ProductName, opt => opt.MapFrom(o => o.Products.Name))
                .ForMember(o => o.ProductImage, opt => opt.MapFrom(o => o.Products.Image));

            CreateMap<LocalUser, LocalUserDTO>();
        }

    }

}