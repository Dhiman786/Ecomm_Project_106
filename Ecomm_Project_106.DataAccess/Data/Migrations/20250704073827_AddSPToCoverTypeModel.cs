using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_Project_106.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSPToCoverTypeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //SP CoverType
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_UpdateCoverType 
                                         @id int,
                                        @name varchar(50)
                                            AS
                                    Update CoverTypes set name=@name where id=@id");

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CreateCoverType 
                                        @name varchar(50)
                                            AS
                                    Insert CoverTypes values(@name)");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_DeleteCoverType 
                                        @id int
                                         AS
                                    delete from CoverTypes where id=@id");

            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetCoverTypes 
                                        AS
                                    Select *from CoverTypes");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_GetCoverType
                                       @id int
                                        AS
                                    Select * from CoverTypes where id=@id");


            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
