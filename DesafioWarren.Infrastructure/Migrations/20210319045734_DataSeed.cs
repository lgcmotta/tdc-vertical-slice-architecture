using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DesafioWarren.Infrastructure.Migrations
{
    public partial class DataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts"
                , columns: new[]
                {
                    "Id"
                    , "Name"
                    , "Email"
                    , "PhoneNumber"
                    , "Created"
                    , "LastModified"
                    , "Cpf"
                    , "Number"
                }
                , values: new object[,]
                {
                    {
                        "4bfd6353-4be2-4c96-ab8f-b86aae6770f7"
                        , "luiz motta"
                        , "motta@gmail.com"
                        , "+5551123456789"
                        , DateTime.Now
                        , DateTime.Now
                        , "098.765.432-10"
                        , "446852"
                    }
                    ,
                    {
                        "69e945d4-dfd6-456a-8e85-16c9ff63a136"
                        , "Desafio Warren"
                        , "desafiowarren@luizmotta01hotmail.onmicrosoft.com"
                        , "+5551987654321"
                        , DateTime.Now
                        , DateTime.Now
                        , "123.456.789.10"
                        , "89418489"
                    }
                    ,
                });

            migrationBuilder.InsertData(
                table: "AccountBalance"
                , columns: new[]
                {
                    "Id"
                    , "Balance"
                    , "AccountId"
                    , "LastModified"
                    , "Currency"
                }
                , values: new object[,]
                {
                    {
                        "dfaaacea-3cb7-422d-96ab-8b75274f5ee5"
                        , 0.0000
                        , "4bfd6353-4be2-4c96-ab8f-b86aae6770f7"
                        , DateTime.Now
                        , "BRL"
                    }
                    ,
                    {
                        "aac0fb03-f3d3-44d9-8eeb-759907944fd9"
                        , 0.0000
                        , "69e945d4-dfd6-456a-8e85-16c9ff63a136"
                        , DateTime.Now
                        , "BRL"
                    }
                    ,
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
