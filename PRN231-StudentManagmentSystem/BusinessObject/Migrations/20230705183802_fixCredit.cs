using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    public partial class fixCredit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "Subjects",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "Students",
                type: "int",
                nullable: true);
        }
    }
}
