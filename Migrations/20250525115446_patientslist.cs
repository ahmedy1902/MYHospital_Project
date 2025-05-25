using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNet_System.Migrations
{
    /// <inheritdoc />
    public partial class patientslist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Patients_patient_id",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Patients_followUp_doctorID",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "treatment",
                table: "Patients",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Patients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "manager",
                table: "Departments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "patient_id",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_followUp_doctorID",
                table: "Patients",
                column: "followUp_doctorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Patients_patient_id",
                table: "Bills",
                column: "patient_id",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Patients_patient_id",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Patients_followUp_doctorID",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "treatment",
                table: "Patients",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "manager",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "patient_id",
                table: "Bills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_followUp_doctorID",
                table: "Patients",
                column: "followUp_doctorID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Patients_patient_id",
                table: "Bills",
                column: "patient_id",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
