using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiemensInternship.Migrations
{
    /// <inheritdoc />
    public partial class LoanUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoadDate",
                table: "Loans",
                newName: "LoanDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanDate",
                table: "Loans",
                newName: "LoadDate");
        }
    }
}
