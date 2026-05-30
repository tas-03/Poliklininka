using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class users2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Users",
                newName: "discriminator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discriminator",
                table: "Users",
                newName: "Discriminator");
        }
    }
}
