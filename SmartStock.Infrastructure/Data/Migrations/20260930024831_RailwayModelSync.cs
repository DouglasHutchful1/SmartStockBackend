using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RailwayModelSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_creditor_payments_creditors_CreditorId",
                schema: "dbo",
                table: "creditor_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_creditor_payments_sales_SaleId",
                schema: "dbo",
                table: "creditor_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_creditors_users_UserId",
                schema: "dbo",
                table: "creditors");

            migrationBuilder.DropForeignKey(
                name: "FK_products_users_UserId",
                schema: "dbo",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_products_ProductId",
                schema: "dbo",
                table: "sale_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_sales_SaleId",
                schema: "dbo",
                table: "sale_items");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_creditors_CreditorId",
                schema: "dbo",
                table: "sales");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_users_UserId",
                schema: "dbo",
                table: "sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sale_items",
                schema: "dbo",
                table: "sale_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_creditor_payments",
                schema: "dbo",
                table: "creditor_payments");

            migrationBuilder.DropColumn(
                name: "updatedat",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "sales");

            migrationBuilder.DropColumn(
                name: "updatedat",
                schema: "dbo",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "products");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "creditors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "sale_items");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "sale_items");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "creditor_payments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "creditor_payments");

            migrationBuilder.RenameTable(
                name: "sale_items",
                schema: "dbo",
                newName: "saleitems",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "creditor_payments",
                schema: "dbo",
                newName: "creditorpayments",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "sales",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "dbo",
                table: "sales",
                newName: "totalamount");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "sales",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "SoldAt",
                schema: "dbo",
                table: "sales",
                newName: "soldat");

            migrationBuilder.RenameColumn(
                name: "SaleNumber",
                schema: "dbo",
                table: "sales",
                newName: "salenumber");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                schema: "dbo",
                table: "sales",
                newName: "paymentmethod");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "dbo",
                table: "sales",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "CreditorId",
                schema: "dbo",
                table: "sales",
                newName: "creditorid");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "sales",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "AmountPaid",
                schema: "dbo",
                table: "sales",
                newName: "amountpaid");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "sales",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_sales_UserId_SaleNumber",
                schema: "dbo",
                table: "sales",
                newName: "IX_sales_userid_salenumber");

            migrationBuilder.RenameIndex(
                name: "IX_sales_CreditorId",
                schema: "dbo",
                table: "sales",
                newName: "IX_sales_creditorid");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "products",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "Unit",
                schema: "dbo",
                table: "products",
                newName: "unit");

            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                schema: "dbo",
                table: "products",
                newName: "stockquantity");

            migrationBuilder.RenameColumn(
                name: "Sku",
                schema: "dbo",
                table: "products",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                schema: "dbo",
                table: "products",
                newName: "sellingprice");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "dbo",
                table: "products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LowStockThreshold",
                schema: "dbo",
                table: "products",
                newName: "lowstockthreshold");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "dbo",
                table: "products",
                newName: "isactive");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "dbo",
                table: "products",
                newName: "imageurl");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "dbo",
                table: "products",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "products",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "CostPrice",
                schema: "dbo",
                table: "products",
                newName: "costprice");

            migrationBuilder.RenameColumn(
                name: "Category",
                schema: "dbo",
                table: "products",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Barcode",
                schema: "dbo",
                table: "products",
                newName: "barcode");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "products",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_products_UserId_Sku",
                schema: "dbo",
                table: "products",
                newName: "IX_products_userid_sku");

            migrationBuilder.RenameIndex(
                name: "IX_products_UserId_Barcode",
                schema: "dbo",
                table: "products",
                newName: "IX_products_userid_barcode");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "creditors",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "TotalOwed",
                schema: "dbo",
                table: "creditors",
                newName: "totalowed");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                schema: "dbo",
                table: "creditors",
                newName: "phonenumber");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "dbo",
                table: "creditors",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "dbo",
                table: "creditors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "dbo",
                table: "creditors",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "CreditLimit",
                schema: "dbo",
                table: "creditors",
                newName: "creditlimit");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "creditors",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "Address",
                schema: "dbo",
                table: "creditors",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "creditors",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_creditors_UserId",
                schema: "dbo",
                table: "creditors",
                newName: "IX_creditors_userid");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                schema: "dbo",
                table: "saleitems",
                newName: "unitprice");

            migrationBuilder.RenameColumn(
                name: "Subtotal",
                schema: "dbo",
                table: "saleitems",
                newName: "subtotal");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                schema: "dbo",
                table: "saleitems",
                newName: "saleid");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "dbo",
                table: "saleitems",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                schema: "dbo",
                table: "saleitems",
                newName: "productname");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "dbo",
                table: "saleitems",
                newName: "productid");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "saleitems",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_sale_items_SaleId",
                schema: "dbo",
                table: "saleitems",
                newName: "IX_saleitems_saleid");

            migrationBuilder.RenameIndex(
                name: "IX_sale_items_ProductId",
                schema: "dbo",
                table: "saleitems",
                newName: "IX_saleitems_productid");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                schema: "dbo",
                table: "creditorpayments",
                newName: "saleid");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                schema: "dbo",
                table: "creditorpayments",
                newName: "paymentmethod");

            migrationBuilder.RenameColumn(
                name: "PaidAt",
                schema: "dbo",
                table: "creditorpayments",
                newName: "paidat");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "dbo",
                table: "creditorpayments",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "CreditorId",
                schema: "dbo",
                table: "creditorpayments",
                newName: "creditorid");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "dbo",
                table: "creditorpayments",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "creditorpayments",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_creditor_payments_SaleId",
                schema: "dbo",
                table: "creditorpayments",
                newName: "IX_creditorpayments_saleid");

            migrationBuilder.RenameIndex(
                name: "IX_creditor_payments_CreditorId",
                schema: "dbo",
                table: "creditorpayments",
                newName: "IX_creditorpayments_creditorid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_saleitems",
                schema: "dbo",
                table: "saleitems",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_creditorpayments",
                schema: "dbo",
                table: "creditorpayments",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_creditorpayments_creditors_creditorid",
                schema: "dbo",
                table: "creditorpayments",
                column: "creditorid",
                principalSchema: "dbo",
                principalTable: "creditors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_creditorpayments_sales_saleid",
                schema: "dbo",
                table: "creditorpayments",
                column: "saleid",
                principalSchema: "dbo",
                principalTable: "sales",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_creditors_users_userid",
                schema: "dbo",
                table: "creditors",
                column: "userid",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_users_userid",
                schema: "dbo",
                table: "products",
                column: "userid",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_saleitems_products_productid",
                schema: "dbo",
                table: "saleitems",
                column: "productid",
                principalSchema: "dbo",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_saleitems_sales_saleid",
                schema: "dbo",
                table: "saleitems",
                column: "saleid",
                principalSchema: "dbo",
                principalTable: "sales",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_creditors_creditorid",
                schema: "dbo",
                table: "sales",
                column: "creditorid",
                principalSchema: "dbo",
                principalTable: "creditors",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_users_userid",
                schema: "dbo",
                table: "sales",
                column: "userid",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_creditorpayments_creditors_creditorid",
                schema: "dbo",
                table: "creditorpayments");

            migrationBuilder.DropForeignKey(
                name: "FK_creditorpayments_sales_saleid",
                schema: "dbo",
                table: "creditorpayments");

            migrationBuilder.DropForeignKey(
                name: "FK_creditors_users_userid",
                schema: "dbo",
                table: "creditors");

            migrationBuilder.DropForeignKey(
                name: "FK_products_users_userid",
                schema: "dbo",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_saleitems_products_productid",
                schema: "dbo",
                table: "saleitems");

            migrationBuilder.DropForeignKey(
                name: "FK_saleitems_sales_saleid",
                schema: "dbo",
                table: "saleitems");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_creditors_creditorid",
                schema: "dbo",
                table: "sales");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_users_userid",
                schema: "dbo",
                table: "sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_saleitems",
                schema: "dbo",
                table: "saleitems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_creditorpayments",
                schema: "dbo",
                table: "creditorpayments");

            migrationBuilder.RenameTable(
                name: "saleitems",
                schema: "dbo",
                newName: "sale_items",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "creditorpayments",
                schema: "dbo",
                newName: "creditor_payments",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "userid",
                schema: "dbo",
                table: "sales",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "totalamount",
                schema: "dbo",
                table: "sales",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "status",
                schema: "dbo",
                table: "sales",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "soldat",
                schema: "dbo",
                table: "sales",
                newName: "SoldAt");

            migrationBuilder.RenameColumn(
                name: "salenumber",
                schema: "dbo",
                table: "sales",
                newName: "SaleNumber");

            migrationBuilder.RenameColumn(
                name: "paymentmethod",
                schema: "dbo",
                table: "sales",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "notes",
                schema: "dbo",
                table: "sales",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "creditorid",
                schema: "dbo",
                table: "sales",
                newName: "CreditorId");

            migrationBuilder.RenameColumn(
                name: "createdat",
                schema: "dbo",
                table: "sales",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "amountpaid",
                schema: "dbo",
                table: "sales",
                newName: "AmountPaid");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "sales",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_sales_userid_salenumber",
                schema: "dbo",
                table: "sales",
                newName: "IX_sales_UserId_SaleNumber");

            migrationBuilder.RenameIndex(
                name: "IX_sales_creditorid",
                schema: "dbo",
                table: "sales",
                newName: "IX_sales_CreditorId");

            migrationBuilder.RenameColumn(
                name: "userid",
                schema: "dbo",
                table: "products",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "unit",
                schema: "dbo",
                table: "products",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "stockquantity",
                schema: "dbo",
                table: "products",
                newName: "StockQuantity");

            migrationBuilder.RenameColumn(
                name: "sku",
                schema: "dbo",
                table: "products",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "sellingprice",
                schema: "dbo",
                table: "products",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "dbo",
                table: "products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "lowstockthreshold",
                schema: "dbo",
                table: "products",
                newName: "LowStockThreshold");

            migrationBuilder.RenameColumn(
                name: "isactive",
                schema: "dbo",
                table: "products",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "imageurl",
                schema: "dbo",
                table: "products",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "description",
                schema: "dbo",
                table: "products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdat",
                schema: "dbo",
                table: "products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "costprice",
                schema: "dbo",
                table: "products",
                newName: "CostPrice");

            migrationBuilder.RenameColumn(
                name: "category",
                schema: "dbo",
                table: "products",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "barcode",
                schema: "dbo",
                table: "products",
                newName: "Barcode");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "products",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_products_userid_sku",
                schema: "dbo",
                table: "products",
                newName: "IX_products_UserId_Sku");

            migrationBuilder.RenameIndex(
                name: "IX_products_userid_barcode",
                schema: "dbo",
                table: "products",
                newName: "IX_products_UserId_Barcode");

            migrationBuilder.RenameColumn(
                name: "userid",
                schema: "dbo",
                table: "creditors",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "totalowed",
                schema: "dbo",
                table: "creditors",
                newName: "TotalOwed");

            migrationBuilder.RenameColumn(
                name: "phonenumber",
                schema: "dbo",
                table: "creditors",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "notes",
                schema: "dbo",
                table: "creditors",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "dbo",
                table: "creditors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "dbo",
                table: "creditors",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "creditlimit",
                schema: "dbo",
                table: "creditors",
                newName: "CreditLimit");

            migrationBuilder.RenameColumn(
                name: "createdat",
                schema: "dbo",
                table: "creditors",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "address",
                schema: "dbo",
                table: "creditors",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "creditors",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_creditors_userid",
                schema: "dbo",
                table: "creditors",
                newName: "IX_creditors_UserId");

            migrationBuilder.RenameColumn(
                name: "unitprice",
                schema: "dbo",
                table: "sale_items",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "subtotal",
                schema: "dbo",
                table: "sale_items",
                newName: "Subtotal");

            migrationBuilder.RenameColumn(
                name: "saleid",
                schema: "dbo",
                table: "sale_items",
                newName: "SaleId");

            migrationBuilder.RenameColumn(
                name: "quantity",
                schema: "dbo",
                table: "sale_items",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "productname",
                schema: "dbo",
                table: "sale_items",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "productid",
                schema: "dbo",
                table: "sale_items",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "sale_items",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_saleitems_saleid",
                schema: "dbo",
                table: "sale_items",
                newName: "IX_sale_items_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_saleitems_productid",
                schema: "dbo",
                table: "sale_items",
                newName: "IX_sale_items_ProductId");

            migrationBuilder.RenameColumn(
                name: "saleid",
                schema: "dbo",
                table: "creditor_payments",
                newName: "SaleId");

            migrationBuilder.RenameColumn(
                name: "paymentmethod",
                schema: "dbo",
                table: "creditor_payments",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "paidat",
                schema: "dbo",
                table: "creditor_payments",
                newName: "PaidAt");

            migrationBuilder.RenameColumn(
                name: "notes",
                schema: "dbo",
                table: "creditor_payments",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "creditorid",
                schema: "dbo",
                table: "creditor_payments",
                newName: "CreditorId");

            migrationBuilder.RenameColumn(
                name: "amount",
                schema: "dbo",
                table: "creditor_payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "creditor_payments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_creditorpayments_saleid",
                schema: "dbo",
                table: "creditor_payments",
                newName: "IX_creditor_payments_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_creditorpayments_creditorid",
                schema: "dbo",
                table: "creditor_payments",
                newName: "IX_creditor_payments_CreditorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedat",
                schema: "dbo",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "sales",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedat",
                schema: "dbo",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "creditors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "sale_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "sale_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "creditor_payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "creditor_payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_sale_items",
                schema: "dbo",
                table: "sale_items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_creditor_payments",
                schema: "dbo",
                table: "creditor_payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_creditor_payments_creditors_CreditorId",
                schema: "dbo",
                table: "creditor_payments",
                column: "CreditorId",
                principalSchema: "dbo",
                principalTable: "creditors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_creditor_payments_sales_SaleId",
                schema: "dbo",
                table: "creditor_payments",
                column: "SaleId",
                principalSchema: "dbo",
                principalTable: "sales",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_creditors_users_UserId",
                schema: "dbo",
                table: "creditors",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_users_UserId",
                schema: "dbo",
                table: "products",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sale_items_products_ProductId",
                schema: "dbo",
                table: "sale_items",
                column: "ProductId",
                principalSchema: "dbo",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sale_items_sales_SaleId",
                schema: "dbo",
                table: "sale_items",
                column: "SaleId",
                principalSchema: "dbo",
                principalTable: "sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_creditors_CreditorId",
                schema: "dbo",
                table: "sales",
                column: "CreditorId",
                principalSchema: "dbo",
                principalTable: "creditors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_users_UserId",
                schema: "dbo",
                table: "sales",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
