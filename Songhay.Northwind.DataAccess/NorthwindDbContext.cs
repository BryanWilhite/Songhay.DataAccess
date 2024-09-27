using Microsoft.EntityFrameworkCore;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess;

public class NorthwindDbContext : DbContext
{
    public NorthwindDbContext() : this(new DbContextOptions<NorthwindDbContext> {}) {}

    public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options) : base(options)
    {
        Categories = Set<Category>();
        CustomerDemographics = Set<CustomerDemographic>();
        Customers = Set<Customer>();
        Employees = Set<Employee>();
        Invoices = Set<Invoice>();
        Orders = Set<Order>();
        OrderDetails = Set<OrderDetail>();
        Products = Set<Product>();
        Regions = Set<Region>();
        Shippers = Set<Shipper>();
        Suppliers = Set<Supplier>();
        Territories = Set<Territory>();

        #region views:

        SalesByCategories = Set<SalesByCategory>();
        SalesTotalsByAmounts = Set<SalesTotalsByAmount>();
        SummaryOfSalesByQuarters = Set<SummaryOfSalesByQuarter>();
        SummaryOfSalesByYears = Set<SummaryOfSalesByYear>();

        #endregion
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; } = null!;
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Region> Regions { get; set; }
    public virtual DbSet<Shipper> Shippers { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public virtual DbSet<Territory> Territories { get; set; }

    #region views:

    public virtual DbSet<SalesByCategory> SalesByCategories { get; set; }
    public virtual DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; }
    public virtual DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; }
    public virtual DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
        });

        modelBuilder.ApplyConfiguration(new Configurations.CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.InvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.OrderConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.OrderDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ProductConfiguration());

        modelBuilder.Entity<Region>(entity =>
        {
            entity.Property(e => e.RegionId)
                .ValueGeneratedNever()
                .HasColumnName("RegionID");
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.Property(e => e.ShipperId).HasColumnName("ShipperID");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
        });

        modelBuilder.ApplyConfiguration(new Configurations.TerritoryConfiguration());

        #region views:

        modelBuilder.Entity<SalesByCategory>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Sales by Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
        });

        modelBuilder.Entity<SalesTotalsByAmount>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Sales Totals by Amount");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
        });

        modelBuilder.Entity<SummaryOfSalesByQuarter>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Summary of Sales by Quarter");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
        });

        modelBuilder.Entity<SummaryOfSalesByYear>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("Summary of Sales by Year");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");
        });

        #endregion
    }
}
