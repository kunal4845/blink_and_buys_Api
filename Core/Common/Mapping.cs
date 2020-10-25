using AutoMapper;
using Database.Models;

namespace Core.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Account, AccountModel>().ReverseMap();
            CreateMap<City, CityModel>().ReverseMap();
            CreateMap<State, StateModel>().ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<ProductImage, ProductImageModel>().ReverseMap();
            CreateMap<Service, ServiceModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<BookedService, BookedServiceModel>().ReverseMap();
        }
    }
}