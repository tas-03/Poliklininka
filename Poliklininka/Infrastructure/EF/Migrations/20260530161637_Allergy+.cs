using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class Allergy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_Users_id",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_Users_id",
                table: "patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receptions",
                table: "Receptions");

            migrationBuilder.DropColumn(
                name: "allergies",
                table: "med_cards");

            migrationBuilder.DropColumn(
                name: "card_number",
                table: "med_cards");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Receptions",
                newName: "receptions");

            migrationBuilder.RenameColumn(
                name: "Phone_number",
                table: "patients",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "Insurance_Policy",
                table: "patients",
                newName: "insurance_Policy");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "patients",
                newName: "address");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_receptions",
                table: "receptions",
                column: "ReceptionId");

            migrationBuilder.CreateTable(
                name: "allergies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allergies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "allergy_patient",
                columns: table => new
                {
                    medcard_id = table.Column<int>(type: "integer", nullable: false),
                    allergy_id = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allergy_patient", x => new { x.medcard_id, x.allergy_id });
                    table.ForeignKey(
                        name: "FK_allergy_patient_allergies_allergy_id",
                        column: x => x.allergy_id,
                        principalTable: "allergies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_allergy_patient_med_cards_medcard_id",
                        column: x => x.medcard_id,
                        principalTable: "med_cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_allergy_patient_patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_allergy_patient_allergy_id",
                table: "allergy_patient",
                column: "allergy_id");

            migrationBuilder.CreateIndex(
                name: "IX_allergy_patient_PatientId",
                table: "allergy_patient",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_users_id",
                table: "doctors",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_users_id",
                table: "patients",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_users_id",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_users_id",
                table: "patients");

            migrationBuilder.DropTable(
                name: "allergy_patient");

            migrationBuilder.DropTable(
                name: "allergies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_receptions",
                table: "receptions");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "receptions",
                newName: "Receptions");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "patients",
                newName: "Phone_number");

            migrationBuilder.RenameColumn(
                name: "insurance_Policy",
                table: "patients",
                newName: "Insurance_Policy");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "patients",
                newName: "Address");

            migrationBuilder.AddColumn<string>(
                name: "allergies",
                table: "med_cards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "card_number",
                table: "med_cards",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receptions",
                table: "Receptions",
                column: "ReceptionId");

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
    }
}
