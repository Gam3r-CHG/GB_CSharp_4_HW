namespace HW2_StoreWebApi.Db;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Price { get; set; }

    public int? GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<Storesproduct> Storesproducts { get; set; } = new List<Storesproduct>();
}