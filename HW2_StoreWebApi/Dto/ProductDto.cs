namespace HW2_StoreWebApi.Dto;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Price { get; set; }

    public int? GroupId { get; set; }

    public override string ToString()
    {
        return $"Id: {Id} Name: {Name} D: {Description} Price: {Price} GroupId: {GroupId}";
    }
}