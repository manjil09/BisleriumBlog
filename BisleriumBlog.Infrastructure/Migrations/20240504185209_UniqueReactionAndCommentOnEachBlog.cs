using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueReactionAndCommentOnEachBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommentReactions_CommentId",
                table: "CommentReactions");

            migrationBuilder.DropIndex(
                name: "IX_BlogReactions_BlogId",
                table: "BlogReactions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentReactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogReactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "741dc879-e2b8-4178-ae47-7b051c038e13", "AQAAAAIAAYagAAAAEK8L6Cr30tM3sqXS49v1dQN2Yuo942nLYRAXEnMrglPdMNKn3JmBavfcPIV39nJQoQ==", "764e38d8-1133-4cd2-8aaf-40517fef8348" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_CommentId_UserId",
                table: "CommentReactions",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogReactions_BlogId_UserId",
                table: "BlogReactions",
                columns: new[] { "BlogId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommentReactions_CommentId_UserId",
                table: "CommentReactions");

            migrationBuilder.DropIndex(
                name: "IX_BlogReactions_BlogId_UserId",
                table: "BlogReactions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentReactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogReactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c5f106c-9d02-41b9-b5cd-4d457b36cc60", "AQAAAAIAAYagAAAAEJplz+W+FmdYXdainQdNvluZXlu8TCt3JTZIlCtUTwJzGUav/Cjnay7yORinCfhnrQ==", "ca36c1a9-fe3a-41b8-8138-390022fa0c2b" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_CommentId",
                table: "CommentReactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogReactions_BlogId",
                table: "BlogReactions",
                column: "BlogId");
        }
    }
}
