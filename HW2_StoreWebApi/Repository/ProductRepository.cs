using System.Text;
using AutoMapper;
using HW2_StoreWebApi.Abstraction;
using HW2_StoreWebApi.Db;
using HW2_StoreWebApi.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace HW2_StoreWebApi.Repository;

public class ProductRepository : IProductRepository
{
    private IMemoryCache _cache;
    private IMapper _mapper;
    private AppDbContext _context;

    public ProductRepository(IMemoryCache cache, IMapper mapper, AppDbContext context)
    {
        _cache = cache;
        _mapper = mapper;
        _context = context;
    }

    public IEnumerable<ProductDto> GetAllProducts()
    {
        if (_cache.TryGetValue("products", out IEnumerable<ProductDto> productDtos))
        {
            return productDtos;
        }

        using (_context)
        {
            productDtos = _context.Products
                .Select(_mapper.Map<ProductDto>)
                .ToList();
            _cache.Set("products", productDtos, TimeSpan.FromMinutes(30));
            return productDtos;
        }
    }

    public int AddProduct(ProductDto productDto)
    {
        using (_context)
        {
            Product? product = _context.Products
                .FirstOrDefault(x => x.Name.ToLower().Equals(productDto.Name.ToLower()));

            if (product != null)
            {
                throw new Exception("Product with the same name already exists");
            }

            product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);
            _context.SaveChanges();

            _cache.Remove("products");

            return product.Id;
        }
    }

    public string GetProductsCsv()
    {
        var products = GetAllProducts();
        var sb = new StringBuilder();
        sb.AppendLine("Id;Name;Description;Price;GroupId");
        foreach (var product in products)
        {
            sb.AppendLine($"{product.Id};{product.Name};{product.Description};{product.Price};{product.GroupId}");
        }

        return sb.ToString();
    }
}