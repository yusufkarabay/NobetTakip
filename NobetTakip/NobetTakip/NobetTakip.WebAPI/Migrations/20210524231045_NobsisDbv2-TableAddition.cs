using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NobetTakip.WebAPI.Migrations
{
    public partial class NobsisDbv2TableAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bildirim",
                columns: table => new
                {
                    BildirimId = table.Column<Guid>(nullable: false),
                    DegisimId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bildirim", x => x.BildirimId);
                });

            migrationBuilder.CreateTable(
                name: "Degisim",
                columns: table => new
                {
                    DegisimId = table.Column<Guid>(nullable: false),
                    IsletmeId = table.Column<Guid>(nullable: false),
                    FirstNobetId = table.Column<Guid>(nullable: false),
                    SecondNobetId = table.Column<Guid>(nullable: false),
                    FirstPersonelId = table.Column<Guid>(nullable: false),
                    SecondPersonelId = table.Column<Guid>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degisim", x => x.DegisimId);
                });

            migrationBuilder.CreateTable(
                name: "Isletme",
                columns: table => new
                {
                    IsletmeId = table.Column<Guid>(nullable: false),
                    IsletmeAdi = table.Column<string>(nullable: true),
                    IsletmeKod = table.Column<string>(nullable: true),
                    MailAddress = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Isletme", x => x.IsletmeId);
                });

            migrationBuilder.CreateTable(
                name: "Nobet",
                columns: table => new
                {
                    NobetId = table.Column<Guid>(nullable: false),
                    IsletmeId = table.Column<Guid>(nullable: false),
                    PersonelIds = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Period = table.Column<int>(nullable: false),
                    DayNight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nobet", x => x.NobetId);
                    table.ForeignKey(
                        name: "FK_Nobet_Isletme_IsletmeId",
                        column: x => x.IsletmeId,
                        principalTable: "Isletme",
                        principalColumn: "IsletmeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personel",
                columns: table => new
                {
                    PersonelId = table.Column<Guid>(nullable: false),
                    IsletmeId = table.Column<Guid>(nullable: false),
                    RealName = table.Column<string>(nullable: true),
                    MailAddress = table.Column<string>(nullable: true),
                    GSMNo = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personel", x => x.PersonelId);
                    table.ForeignKey(
                        name: "FK_Personel_Isletme_IsletmeId",
                        column: x => x.IsletmeId,
                        principalTable: "Isletme",
                        principalColumn: "IsletmeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Isletme_IsletmeKod",
                table: "Isletme",
                column: "IsletmeKod",
                unique: true,
                filter: "[IsletmeKod] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nobet_IsletmeId",
                table: "Nobet",
                column: "IsletmeId");

            migrationBuilder.CreateIndex(
                name: "IX_Personel_IsletmeId",
                table: "Personel",
                column: "IsletmeId");

            migrationBuilder.CreateIndex(
                name: "IX_Personel_MailAddress",
                table: "Personel",
                column: "MailAddress",
                unique: true,
                filter: "[MailAddress] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bildirim");

            migrationBuilder.DropTable(
                name: "Degisim");

            migrationBuilder.DropTable(
                name: "Nobet");

            migrationBuilder.DropTable(
                name: "Personel");

            migrationBuilder.DropTable(
                name: "Isletme");
        }
    }
}
