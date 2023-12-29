using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanHang_DAFRW.Migrations
{
    public partial class OrderDTtesst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
