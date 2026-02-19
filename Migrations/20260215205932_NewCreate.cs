using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Insurance.Migrations
{
    /// <inheritdoc />
    public partial class NewCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyTotalDescriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolicyTotalDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EMI = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MedicalId = table.Column<int>(type: "int", nullable: true),
                    PolicyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PolicyDesc = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PolicyDurationMonths = table.Column<int>(type: "int", nullable: true),
                    PolicyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyTotalDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyTotalDescriptions_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "PolicyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyTotalDescriptions_PolicyId",
                table: "PolicyTotalDescriptions",
                column: "PolicyId");
        }
    }
}
