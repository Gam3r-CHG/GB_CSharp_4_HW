using AutoMapper;
using HW2_StoreWebApi.Abstraction;
using HW2_StoreWebApi.Db;
using HW2_StoreWebApi.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace HW2_StoreWebApi.Repository;

public class GroupRepository : IGroupRepository
{
    private IMemoryCache _cache;
    private IMapper _mapper;
    private AppDbContext _context;

    public GroupRepository(IMemoryCache cache, IMapper mapper, AppDbContext context)
    {
        _cache = cache;
        _mapper = mapper;
        _context = context;
    }

    public IEnumerable<GroupDto> GetAllGroups()
    {
        if (_cache.TryGetValue("groups", out IEnumerable<GroupDto> groupDtos))
        {
            return groupDtos;
        }

        using (_context)
        {
            groupDtos = _context.Groups
                .Select(_mapper.Map<GroupDto>)
                .ToList();
            _cache.Set("groups", groupDtos, TimeSpan.FromMinutes(30));
            return groupDtos;
        }
    }

    public int AddGroup(GroupDto groupDto)
    {
        using (_context)
        {
            Group? group = _context.Groups
                .FirstOrDefault(x => x.Name.ToLower().Equals(groupDto.Name.ToLower()));

            if (group != null)
            {
                throw new Exception("Group with the same name already exists");
            }

            group = _mapper.Map<Group>(groupDto);
            _context.Groups.Add(group);
            _context.SaveChanges();

            _cache.Remove("groups");

            return group.Id;
        }
    }
}