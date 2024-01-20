using HW1_StoreWebApi.DTO;
using HW1_StoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HW1_StoreWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet(template: "GetAllProducts")]
    public ActionResult<List<ProductDTO>> GetProducts()
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                var products = context.Products.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    GroupId = x.GroupId,
                    GroupName = x.Group.Name
                }).ToList();
                return Ok(products);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet(template: "GetProductById")]
    public ActionResult<ProductDTO> GetProductById(int productId)
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Product? product = context.Products
                    .Include(product => product.Group)
                    .FirstOrDefault(x => x.Id == productId);

                if (product == null)
                {
                    return StatusCode(404, $"Product with id {productId} not found!");
                }

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    GroupId = product.GroupId,
                    GroupName = product.Group.Name
                };

                return Ok(productDTO);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet(template: "GetProductByName")]
    public ActionResult<ProductDTO> GetProductByName(string productName)
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Product? product = context.Products
                    .Include(product => product.Group)
                    .FirstOrDefault(x => x.Name.ToLower().Equals(productName.ToLower()));

                if (product == null)
                {
                    return StatusCode(404, $"Product with name {productName} not found!");
                }

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    GroupId = product.GroupId,
                    GroupName = product.Group.Name
                };

                return Ok(productDTO);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPost(template: "PostProduct")]
    public ActionResult<int> PostProduct(
        string productName,
        string? productDescription,
        string groupName,
        int productPrice
    )
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Group? group = context.Groups
                    .FirstOrDefault(x => x.Name.ToLower().Equals(groupName.ToLower()));

                if (group == null)
                {
                    group = new Group
                    {
                        Name = groupName
                    };
                }

                Product? product = context.Products
                    .FirstOrDefault(x => x.Name.ToLower().Equals(productName.ToLower()));

                if (product != null)
                {
                    product.Description = productDescription;
                    product.Price = productPrice;
                    product.Group = group;
                }
                else
                {
                    product = new Product
                    {
                        Name = productName,
                        Description = productDescription,
                        Price = productPrice,
                        Group = group
                    };
                    context.Products.Add(product);
                }

                context.SaveChanges();

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = productName,
                    Description = productDescription,
                    Price = productPrice,
                    GroupId = group.Id,
                    GroupName = groupName
                };

                return Ok(product.Id);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpDelete(template: "DeleteProductByName")]
    public ActionResult<int> DeleteProductByName(string productName)
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Product? product = context.Products
                    .FirstOrDefault(x => x.Name.ToLower().Equals(productName.ToLower()));

                if (product != null)
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                    return Ok(product.Id);
                }

                return NotFound($"Product with name {productName} not found!");
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpDelete(template: "DeleteProductById")]
    public ActionResult<int> DeleteProductById(int productId)
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Product? product = context.Products
                    .FirstOrDefault(x => x.Id == productId);

                if (product != null)
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                    return Ok(product.Id);
                }

                return NotFound($"Product with id {productId} not found!");
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet(template: "GetAllGroups")]
    public ActionResult<List<GroupDTO>> GetAllGroups()
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                var groups = context.Groups.Select(x => new GroupDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

                return Ok(groups);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPost(template: "PostGroup")]
    public ActionResult<int> PostGroup(string groupName, string? groupDescription)
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Group? group = context.Groups
                    .FirstOrDefault(x => x.Name.ToLower().Equals(groupName.ToLower()));

                if (group == null)
                {
                    group = new Group
                    {
                        Name = groupName,
                        Description = groupDescription
                    };
                    context.Groups.Add(group);
                }
                else
                {
                    group.Description = groupDescription;
                }

                context.SaveChanges();

                return Ok(group.Id);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpDelete(template: "DeleteGroupByName")]
    public ActionResult<int> DeleteGroupByName(string groupName)
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Group? group = context.Groups
                    .Include(group => group.Products)
                    .FirstOrDefault(x => x.Name.ToLower().Equals(groupName.ToLower()));

                if (group == null)
                {
                    return NotFound($"Group with name {groupName} not found");
                }

                var groupProducts = group.Products.Count;
                if (groupProducts != 0)
                {
                    return StatusCode(409, $"Can't delete group. Found {groupProducts} products in group");
                }

                context.Groups.Remove(group);
                context.SaveChanges();
                return Ok(group.Id);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpDelete(template: "DeleteGroupById")]
    public ActionResult<int> DeleteGroupById(int groupId)
    {
        try
        {
            using (var context = new StoreDbContext())
            {
                Group? group = context.Groups
                    .Include(group => group.Products)
                    .FirstOrDefault(x => x.Id == groupId);

                if (group == null)
                {
                    return NotFound($"Group with id {groupId} not found");
                }

                var groupProducts = group.Products.Count;
                if (groupProducts != 0)
                {
                    return StatusCode(409, $"Can't delete group. Found {groupProducts} products in group");
                }

                context.Groups.Remove(group);
                context.SaveChanges();
                return Ok(group.Id);
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }
}