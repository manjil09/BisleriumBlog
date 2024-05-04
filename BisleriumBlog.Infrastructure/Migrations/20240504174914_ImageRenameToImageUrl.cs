using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageRenameToImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Blogs",
                newName: "ImageUrl");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b99673ca-3bc7-4751-9cce-fa0a96673d1c", "AQAAAAIAAYagAAAAEAueFw3pEdHF9OnIInn7qubBsWJ/SBW/62+D0fS9gJLf8jOBKrWEc2mssm5txi7Zfg==", "baa6bfb1-aa82-4b04-9666-089dde48df09" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Blogs",
                newName: "Image");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8b8909a-1f6e-49cf-b836-4ae5ce66529f", "AQAAAAIAAYagAAAAEK4JE9Rn68BgRl3Qvwotigq5gEe41TZCWdZuPdeANU73RagfUvc6y1w92Eqy40UEhg==", "904b65ce-19bd-4fe5-ba70-c055fbefe0c9" });
        }
    }
}
