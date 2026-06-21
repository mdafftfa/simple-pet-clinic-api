using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simple_pet_clinic_api.Migrations
{
    /// <inheritdoc />
    public partial class AddGroomingRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "diagnosis_results",
                table: "medical_record");

            migrationBuilder.CreateTable(
                name: "grooming_record",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    check_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reservation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    groomer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grooming_record", x => x.id);
                    table.ForeignKey(
                        name: "fk_grooming_record_pet_pet_id",
                        column: x => x.pet_id,
                        principalTable: "pet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grooming_record_reservation_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "reservation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grooming_record_user_entity_groomer_id",
                        column: x => x.groomer_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_grooming_record_groomer_id",
                table: "grooming_record",
                column: "groomer_id");

            migrationBuilder.CreateIndex(
                name: "ix_grooming_record_pet_id",
                table: "grooming_record",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_grooming_record_reservation_id",
                table: "grooming_record",
                column: "reservation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grooming_record");

            migrationBuilder.AddColumn<string>(
                name: "diagnosis_results",
                table: "medical_record",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
