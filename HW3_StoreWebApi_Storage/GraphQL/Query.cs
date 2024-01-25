using HW3_StoreWebApi_Storage.Abstraction;
using HW3_StoreWebApi_Storage.Dto;

namespace HW3_StoreWebApi_Storage.GraphQL;

public class Query
{
    public IEnumerable<StorageDto> GetStorages([Service] IStorageRepository repository) => repository.GetAllStorages();
    public IEnumerable<StorageProductsDto> GetStorageProducts([Service] IStorageRepository repository, int storageId) => repository.GetStorageProducts(storageId);
}