﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorCode = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tittle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CellPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DeliveryUniqueCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DeliveryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryPrice = table.Column<decimal>(type: "money", nullable: false),
                    DeliveryParameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentServiceName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PaymentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentParameters = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JewelryId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
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
                columns: new[] { "Id", "Description", "Material", "Price", "Tittle", "VendorCode" },
                values: new object[] { 1, "Earrings made of jewelry resin with hypoallergenic accessories and pink peonies inside", "Epoxy resin and peonis", 2000m, "Earrings with peonies", "VendorCode0000000001" });

            migrationBuilder.InsertData(
                table: "Jewelries",
                columns: new[] { "Id", "Description", "Material", "Price", "Tittle", "VendorCode" },
                values: new object[] { 2, "Pendant made of jewelry resin in the form of a drop with a red rose inside", "Epoxy resin and rose", 1200m, "Rose pendant", "VendorCode0000000002" });

            migrationBuilder.InsertData(
                table: "Jewelries",
                columns: new[] { "Id", "Description", "Material", "Price", "Tittle", "VendorCode" },
                values: new object[] { 3, "A necklace made of natural pearls that will adorn any woman", "Pearl", 3000m, "Pearl Necklace", "VendorCode0000000003" });

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
