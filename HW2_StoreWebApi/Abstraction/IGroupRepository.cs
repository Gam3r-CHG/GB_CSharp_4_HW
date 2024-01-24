using HW2_StoreWebApi.Dto;

namespace HW2_StoreWebApi.Abstraction;

public interface IGroupRepository
{
    public IEnumerable<GroupDto> GetAllGroups();
    public int AddGroup(GroupDto groupDto);
}