using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class _14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hronic_diseases_patient");

            migrationBuilder.CreateTable(
                name: "сronic_diseases_patient",
                columns: table => new
                {
                    medcard_id = table.Column<int>(type: "integer", nullable: false),
                    chronic_diseases_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_сronic_diseases_patient", x => new { x.medcard_id, x.chronic_diseases_id });
                    table.ForeignKey(
                        name: "FK_сronic_diseases_patient_hronic_diseases_chronic_diseases_id",
                        column: x => x.chronic_diseases_id,
                        principalTable: "hronic_diseases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_сronic_diseases_patient_med_cards_medcard_id",
                        column: x => x.medcard_id,
                        principalTable: "med_cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_сronic_diseases_patient_chronic_diseases_id",
                table: "сronic_diseases_patient",
                column: "chronic_diseases_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "сronic_diseases_patient");

            migrationBuilder.CreateTable(
                name: "hronic_diseases_patient",
                columns: table => new
                {
                    medcard_id = table.Column<int>(type: "integer", nullable: false),
                    chronic_diseases_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hronic_diseases_patient", x => new { x.medcard_id, x.chronic_diseases_id });
                    table.ForeignKey(
                        name: "FK_hronic_diseases_patient_hronic_diseases_chronic_diseases_id",
                        column: x => x.chronic_diseases_id,
                        principalTable: "hronic_diseases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hronic_diseases_patient_med_cards_medcard_id",
                        column: x => x.medcard_id,
                        principalTable: "med_cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hronic_diseases_patient_chronic_diseases_id",
                table: "hronic_diseases_patient",
                column: "chronic_diseases_id");
        }
    }
}
