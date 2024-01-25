using HW3_StoreWebApi_Storage.Dto;

namespace HW3_StoreWebApi_Storage.Abstraction;

public interface IStorageRepository
{
    public IEnumerable<StorageDto> GetAllStorages();
    public int AddStorage(StorageDto storageDto);
    public void SetProductAmountInStorage(StorageProductsDto storageProductsDto);
    public IEnumerable<StorageProductsDto> GetStorageProducts(int storageId);
}