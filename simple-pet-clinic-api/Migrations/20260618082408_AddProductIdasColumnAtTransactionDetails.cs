using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simple_pet_clinic_api.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIdasColumnAtTransactionDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transaction_details_service_product_item_id",
                table: "transaction_details");

            migrationBuilder.RenameColumn(
                name: "item_id",
                table: "transaction_details",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "ix_transaction_details_item_id",
                table: "transaction_details",
                newName: "ix_transaction_details_product_id");

            migrationBuilder.RenameColumn(
                name: "pay_time",
                table: "transaction",
                newName: "transaction_date");

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                table: "transaction",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_transaction_customer_id",
                table: "transaction",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "fk_transaction_user_entity_customer_id",
                table: "transaction",
                column: "customer_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_transaction_details_service_product_product_id",
                table: "transaction_details",
                column: "product_id",
                principalTable: "service_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transaction_user_entity_customer_id",
                table: "transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_transaction_details_service_product_product_id",
                table: "transaction_details");

            migrationBuilder.DropIndex(
                name: "ix_transaction_customer_id",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "transaction");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "transaction_details",
                newName: "item_id");

            migrationBuilder.RenameIndex(
                name: "ix_transaction_details_product_id",
                table: "transaction_details",
                newName: "ix_transaction_details_item_id");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                table: "transaction",
                newName: "pay_time");

            migrationBuilder.AddForeignKey(
                name: "fk_transaction_details_service_product_item_id",
                table: "transaction_details",
                column: "item_id",
                principalTable: "service_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
