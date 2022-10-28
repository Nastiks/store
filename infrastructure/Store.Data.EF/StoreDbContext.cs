using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<JewelryDto> Jewelries { get; set; }

        public DbSet<OrderDto> Orders { get; set; }

        public DbSet<OrderItemDto> OrderItems { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildJewelries(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);
        }

        private void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDto>(action =>
            {
                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasOne(dto => dto.Order)
                      .WithMany(dto => dto.Items)
                      .IsRequired();
            });
        }

        private static void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>(action =>
            {
                action.Property(dto => dto.CellPhone)
                      .HasMaxLength(20);

                action.Property(dto => dto.DeliveryUniqueCode)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryPrice)
                      .HasColumnType("money");

                action.Property(dto => dto.PaymentServiceName)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryParameters)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.PaymentParameters)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);
            });
        }

        private static readonly ValueComparer DictionaryComparer =
                   new ValueComparer<Dictionary<string, string>>(
                       (dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2),
                       dictionary => dictionary.Aggregate(
                           0,
                           (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode()), p.Value.GetHashCode())));

        private static void BuildJewelries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JewelryDto>(action =>
            {
                action.Property(dto => dto.VendorCode)
                      .HasMaxLength(23)
                      .IsRequired();

                action.Property(dto => dto.Title)
                      .IsRequired();

                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasData(
                    new JewelryDto
                    {
                        Id = 1,
                        VendorCode = "VENDORCODE0000000001",
                        Material = "Epoxy resin and peonis",
                        Title = "Earrings with peonies",
                        Description = "Earrings made of jewelry resin with hypoallergenic accessories and pink peonies inside",
                        Price = 2000m,
                    },
                    new JewelryDto
                    {
                        Id = 2,
                        VendorCode = "VENDORCODE0000000002",
                        Material = "Epoxy resin and rose",
                        Title = "Rose pendant",
                        Description = "Pendant made of jewelry resin in the form of a drop with a red rose inside",
                        Price = 1200m,
                    },
                    new JewelryDto
                    {
                        Id = 3,
                        VendorCode = "VENDORCODE0000000003",
                        Material = "Pearl",
                        Title = "Pearl Necklace",
                        Description = "A necklace made of natural pearls that will adorn any woman",
                        Price = 3000m,
                    });
            });
        }
    }
}
