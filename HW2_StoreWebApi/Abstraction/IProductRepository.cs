using HW2_StoreWebApi.Dto;

namespace HW2_StoreWebApi.Abstraction;

public interface IProductRepository
{
    public IEnumerable<ProductDto> GetAllProducts();
    public int AddProduct(ProductDto productDto);
    public string GetProductsCsv();
}