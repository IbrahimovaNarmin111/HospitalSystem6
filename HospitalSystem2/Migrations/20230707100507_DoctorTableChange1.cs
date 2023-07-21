using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalSystem2.Migrations
{
    public partial class DoctorTableChange1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "Doctors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
