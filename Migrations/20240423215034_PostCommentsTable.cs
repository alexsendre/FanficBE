using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficBE.Migrations
{
    public partial class PostCommentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Comments_CommentId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CommentId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "CommentPost",
                columns: table => new
                {
                    CommentsId = table.Column<int>(type: "integer", nullable: false),
                    PostsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPost", x => new { x.CommentsId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CommentPost_Comments_CommentsId",
                        column: x => x.CommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "Content", "PostId" },
                values: new object[,]
                {
                    { 1, 4, "this is a comment!", 1 },
                    { 2, 3, "comment 2!", 2 },
                    { 3, 2, "a new comment, totally new", 3 },
                    { 4, 1, "this is also a comment", 1 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 50, 34, 775, DateTimeKind.Local).AddTicks(1602));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 50, 34, 775, DateTimeKind.Local).AddTicks(1652));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 50, 34, 775, DateTimeKind.Local).AddTicks(1655));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 50, 34, 775, DateTimeKind.Local).AddTicks(1657));

            migrationBuilder.CreateIndex(
                name: "IX_CommentPost_PostsId",
                table: "CommentPost",
                column: "PostsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentPost");

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Posts",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 47, 35, 346, DateTimeKind.Local).AddTicks(1045));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 47, 35, 346, DateTimeKind.Local).AddTicks(1088));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 47, 35, 346, DateTimeKind.Local).AddTicks(1091));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 23, 16, 47, 35, 346, DateTimeKind.Local).AddTicks(1093));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CommentId",
                table: "Posts",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Comments_CommentId",
                table: "Posts",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
