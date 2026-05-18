using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "результаты_приема",
                table: "visit_histories",
                newName: "visit_results");

            migrationBuilder.RenameColumn(
                name: "дата_приема",
                table: "visit_histories",
                newName: "visit_date");

            migrationBuilder.RenameColumn(
                name: "время_приема",
                table: "visit_histories",
                newName: "visit_time");

            migrationBuilder.RenameColumn(
                name: "статус_слота",
                table: "schedules",
                newName: "slot_status");

            migrationBuilder.RenameColumn(
                name: "кабинет",
                table: "schedules",
                newName: "office");

            migrationBuilder.RenameColumn(
                name: "дата",
                table: "schedules",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "время_окончания",
                table: "schedules",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "время_начала",
                table: "schedules",
                newName: "end_time");

            migrationBuilder.RenameColumn(
                name: "описание",
                table: "recipes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "дозировка",
                table: "recipe_histories",
                newName: "duration");

            migrationBuilder.RenameColumn(
                name: "длительность",
                table: "recipe_histories",
                newName: "dosage");

            migrationBuilder.RenameColumn(
                name: "фио",
                table: "patients",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "стоимость",
                table: "med_services",
                newName: "cost");

            migrationBuilder.RenameColumn(
                name: "название_услуги",
                table: "med_services",
                newName: "service_name");

            migrationBuilder.RenameColumn(
                name: "категория",
                table: "med_services",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "хронические_заболевания",
                table: "med_cards",
                newName: "chronic_diseases");

            migrationBuilder.RenameColumn(
                name: "номер_карты",
                table: "med_cards",
                newName: "card_number");

            migrationBuilder.RenameColumn(
                name: "инвалидность",
                table: "med_cards",
                newName: "disability");

            migrationBuilder.RenameColumn(
                name: "дата_рождения",
                table: "med_cards",
                newName: "open_date");

            migrationBuilder.RenameColumn(
                name: "дата_открытия",
                table: "med_cards",
                newName: "date_of_birth");

            migrationBuilder.RenameColumn(
                name: "аллергии",
                table: "med_cards",
                newName: "allergies");

            migrationBuilder.RenameColumn(
                name: "фио",
                table: "doctors",
                newName: "specialization");

            migrationBuilder.RenameColumn(
                name: "специализация",
                table: "doctors",
                newName: "office");

            migrationBuilder.RenameColumn(
                name: "кабинет",
                table: "doctors",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "резус_фактор",
                table: "blood_groups",
                newName: "rh_factor");

            migrationBuilder.RenameColumn(
                name: "название",
                table: "blood_groups",
                newName: "office");

            migrationBuilder.RenameColumn(
                name: "кабинет",
                table: "blood_groups",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "статус_записи",
                table: "appointments",
                newName: "booking_status");

            migrationBuilder.RenameColumn(
                name: "дата_создания_записи",
                table: "appointments",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "дата_приема",
                table: "appointments",
                newName: "appointment_date");

            migrationBuilder.RenameColumn(
                name: "результат",
                table: "analysis_histories",
                newName: "result");

            migrationBuilder.RenameColumn(
                name: "описание",
                table: "analyses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "название_анализа",
                table: "analyses",
                newName: "analysis_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "visit_time",
                table: "visit_histories",
                newName: "время_приема");

            migrationBuilder.RenameColumn(
                name: "visit_results",
                table: "visit_histories",
                newName: "результаты_приема");

            migrationBuilder.RenameColumn(
                name: "visit_date",
                table: "visit_histories",
                newName: "дата_приема");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "schedules",
                newName: "время_окончания");

            migrationBuilder.RenameColumn(
                name: "slot_status",
                table: "schedules",
                newName: "статус_слота");

            migrationBuilder.RenameColumn(
                name: "office",
                table: "schedules",
                newName: "кабинет");

            migrationBuilder.RenameColumn(
                name: "end_time",
                table: "schedules",
                newName: "время_начала");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "schedules",
                newName: "дата");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "recipes",
                newName: "описание");

            migrationBuilder.RenameColumn(
                name: "duration",
                table: "recipe_histories",
                newName: "дозировка");

            migrationBuilder.RenameColumn(
                name: "dosage",
                table: "recipe_histories",
                newName: "длительность");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "patients",
                newName: "фио");

            migrationBuilder.RenameColumn(
                name: "service_name",
                table: "med_services",
                newName: "название_услуги");

            migrationBuilder.RenameColumn(
                name: "cost",
                table: "med_services",
                newName: "стоимость");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "med_services",
                newName: "категория");

            migrationBuilder.RenameColumn(
                name: "open_date",
                table: "med_cards",
                newName: "дата_рождения");

            migrationBuilder.RenameColumn(
                name: "disability",
                table: "med_cards",
                newName: "инвалидность");

            migrationBuilder.RenameColumn(
                name: "date_of_birth",
                table: "med_cards",
                newName: "дата_открытия");

            migrationBuilder.RenameColumn(
                name: "chronic_diseases",
                table: "med_cards",
                newName: "хронические_заболевания");

            migrationBuilder.RenameColumn(
                name: "card_number",
                table: "med_cards",
                newName: "номер_карты");

            migrationBuilder.RenameColumn(
                name: "allergies",
                table: "med_cards",
                newName: "аллергии");

            migrationBuilder.RenameColumn(
                name: "specialization",
                table: "doctors",
                newName: "фио");

            migrationBuilder.RenameColumn(
                name: "office",
                table: "doctors",
                newName: "специализация");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "doctors",
                newName: "кабинет");

            migrationBuilder.RenameColumn(
                name: "rh_factor",
                table: "blood_groups",
                newName: "резус_фактор");

            migrationBuilder.RenameColumn(
                name: "office",
                table: "blood_groups",
                newName: "название");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "blood_groups",
                newName: "кабинет");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "appointments",
                newName: "дата_создания_записи");

            migrationBuilder.RenameColumn(
                name: "booking_status",
                table: "appointments",
                newName: "статус_записи");

            migrationBuilder.RenameColumn(
                name: "appointment_date",
                table: "appointments",
                newName: "дата_приема");

            migrationBuilder.RenameColumn(
                name: "result",
                table: "analysis_histories",
                newName: "результат");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "analyses",
                newName: "описание");

            migrationBuilder.RenameColumn(
                name: "analysis_name",
                table: "analyses",
                newName: "название_анализа");
        }
    }
}
