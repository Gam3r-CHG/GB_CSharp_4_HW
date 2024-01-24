using System.Text;
using HW2_StoreWebApi.Abstraction;
using HW2_StoreWebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HW2_StoreWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }


    [HttpGet(template: "get_all_products")]
    public ActionResult<List<ProductDto>> GetAllProducts()
    {
        try
        {
            var products = _repository.GetAllProducts();
            return Ok(products);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }

    [HttpPost("add_product")]
    public ActionResult<int> AddProduct(ProductDto productDto)
    {
        try
        {
            var productId = _repository.AddProduct(productDto);
            return Ok(productId);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }

    [HttpGet(template: "get_products_csv")]
    public ActionResult<string> GetProductsCsv()
    {
        var productsCsv = _repository.GetProductsCsv();
        return Ok(productsCsv);
    }

    [HttpGet(template: "get_products_csv_url")]
    public ActionResult<string> GetProductsCsvUrl()
    {
        var productsCsv = _repository.GetProductsCsv();

        var fileName = "products" + DateTime.Now.ToBinary() + ".csv";

        System.IO.File.WriteAllText(
            Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName),
            productsCsv);
        return "http://" + Request.Host + "/static/" + fileName;
    }

    [HttpGet(template: "get_products_csv_file")]
    public FileContentResult GetProductsCsvFile()
    {
        var productsCsv = _repository.GetProductsCsv();
        var fileName = "products" + DateTime.Now.ToBinary() + ".csv";
        return File(new UTF8Encoding().GetBytes(productsCsv), "text/csv", fileName);
    }
}