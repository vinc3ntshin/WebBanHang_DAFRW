using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanHang_DAFRW.Migrations
{
    public partial class FixMigrationEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "OrderDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalMoney",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                columns: new[] { "ProductId", "OrderCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalMoney",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                columns: new[] { "ProductId", "UserId", "OrderCode" });
        }
    }
}
