using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nokubico.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueProductSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_product_Slug",
                table: "product",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_product_Slug",
                table: "product");
        }
    }
}
