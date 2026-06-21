using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simple_pet_clinic_api.Migrations
{
    /// <inheritdoc />
    public partial class RecreateMedicalAndGroomingRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "medical_results",
                table: "medical_record",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "grooming_results",
                table: "grooming_record",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "medical_results",
                table: "medical_record");

            migrationBuilder.DropColumn(
                name: "grooming_results",
                table: "grooming_record");
        }
    }
}
