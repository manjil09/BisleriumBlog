using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3e4007fd-f323-41c5-892a-29e4c50d0f6b", "3e4007fd-f323-41c5-892a-29e4c50d0f6b", "Admin", "ADMIN" },
                    { "f8ca833b-8b85-41b3-9279-eb99b563326d", "f8ca833b-8b85-41b3-9279-eb99b563326d", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "36109bb5-c596-4fee-a016-1d8f8c7496cd", 0, "14a10ea9-e734-45bd-a8b8-26fb62e832f1", "manjil.koju.a@gmail.com", true, false, null, null, "SPADMIN", "AQAAAAIAAYagAAAAEI/u+Wl8xg1Xt0ry4hgCaX/NdSHbkEbzrbjgG/LuL2eYhrdZ62tf3BlKzovl6nyB/A==", null, false, "ce6eb307-cbfb-4aac-b0a5-0d26a908e464", false, "SpAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3e4007fd-f323-41c5-892a-29e4c50d0f6b", "36109bb5-c596-4fee-a016-1d8f8c7496cd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8ca833b-8b85-41b3-9279-eb99b563326d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3e4007fd-f323-41c5-892a-29e4c50d0f6b", "36109bb5-c596-4fee-a016-1d8f8c7496cd" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e4007fd-f323-41c5-892a-29e4c50d0f6b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd");
        }
    }
}
