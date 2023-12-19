using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DockerDBMigrator.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "opportunities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opportunities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fullname = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "opportunitiesWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    money = table.Column<double>(type: "double precision", nullable: false),
                    OpportunityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opportunitiesWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_opportunitiesWallets_opportunities_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invesments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvestorId = table.Column<int>(type: "integer", nullable: false),
                    OppertunityId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invesments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invesments_opportunities_OppertunityId",
                        column: x => x.OppertunityId,
                        principalTable: "opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invesments_users_InvestorId",
                        column: x => x.InvestorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestorsWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    money = table.Column<double>(type: "double precision", nullable: false),
                    InvestorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestorsWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestorsWallets_users_InvestorId",
                        column: x => x.InvestorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvesmentsTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvestorWalletId = table.Column<int>(type: "integer", nullable: false),
                    OpportunityWalletId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvesmentsTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvesmentsTransactions_InvestorsWallets_InvestorWalletId",
                        column: x => x.InvestorWalletId,
                        principalTable: "InvestorsWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvesmentsTransactions_opportunitiesWallets_OpportunityWall~",
                        column: x => x.OpportunityWalletId,
                        principalTable: "opportunitiesWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_invesments_InvestorId",
                table: "invesments",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_invesments_OppertunityId_InvestorId",
                table: "invesments",
                columns: new[] { "OppertunityId", "InvestorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvesmentsTransactions_InvestorWalletId",
                table: "InvesmentsTransactions",
                column: "InvestorWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_InvesmentsTransactions_OpportunityWalletId",
                table: "InvesmentsTransactions",
                column: "OpportunityWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorsWallets_InvestorId",
                table: "InvestorsWallets",
                column: "InvestorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_opportunitiesWallets_OpportunityId",
                table: "opportunitiesWallets",
                column: "OpportunityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invesments");

            migrationBuilder.DropTable(
                name: "InvesmentsTransactions");

            migrationBuilder.DropTable(
                name: "InvestorsWallets");

            migrationBuilder.DropTable(
                name: "opportunitiesWallets");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "opportunities");
        }
    }
}
