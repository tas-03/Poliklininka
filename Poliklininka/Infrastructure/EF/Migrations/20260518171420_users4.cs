using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class users4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_Users_Id",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_Users_Id",
                table: "patients");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "patients",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "doctors",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_Users_id",
                table: "doctors",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_Users_id",
                table: "patients",
                column: "id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_Users_id",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_Users_id",
                table: "patients");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "patients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "doctors",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_Users_Id",
                table: "doctors",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_Users_Id",
                table: "patients",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
