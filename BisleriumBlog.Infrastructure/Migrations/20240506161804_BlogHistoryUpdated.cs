using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BlogHistoryUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "BlogHistory",
                newName: "ImageUrl");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57aa7ed1-f2ec-47df-b604-e4299ff3383c", "AQAAAAIAAYagAAAAEDcqGPycLDdqXVi8PvRtKDpn3Wcp4SsPnYxQ9uFtM/9cdMlt1ZxUKc+MjO8+yyyDRw==", "8dc5dbe6-8018-405d-97ec-29e6ab8407a5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "BlogHistory",
                newName: "Image");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ff91bc3-f998-4f96-b726-d06ee51d1d10", "AQAAAAIAAYagAAAAEF4wTzAs+TSpQfd5FWhDU40ZYsYf+0JqOMMIxhofz+TK8lFux7TOgyxTTtUAGE/dJQ==", "46426573-9702-4bfd-a0c6-5ffb9d6d4bfe" });
        }
    }
}
