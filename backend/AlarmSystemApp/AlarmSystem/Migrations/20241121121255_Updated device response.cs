using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlarmSystem.Migrations
{
    /// <inheritdoc />
    public partial class Updateddeviceresponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MotionDetected",
                table: "DeviceResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotionDetected",
                table: "DeviceResponses");
        }
    }
}
