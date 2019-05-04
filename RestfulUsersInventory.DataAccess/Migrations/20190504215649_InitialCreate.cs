using Microsoft.EntityFrameworkCore.Migrations;

namespace RestfulUsersInventory.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ItemTypeId = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(type: "Decimal(18,0)", nullable: false),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.UniqueConstraint("AK_Items_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ItemId",
                table: "UserItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_UserId",
                table: "UserItems",
                column: "UserId");
            migrationBuilder.Sql(
            @"
                INSERT INTO [ItemTypes] ([Id], [Name]) VALUES (1, 'Sword');
                INSERT INTO [ItemTypes] ([Id], [Name]) VALUES (2, 'Battery');
                INSERT INTO [ItemTypes] ([Id], [Name]) VALUES (3, 'Key');
            ");
            migrationBuilder.Sql(
            @"
                INSERT INTO [Items] ([Id], [Name], [ItemTypeId], [Value], [Weight]) VALUES (1, 'Longsword', 1, 100, 10);
                INSERT INTO [Items] ([Id], [Name], [ItemTypeId], [Value], [Weight]) VALUES (2, 'Claymore', 1, 150, 15);
                INSERT INTO [Items] ([Id], [Name], [ItemTypeId], [Value], [Weight]) VALUES (3, 'Dagger', 1, 50, 5);
            ");
            migrationBuilder.Sql(
            @"
                INSERT INTO [Users] ([Id], [Name]) VALUES (1, 'Rick Sanchez');
                INSERT INTO [Users] ([Id], [Name]) VALUES (2, 'Morty Smith');
            ");
            for (int swords = 1; swords < 47; swords++)
            {
                migrationBuilder.Sql($"INSERT INTO [UserItems] ([Id], [UserId], [ItemId]) VALUES({swords}, 1, 1)");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ItemTypes");
        }
    }
}
