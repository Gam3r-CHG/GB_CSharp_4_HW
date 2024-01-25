using HW3_StoreWebApi_Storage.Abstraction;
using HW3_StoreWebApi_Storage.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HW3_StoreWebApi_Storage.Controllers;

[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
    private IStorageRepository _repository;

    public StorageController(IStorageRepository repository)
    {
        _repository = repository;
    }

    [HttpGet(template: "get_all_storages")]
    public ActionResult<List<StorageProductsDto>> GetAllStorages()
    {
        try
        {
            var storages = _repository.GetAllStorages();
            return Ok(storages);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }

    [HttpPost("add_storage")]
    public ActionResult<int> AddStorage(StorageDto storageDto)
    {
        try
        {
            var storageId = _repository.AddStorage(storageDto);
            return Ok(storageId);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }

    [HttpPost("set_product_amount_in_storage")]
    public ActionResult<int> SetProductAmountInStorage(StorageProductsDto storageProductsDto)
    {
        try
        {
            _repository.SetProductAmountInStorage(storageProductsDto);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }

    [HttpGet("get_storage_products")]
    public ActionResult<IEnumerable<StorageProductsDto>> GetStorageProducts(int storageId)
    {
        try
        {
            var products = _repository.GetStorageProducts(storageId);
            return Ok(products);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }
}