using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalSystem2.Migrations
{
    public partial class AddIsDeactiveColumnToCostsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Costs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Costs");
        }
    }
}
