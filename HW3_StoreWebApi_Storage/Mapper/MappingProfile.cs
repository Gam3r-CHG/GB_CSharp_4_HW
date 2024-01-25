using AutoMapper;
using HW3_StoreWebApi_Storage.Db;
using HW3_StoreWebApi_Storage.Dto;

namespace HW3_StoreWebApi_Storage.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StorageDto, Store>().ReverseMap();
        CreateMap<StorageProductsDto, Storesproduct>().ReverseMap();
    }
}