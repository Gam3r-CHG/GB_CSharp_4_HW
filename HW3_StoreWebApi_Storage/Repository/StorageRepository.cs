using AutoMapper;
using HW3_StoreWebApi_Storage.Abstraction;
using HW3_StoreWebApi_Storage.Db;
using HW3_StoreWebApi_Storage.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace HW3_StoreWebApi_Storage.Repository;

public class StorageRepository : IStorageRepository
{
    private IMemoryCache _cache;
    private IMapper _mapper;
    private AppDbContext _context;

    public StorageRepository(IMemoryCache cache, IMapper mapper, AppDbContext context)
    {
        _cache = cache;
        _mapper = mapper;
        _context = context;
    }

    public IEnumerable<StorageDto> GetAllStorages()
    {
        if (_cache.TryGetValue("storages", out IEnumerable<StorageDto> storageDtos))
        {
            return storageDtos;
        }

        using (_context)
        {
            storageDtos = _context.Stores
                .Select(_mapper.Map<StorageDto>)
                .ToList();
            _cache.Set("storages", storageDtos, TimeSpan.FromMinutes(30));
            return storageDtos;
        }
    }

    public int AddStorage(StorageDto storageDto)
    {
        using (_context)
        {
            Store? store = _context.Stores
                .FirstOrDefault(x => x.Name.ToLower().Equals(storageDto.Name.ToLower()));

            if (store != null)
            {
                throw new Exception("Storage with the same name already exists");
            }

            store = _mapper.Map<Store>(storageDto);
            _context.Stores.Add(store);
            _context.SaveChanges();

            _cache.Remove("storages");

            return store.Id;
        }
    }

    public void SetProductAmountInStorage(StorageProductsDto storageProductsDto)
    {
        Storesproduct? productInStorage = _context.Storesproducts
            .FirstOrDefault(x =>
                x.ProductId == storageProductsDto.ProductId && x.StoreId == storageProductsDto.StoreId);

        if (productInStorage != null)
        {
            productInStorage.Count = storageProductsDto.Count;
        }
        else
        {
            _context.Storesproducts.Add(_mapper.Map<Storesproduct>(storageProductsDto));
        }

        _context.SaveChanges();
    }

    public IEnumerable<StorageProductsDto> GetStorageProducts(int storageId)
    {
        var storageBooks = _context.Storesproducts.Where(x => x.StoreId == storageId)
            .Select(x => _mapper.Map<StorageProductsDto>(x)).ToList();

        return storageBooks;
    }
}