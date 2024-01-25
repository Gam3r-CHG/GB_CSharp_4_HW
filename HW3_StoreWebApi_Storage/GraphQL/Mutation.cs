using HW3_StoreWebApi_Storage.Abstraction;
using HW3_StoreWebApi_Storage.Dto;

namespace HW3_StoreWebApi_Storage.GraphQL;

public class Mutation
{
    public int AddStorage([Service] IStorageRepository repository, StorageDto storageDto)
    {
        var storageId = repository.AddStorage(storageDto);
        return storageId;
    }

    public bool AddProductToStorage([Service] IStorageRepository repository, StorageProductsDto storageProductsDto)
    {
        try
        {
            repository.SetProductAmountInStorage(storageProductsDto);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}