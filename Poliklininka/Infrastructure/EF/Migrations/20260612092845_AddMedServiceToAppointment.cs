using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class AddMedServiceToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "med_service_id",
                table: "appointments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_med_service_id",
                table: "appointments",
                column: "med_service_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_med_services_med_service_id",
                table: "appointments",
                column: "med_service_id",
                principalTable: "med_services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_med_services_med_service_id",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_med_service_id",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "med_service_id",
                table: "appointments");
        }
    }
}
