using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserUniqueCommentRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogId_UserId",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ef91db41-0a1a-4d82-b80b-e3af23df727d", "AQAAAAIAAYagAAAAEFjKhN4zuFrZrj9xxjPPmb0ad9vE8TzpNkPS36IPkcwTrYVu7lDAkmqGB1N1lNbRUQ==", "f89e1da3-7cde-494a-a714-28da97335632" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogId",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e0c4954-fc14-4a92-a81b-6b12d147d249", "AQAAAAIAAYagAAAAEEgE8rUjQn4XN6pDhYFH8qkQ2SQZWMONa+k3g1PehxXUHiUN8pAFcKMcnvBcBvDyew==", "72590618-2cbe-4a8b-953e-46d9ff0002c4" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId_UserId",
                table: "Comments",
                columns: new[] { "BlogId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
