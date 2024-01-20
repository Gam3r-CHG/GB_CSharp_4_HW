namespace HW1_StoreWebApi.Models;

public partial class Store
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Storesproduct> Storesproducts { get; set; } = new List<Storesproduct>();
}
