using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserIdMadeString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentReactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogReactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad8527e6-5647-4a2c-8a41-375c2afe9941", "AQAAAAIAAYagAAAAEPVYxnf+W3963VjWgpSCZdevgOUpQHE32RH6ZpKEdW9znGtXWxvc05Z2Apk5cpimBg==", "f17f6417-fa66-44ca-8492-65fb15c41c75" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CommentReactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Blogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BlogReactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "14a10ea9-e734-45bd-a8b8-26fb62e832f1", "AQAAAAIAAYagAAAAEI/u+Wl8xg1Xt0ry4hgCaX/NdSHbkEbzrbjgG/LuL2eYhrdZ62tf3BlKzovl6nyB/A==", "ce6eb307-cbfb-4aac-b0a5-0d26a908e464" });
        }
    }
}
