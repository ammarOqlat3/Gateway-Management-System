using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetWayPro.Migrations
{
    public partial class AddImgPathAndUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "AddProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AddProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "AddProduct");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AddProduct");
        }
    }
}
