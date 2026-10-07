using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryTrafficManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LastSequenceNumber",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSequenceNumber",
                table: "Vehicles");
        }
    }
}
