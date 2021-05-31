using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NobetTakip.WebAPI.Migrations
{
    public partial class TestMigrationv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayNight",
                table: "Nobet");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Nobet");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonelId",
                table: "Bildirim",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Degisim_FirstNobetId",
                table: "Degisim",
                column: "FirstNobetId");

            migrationBuilder.CreateIndex(
                name: "IX_Degisim_FirstPersonelId",
                table: "Degisim",
                column: "FirstPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Degisim_SecondNobetId",
                table: "Degisim",
                column: "SecondNobetId");

            migrationBuilder.CreateIndex(
                name: "IX_Degisim_SecondPersonelId",
                table: "Degisim",
                column: "SecondPersonelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Degisim_Nobet_FirstNobetId",
                table: "Degisim",
                column: "FirstNobetId",
                principalTable: "Nobet",
                principalColumn: "NobetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Degisim_Personel_FirstPersonelId",
                table: "Degisim",
                column: "FirstPersonelId",
                principalTable: "Personel",
                principalColumn: "PersonelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Degisim_Nobet_SecondNobetId",
                table: "Degisim",
                column: "SecondNobetId",
                principalTable: "Nobet",
                principalColumn: "NobetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Degisim_Personel_SecondPersonelId",
                table: "Degisim",
                column: "SecondPersonelId",
                principalTable: "Personel",
                principalColumn: "PersonelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Degisim_Nobet_FirstNobetId",
                table: "Degisim");

            migrationBuilder.DropForeignKey(
                name: "FK_Degisim_Personel_FirstPersonelId",
                table: "Degisim");

            migrationBuilder.DropForeignKey(
                name: "FK_Degisim_Nobet_SecondNobetId",
                table: "Degisim");

            migrationBuilder.DropForeignKey(
                name: "FK_Degisim_Personel_SecondPersonelId",
                table: "Degisim");

            migrationBuilder.DropIndex(
                name: "IX_Degisim_FirstNobetId",
                table: "Degisim");

            migrationBuilder.DropIndex(
                name: "IX_Degisim_FirstPersonelId",
                table: "Degisim");

            migrationBuilder.DropIndex(
                name: "IX_Degisim_SecondNobetId",
                table: "Degisim");

            migrationBuilder.DropIndex(
                name: "IX_Degisim_SecondPersonelId",
                table: "Degisim");

            migrationBuilder.DropColumn(
                name: "PersonelId",
                table: "Bildirim");

            migrationBuilder.AddColumn<int>(
                name: "DayNight",
                table: "Nobet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Nobet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
