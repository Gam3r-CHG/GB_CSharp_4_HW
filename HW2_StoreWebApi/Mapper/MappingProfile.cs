using AutoMapper;
using HW2_StoreWebApi.Db;
using HW2_StoreWebApi.Dto;

namespace HW2_StoreWebApi.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GroupDto, Group>().ReverseMap();
        CreateMap<ProductDto, Product>().ReverseMap();
    }
}