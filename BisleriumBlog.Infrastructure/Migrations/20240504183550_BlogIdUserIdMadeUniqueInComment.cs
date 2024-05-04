using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BlogIdUserIdMadeUniqueInComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c5f106c-9d02-41b9-b5cd-4d457b36cc60", "AQAAAAIAAYagAAAAEJplz+W+FmdYXdainQdNvluZXlu8TCt3JTZIlCtUTwJzGUav/Cjnay7yORinCfhnrQ==", "ca36c1a9-fe3a-41b8-8138-390022fa0c2b" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId_UserId",
                table: "Comments",
                columns: new[] { "BlogId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogId_UserId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b99673ca-3bc7-4751-9cce-fa0a96673d1c", "AQAAAAIAAYagAAAAEAueFw3pEdHF9OnIInn7qubBsWJ/SBW/62+D0fS9gJLf8jOBKrWEc2mssm5txi7Zfg==", "baa6bfb1-aa82-4b04-9666-089dde48df09" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");
        }
    }
}
