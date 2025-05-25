using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNet_System.Migrations
{
    /// <inheritdoc />
    public partial class patientslist1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdmissionDate",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Patients",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionDate",
                table: "Patients");
           
            migrationBuilder.DropColumn(
                name: "status",
                table: "Patients");
        }
    }
}
