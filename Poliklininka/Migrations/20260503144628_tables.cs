using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Poliklininka.Migrations
{
    /// <inheritdoc />
    public partial class tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "analyses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    название_анализа = table.Column<string>(type: "text", nullable: false),
                    описание = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_analyses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blood_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    название = table.Column<string>(type: "text", nullable: false),
                    резус_фактор = table.Column<string>(type: "text", nullable: false),
                    кабинет = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blood_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "doctors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    фио = table.Column<string>(type: "text", nullable: false),
                    специализация = table.Column<string>(type: "text", nullable: false),
                    кабинет = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "med_services",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    название_услуги = table.Column<string>(type: "text", nullable: false),
                    стоимость = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    категория = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_med_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    patient_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    фио = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.patient_id);
                });

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    описание = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    doctor_id = table.Column<int>(type: "integer", nullable: false),
                    дата = table.Column<DateOnly>(type: "date", nullable: false),
                    время_начала = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    время_окончания = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    кабинет = table.Column<string>(type: "text", nullable: false),
                    статус_слота = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_schedules_doctors_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "med_cards",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patient_id = table.Column<int>(type: "integer", nullable: false),
                    blood_group_id = table.Column<int>(type: "integer", nullable: false),
                    номер_карты = table.Column<string>(type: "text", nullable: false),
                    дата_открытия = table.Column<DateOnly>(type: "date", nullable: false),
                    аллергии = table.Column<string>(type: "text", nullable: true),
                    хронические_заболевания = table.Column<string>(type: "text", nullable: true),
                    инвалидность = table.Column<bool>(type: "boolean", nullable: false),
                    дата_рождения = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_med_cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_med_cards_blood_groups_blood_group_id",
                        column: x => x.blood_group_id,
                        principalTable: "blood_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_med_cards_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patient_id = table.Column<int>(type: "integer", nullable: false),
                    doctor_id = table.Column<int>(type: "integer", nullable: false),
                    schedule_id = table.Column<int>(type: "integer", nullable: false),
                    дата_приема = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    статус_записи = table.Column<string>(type: "text", nullable: false),
                    дата_создания_записи = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_appointments_doctors_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_schedules_schedule_id",
                        column: x => x.schedule_id,
                        principalTable: "schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "visit_histories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<int>(type: "integer", nullable: false),
                    patient_id = table.Column<int>(type: "integer", nullable: false),
                    med_service_id = table.Column<int>(type: "integer", nullable: false),
                    дата_приема = table.Column<DateOnly>(type: "date", nullable: false),
                    время_приема = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    результаты_приема = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visit_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_visit_histories_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_visit_histories_med_services_med_service_id",
                        column: x => x.med_service_id,
                        principalTable: "med_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_visit_histories_patients_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patients",
                        principalColumn: "patient_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "analysis_histories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    visit_history_id = table.Column<int>(type: "integer", nullable: false),
                    analysis_id = table.Column<int>(type: "integer", nullable: false),
                    результат = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_analysis_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_analysis_histories_analyses_analysis_id",
                        column: x => x.analysis_id,
                        principalTable: "analyses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_analysis_histories_visit_histories_visit_history_id",
                        column: x => x.visit_history_id,
                        principalTable: "visit_histories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_histories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    visit_history_id = table.Column<int>(type: "integer", nullable: false),
                    recipe_id = table.Column<int>(type: "integer", nullable: false),
                    дозировка = table.Column<string>(type: "text", nullable: false),
                    длительность = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipe_histories_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipe_histories_visit_histories_visit_history_id",
                        column: x => x.visit_history_id,
                        principalTable: "visit_histories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_analysis_histories_analysis_id",
                table: "analysis_histories",
                column: "analysis_id");

            migrationBuilder.CreateIndex(
                name: "IX_analysis_histories_visit_history_id",
                table: "analysis_histories",
                column: "visit_history_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_doctor_id",
                table: "appointments",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_patient_id",
                table: "appointments",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_schedule_id",
                table: "appointments",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_med_cards_blood_group_id",
                table: "med_cards",
                column: "blood_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_med_cards_patient_id",
                table: "med_cards",
                column: "patient_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipe_histories_recipe_id",
                table: "recipe_histories",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_histories_visit_history_id",
                table: "recipe_histories",
                column: "visit_history_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_doctor_id",
                table: "schedules",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_visit_histories_appointment_id",
                table: "visit_histories",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_visit_histories_med_service_id",
                table: "visit_histories",
                column: "med_service_id");

            migrationBuilder.CreateIndex(
                name: "IX_visit_histories_patient_id",
                table: "visit_histories",
                column: "patient_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analysis_histories");

            migrationBuilder.DropTable(
                name: "med_cards");

            migrationBuilder.DropTable(
                name: "recipe_histories");

            migrationBuilder.DropTable(
                name: "analyses");

            migrationBuilder.DropTable(
                name: "blood_groups");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "visit_histories");

            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "med_services");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "doctors");
        }
    }
}
