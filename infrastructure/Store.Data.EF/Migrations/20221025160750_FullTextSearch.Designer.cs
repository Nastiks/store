using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.EF.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20221025160750_FullTextSearch")]
    partial class FullTextSearch
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Store.Data.JewelryDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(23)")
                        .HasMaxLength(23);

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Jewelries");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Material = "Epoxy resin and peonis",
                            Description = "Earrings made of jewelry resin with hypoallergenic accessories and pink peonies inside",
                            VendorCode = "VENDORCODE0000000001",
                            Price = 2000m,
                            Title = "Earrings with peonies"

                        },
                        new
                        {
                            Id = 2,
                            Material = "Epoxy resin and rose",
                            Description = "Pendant made of jewelry resin in the form of a drop with a red rose inside",
                            VendorCode = "VENDORCODE0000000002",
                            Price = 1200m,
                            Title = "Rose pendant"

                        },
                        new
                        {
                            Id = 3,
                            Material = "Pearl",
                            Description = "A necklace made of natural pearls that will adorn any woman",
                            VendorCode = "VENDORCODE0000000003",
                            Price = 3000m,
                            Title = "Pearl Necklace"

                        });
                });

            modelBuilder.Entity("Store.Data.OrderDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CellPhone")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("DeliveryDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryParameters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DeliveryPrice")
                        .HasColumnType("money");

                    b.Property<string>("DeliveryUniqueCode")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("PaymentDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentParameters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentServiceName")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Store.Data.OrderItemDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("JewelryId")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Store.Data.OrderItemDto", b =>
                {
                    b.HasOne("Store.Data.OrderDto", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                 });            
#pragma warning restore 612, 618
        }
    }
}
