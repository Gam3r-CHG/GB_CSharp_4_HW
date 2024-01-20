using Microsoft.EntityFrameworkCore;

namespace HW1_StoreWebApi.Models;

public partial class StoreDbContext : DbContext
{
    public StoreDbContext()
    {
        Database.EnsureCreated();
    }

    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Storesproduct> Storesproducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get connection string from appsettings.json
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString).UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("groups_pk");

            entity.ToTable("groups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pk");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.GroupId).HasColumnName("group_id");

            entity.HasOne(d => d.Group).WithMany(p => p.Products)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_groups_id_fk");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stores_pk");

            entity.ToTable("stores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Storesproduct>(entity =>
        {
            entity.HasKey(e => new {e.StoreId, e.ProductId}).HasName("storesproducts_pk");

            entity.ToTable("storesproducts");

            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasOne(d => d.Product).WithMany(p => p.Storesproducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("storesproducts_products_id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Storesproducts)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("storesproducts_stores_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}