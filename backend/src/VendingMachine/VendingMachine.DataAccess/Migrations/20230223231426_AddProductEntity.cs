using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendingMachine.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AmountAvailable = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SellerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: "30A07246-8B22-4F6C-B41B-408A9D0ADAFE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0c141469-ac47-4a50-99f2-062fa60e606c", "AQAAAAIAAYagAAAAEPnZ7gj7irhpmjaeNA6LF3+Z8cH8Jfcel3UX+0sTj6Fwa0axTh9ZmKxpSqbAFUWcSg==" });

            migrationBuilder.CreateIndex(
                name: "IX_products_SellerId",
                table: "products",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: "30A07246-8B22-4F6C-B41B-408A9D0ADAFE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0601c8ef-149c-47f7-bb4c-984494a97b4d", "AQAAAAIAAYagAAAAEKt02TGmPlmUWI4AEdz1/oe6BfLbM4NjbkOAmbUVfobZKVIUtelb5n+pF0WKhhU5hw==" });
        }
    }
}
