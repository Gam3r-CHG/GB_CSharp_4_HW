using HW2_StoreWebApi.Abstraction;
using HW2_StoreWebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HW2_StoreWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController : ControllerBase
{
    private IGroupRepository _repository;

    public GroupController(IGroupRepository repository)
    {
        _repository = repository;
    }

    [HttpGet(template: "get_all_groups")]
    public ActionResult<List<ProductDto>> GetAllGroups()
    {
        try
        {
            var groups = _repository.GetAllGroups();
            return Ok(groups);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }

    [HttpPost("add_group")]
    public ActionResult<int> AddGroup(GroupDto groupDto)
    {
        try
        {
            var groupId = _repository.AddGroup(groupDto);
            return Ok(groupId);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException?.Message ?? e.Message);
        }
    }
}