using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cuahangsua.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserMessage = table.Column<string>(nullable: false),
                    BotResponse = table.Column<string>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ChatMessages");
        }
    }
}
