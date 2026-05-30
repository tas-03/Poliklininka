using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class _17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_сronic_diseases_patient_hronic_diseases_chronic_diseases_id",
                table: "сronic_diseases_patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hronic_diseases",
                table: "hronic_diseases");

            migrationBuilder.RenameTable(
                name: "hronic_diseases",
                newName: "сronic_diseases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_сronic_diseases",
                table: "сronic_diseases",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_сronic_diseases_patient_сronic_diseases_chronic_diseases_id",
                table: "сronic_diseases_patient",
                column: "chronic_diseases_id",
                principalTable: "сronic_diseases",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_сronic_diseases_patient_сronic_diseases_chronic_diseases_id",
                table: "сronic_diseases_patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_сronic_diseases",
                table: "сronic_diseases");

            migrationBuilder.RenameTable(
                name: "сronic_diseases",
                newName: "hronic_diseases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hronic_diseases",
                table: "hronic_diseases",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_сronic_diseases_patient_hronic_diseases_chronic_diseases_id",
                table: "сronic_diseases_patient",
                column: "chronic_diseases_id",
                principalTable: "hronic_diseases",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
