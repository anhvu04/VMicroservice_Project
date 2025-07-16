using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUsernamePasswodCustomerSegment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerSegments_UserName",
                table: "CustomerSegments");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "CustomerSegments");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CustomerSegments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "CustomerSegments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CustomerSegments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSegments_UserName",
                table: "CustomerSegments",
                column: "UserName",
                unique: true);
        }
    }
}
