using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RechargeFunctions.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Adding_IsDeleted_To_Cliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Recargas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Recargas");
        }
    }
}
