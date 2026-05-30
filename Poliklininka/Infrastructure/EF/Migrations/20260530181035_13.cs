using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class _13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "chronic_diseases",
                table: "med_cards",
                newName: "ChronicDiseases");

            migrationBuilder.CreateTable(
                name: "hronic_diseases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hronic_diseases", x => x.id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hronic_diseases_patient");

            migrationBuilder.DropTable(
                name: "hronic_diseases");

            migrationBuilder.RenameColumn(
                name: "ChronicDiseases",
                table: "med_cards",
                newName: "chronic_diseases");
        }
    }
}
