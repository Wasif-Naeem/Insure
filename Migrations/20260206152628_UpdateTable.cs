using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Insurance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovalId",
                table: "PolicyApprovalDetails",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PolicyApprovalDetails",
                newName: "ApprovalId");
        }
    }
}
