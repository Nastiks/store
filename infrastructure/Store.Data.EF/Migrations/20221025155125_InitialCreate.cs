using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.EF.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jewelries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorCode = table.Column<string>(maxLength: 23, nullable: false),
                    Material = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jewelries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CellPhone = table.Column<string>(maxLength: 20, nullable: true),
                    DeliveryUniqueCode = table.Column<string>(maxLength: 40, nullable: true),
                    DeliveryDescription = table.Column<string>(nullable: true),
                    DeliveryPrice = table.Column<decimal>(type: "money", nullable: false),
                    DeliveryParameters = table.Column<string>(nullable: true),
                    PaymentServiceName = table.Column<string>(maxLength: 40, nullable: true),
                    PaymentDescription = table.Column<string>(nullable: true),
                    PaymentParameters = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JewelryId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Count = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Jewelries",

                columns: new[] { "Id", "Material", "Description", "VendorCode", "Price", "Title" },
                values: new object[] { 1,"Epoxy resin and peonis", "Earrings made of jewelry resin with hypoallergenic accessories and pink peonies inside", "VENDORCODE0000000001", 2000m, "Earrings with peonies"});

            migrationBuilder.InsertData(
                table: "Jewelries",
                columns: new[] { "Id", "Material", "Description", "VendorCode", "Price", "Title" },
                values: new object[] { 2, "Epoxy resin and rose", "Pendant made of jewelry resin in the form of a drop with a red rose inside", "VENDORCODE0000000002", 1200m, "Rose pendant"});

            migrationBuilder.InsertData(
                table: "Jewelries",
                columns: new[] { "Id", "Material", "Description", "VendorCode", "Price", "Title" },
                values: new object[] { 3,"Pearl", "A necklace made of natural pearls that will adorn any woman", "VENDORCODE0000000003", 3000m, "Pearl Necklace"});


            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jewelries");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
