namespace HW2_StoreWebApi.Db;

public partial class Storesproduct
{
    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}