using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_сronic_diseases_patient_med_cards_medcard_id",
                table: "сronic_diseases_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_сronic_diseases_patient_сronic_diseases_chronic_diseases_id",
                table: "сronic_diseases_patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_сronic_diseases_patient",
                table: "сronic_diseases_patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_сronic_diseases",
                table: "сronic_diseases");

            migrationBuilder.RenameTable(
                name: "сronic_diseases_patient",
                newName: "chronic_diseases_patient");

            migrationBuilder.RenameTable(
                name: "сronic_diseases",
                newName: "chronic_diseases");

            migrationBuilder.RenameIndex(
                name: "IX_сronic_diseases_patient_chronic_diseases_id",
                table: "chronic_diseases_patient",
                newName: "IX_chronic_diseases_patient_chronic_diseases_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chronic_diseases_patient",
                table: "chronic_diseases_patient",
                columns: new[] { "medcard_id", "chronic_diseases_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_chronic_diseases",
                table: "chronic_diseases",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_chronic_diseases_patient_chronic_diseases_chronic_diseases_~",
                table: "chronic_diseases_patient",
                column: "chronic_diseases_id",
                principalTable: "chronic_diseases",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chronic_diseases_patient_med_cards_medcard_id",
                table: "chronic_diseases_patient",
                column: "medcard_id",
                principalTable: "med_cards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chronic_diseases_patient_chronic_diseases_chronic_diseases_~",
                table: "chronic_diseases_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_chronic_diseases_patient_med_cards_medcard_id",
                table: "chronic_diseases_patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chronic_diseases_patient",
                table: "chronic_diseases_patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chronic_diseases",
                table: "chronic_diseases");

            migrationBuilder.RenameTable(
                name: "chronic_diseases_patient",
                newName: "сronic_diseases_patient");

            migrationBuilder.RenameTable(
                name: "chronic_diseases",
                newName: "сronic_diseases");

            migrationBuilder.RenameIndex(
                name: "IX_chronic_diseases_patient_chronic_diseases_id",
                table: "сronic_diseases_patient",
                newName: "IX_сronic_diseases_patient_chronic_diseases_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_сronic_diseases_patient",
                table: "сronic_diseases_patient",
                columns: new[] { "medcard_id", "chronic_diseases_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_сronic_diseases",
                table: "сronic_diseases",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_сronic_diseases_patient_med_cards_medcard_id",
                table: "сronic_diseases_patient",
                column: "medcard_id",
                principalTable: "med_cards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_сronic_diseases_patient_сronic_diseases_chronic_diseases_id",
                table: "сronic_diseases_patient",
                column: "chronic_diseases_id",
                principalTable: "сronic_diseases",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
