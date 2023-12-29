using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanHang_DAFRW.Migrations
{
    public partial class FixMigration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalMoney",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "OrderDetails");
        }
    }
}
