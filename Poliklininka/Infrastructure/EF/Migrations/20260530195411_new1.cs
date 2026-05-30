using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class new1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_allergy_patient_patients_PatientId",
                table: "allergy_patient");

            migrationBuilder.DropIndex(
                name: "IX_allergy_patient_PatientId",
                table: "allergy_patient");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "allergy_patient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "allergy_patient",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_allergy_patient_PatientId",
                table: "allergy_patient",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_allergy_patient_patients_PatientId",
                table: "allergy_patient",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "id");
        }
    }
}
