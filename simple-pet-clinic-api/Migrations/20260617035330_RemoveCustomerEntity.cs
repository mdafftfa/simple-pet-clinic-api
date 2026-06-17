using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simple_pet_clinic_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pet_customer_entity_customer_id",
                table: "pet");

            migrationBuilder.DropForeignKey(
                name: "fk_reservation_customer_entity_customer_id",
                table: "reservation");

            migrationBuilder.DropTable(
                name: "customer_entity");

            migrationBuilder.AddForeignKey(
                name: "fk_pet_user_entity_customer_id",
                table: "pet",
                column: "customer_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reservation_user_entity_customer_id",
                table: "reservation",
                column: "customer_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pet_user_entity_customer_id",
                table: "pet");

            migrationBuilder.DropForeignKey(
                name: "fk_reservation_user_entity_customer_id",
                table: "reservation");

            migrationBuilder.CreateTable(
                name: "customer_entity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    no_hp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_entity", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_pet_customer_entity_customer_id",
                table: "pet",
                column: "customer_id",
                principalTable: "customer_entity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reservation_customer_entity_customer_id",
                table: "reservation",
                column: "customer_id",
                principalTable: "customer_entity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
