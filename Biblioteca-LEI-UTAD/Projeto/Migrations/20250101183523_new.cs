using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabalho.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administradores",
                columns: table => new
                {
                    Username_Admin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Administ__266ED3E6575F5872", x => x.Username_Admin);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Autenticado",
                columns: table => new
                {
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Contacto2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Contacto3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Nome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Autentic__536C85E503316A59", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Autor",
                columns: table => new
                {
                    ID_Autor = table.Column<int>(type: "int", nullable: false),
                    Name_Autor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Autor__9626AD264DB6FFA5", x => x.ID_Autor);
                });

            migrationBuilder.CreateTable(
                name: "Biblioteca",
                columns: table => new
                {
                    ID_Biblioteca = table.Column<int>(type: "int", nullable: false),
                    HorarioI = table.Column<TimeOnly>(type: "time", nullable: false),
                    HorarioF = table.Column<TimeOnly>(type: "time", nullable: false),
                    Localizacao = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Nome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Telefone = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bibliote__906E4B4CC3FA4858", x => x.ID_Biblioteca);
                });

            migrationBuilder.CreateTable(
                name: "MotivoBloq",
                columns: table => new
                {
                    ID_MotivoBloq = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MotivoBl__FD6AC615A6A67AE0", x => x.ID_MotivoBloq);
                });

            migrationBuilder.CreateTable(
                name: "Cria_Admin",
                columns: table => new
                {
                    Username_Admin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Username_NovoAdmin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__Cria_Admi__Usern__693CA210",
                        column: x => x.Username_Admin,
                        principalTable: "Administradores",
                        principalColumn: "Username_Admin");
                    table.ForeignKey(
                        name: "FK__Cria_Admi__Usern__6A30C649",
                        column: x => x.Username_NovoAdmin,
                        principalTable: "Administradores",
                        principalColumn: "Username_Admin");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bibliotecario",
                columns: table => new
                {
                    Username_Bib = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bibliote__1E4CD8D2F5865935", x => x.Username_Bib);
                    table.ForeignKey(
                        name: "FK__Bibliotec__Usern__3C69FB99",
                        column: x => x.Username_Bib,
                        principalTable: "Autenticado",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Leitores",
                columns: table => new
                {
                    Username_Lei = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Morada = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Leitores__15E92569CF5B3BB7", x => x.Username_Lei);
                    table.ForeignKey(
                        name: "FK__Leitores__Userna__3F466844",
                        column: x => x.Username_Lei,
                        principalTable: "Autenticado",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Bloquear",
                columns: table => new
                {
                    ID_Bloqueio = table.Column<int>(type: "int", nullable: false),
                    ID_MotivoBloq = table.Column<int>(type: "int", nullable: false),
                    Username_Admin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Data_Bloqueio = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "(getdate())"),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bloquear__6267F0075C931E99", x => x.ID_Bloqueio);
                    table.ForeignKey(
                        name: "FK__Bloquear__ID_Mot__4BAC3F29",
                        column: x => x.ID_MotivoBloq,
                        principalTable: "MotivoBloq",
                        principalColumn: "ID_MotivoBloq");
                    table.ForeignKey(
                        name: "FK__Bloquear__Userna__49C3F6B7",
                        column: x => x.Username_Admin,
                        principalTable: "Administradores",
                        principalColumn: "Username_Admin");
                    table.ForeignKey(
                        name: "FK__Bloquear__Userna__4AB81AF0",
                        column: x => x.Username,
                        principalTable: "Autenticado",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    ISBN = table.Column<long>(type: "bigint", nullable: false),
                    Preco = table.Column<decimal>(type: "money", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    N_Exemplares = table.Column<int>(type: "int", nullable: false),
                    Genero = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Username_BibAdd = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ID_Autor = table.Column<int>(type: "int", nullable: false),
                    ID_Biblioteca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Livro__447D36EB33DDD9E3", x => x.ISBN);
                    table.ForeignKey(
                        name: "FK__Livro__ID_Autor__5DCAEF64",
                        column: x => x.ID_Autor,
                        principalTable: "Autor",
                        principalColumn: "ID_Autor");
                    table.ForeignKey(
                        name: "FK__Livro__ID_Biblio__5EBF139D",
                        column: x => x.ID_Biblioteca,
                        principalTable: "Biblioteca",
                        principalColumn: "ID_Biblioteca");
                    table.ForeignKey(
                        name: "FK__Livro__Username___5CD6CB2B",
                        column: x => x.Username_BibAdd,
                        principalTable: "Bibliotecario",
                        principalColumn: "Username_Bib");
                });

            migrationBuilder.CreateTable(
                name: "ValidarRegisto",
                columns: table => new
                {
                    ID_Valid = table.Column<int>(type: "int", nullable: false),
                    Username_Admin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Username_Bib = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Data_Valid = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Validado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ValidarR__7DA04BE5E1DF26D3", x => x.ID_Valid);
                    table.ForeignKey(
                        name: "FK__ValidarRe__Usern__5070F446",
                        column: x => x.Username_Admin,
                        principalTable: "Administradores",
                        principalColumn: "Username_Admin");
                    table.ForeignKey(
                        name: "FK__ValidarRe__Usern__5165187F",
                        column: x => x.Username_Bib,
                        principalTable: "Bibliotecario",
                        principalColumn: "Username_Bib");
                });

            migrationBuilder.CreateTable(
                name: "Requisicao",
                columns: table => new
                {
                    ID_Requisicao = table.Column<int>(type: "int", nullable: false),
                    Username_Bib = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Username_Lei = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ISBN = table.Column<long>(type: "bigint", nullable: false),
                    Data_Requisicao = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "(getdate())"),
                    Data_Devolucao = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Requisic__7B7D23BB3CB5D882", x => x.ID_Requisicao);
                    table.ForeignKey(
                        name: "FK__Requisica__Usern__656C112C",
                        column: x => x.Username_Bib,
                        principalTable: "Bibliotecario",
                        principalColumn: "Username_Bib");
                    table.ForeignKey(
                        name: "FK__Requisica__Usern__66603565",
                        column: x => x.Username_Lei,
                        principalTable: "Leitores",
                        principalColumn: "Username_Lei");
                    table.ForeignKey(
                        name: "FK__Requisicao__ISBN__6754599E",
                        column: x => x.ISBN,
                        principalTable: "Livro",
                        principalColumn: "ISBN");
                });

            migrationBuilder.CreateTable(
                name: "EntregasAtraso",
                columns: table => new
                {
                    ID_Entregas = table.Column<int>(type: "int", nullable: false),
                    ID_Requisicao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Entregas__0FB6D86D817DB75A", x => x.ID_Entregas);
                    table.ForeignKey(
                        name: "FK__EntregasA__ID_Re__6E01572D",
                        column: x => x.ID_Requisicao,
                        principalTable: "Requisicao",
                        principalColumn: "ID_Requisicao");
                });

            migrationBuilder.CreateTable(
                name: "HistoricoRequ",
                columns: table => new
                {
                    ID_Historico = table.Column<int>(type: "int", nullable: false),
                    ID_Requisicao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Historic__ECA88795D04FBAF5", x => x.ID_Historico);
                    table.ForeignKey(
                        name: "FK__Historico__ID_Re__71D1E811",
                        column: x => x.ID_Requisicao,
                        principalTable: "Requisicao",
                        principalColumn: "ID_Requisicao");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bloquear_ID_MotivoBloq",
                table: "Bloquear",
                column: "ID_MotivoBloq");

            migrationBuilder.CreateIndex(
                name: "IX_Bloquear_Username",
                table: "Bloquear",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Bloquear_Username_Admin",
                table: "Bloquear",
                column: "Username_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_Cria_Admin_Username_Admin",
                table: "Cria_Admin",
                column: "Username_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_Cria_Admin_Username_NovoAdmin",
                table: "Cria_Admin",
                column: "Username_NovoAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_EntregasAtraso_ID_Requisicao",
                table: "EntregasAtraso",
                column: "ID_Requisicao");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoRequ_ID_Requisicao",
                table: "HistoricoRequ",
                column: "ID_Requisicao");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_ID_Autor",
                table: "Livro",
                column: "ID_Autor");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_ID_Biblioteca",
                table: "Livro",
                column: "ID_Biblioteca");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Username_BibAdd",
                table: "Livro",
                column: "Username_BibAdd");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicao_ISBN",
                table: "Requisicao",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicao_Username_Bib",
                table: "Requisicao",
                column: "Username_Bib");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicao_Username_Lei",
                table: "Requisicao",
                column: "Username_Lei");

            migrationBuilder.CreateIndex(
                name: "IX_ValidarRegisto_Username_Admin",
                table: "ValidarRegisto",
                column: "Username_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_ValidarRegisto_Username_Bib",
                table: "ValidarRegisto",
                column: "Username_Bib");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Bloquear");

            migrationBuilder.DropTable(
                name: "Cria_Admin");

            migrationBuilder.DropTable(
                name: "EntregasAtraso");

            migrationBuilder.DropTable(
                name: "HistoricoRequ");

            migrationBuilder.DropTable(
                name: "ValidarRegisto");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MotivoBloq");

            migrationBuilder.DropTable(
                name: "Requisicao");

            migrationBuilder.DropTable(
                name: "Administradores");

            migrationBuilder.DropTable(
                name: "Leitores");

            migrationBuilder.DropTable(
                name: "Livro");

            migrationBuilder.DropTable(
                name: "Autor");

            migrationBuilder.DropTable(
                name: "Biblioteca");

            migrationBuilder.DropTable(
                name: "Bibliotecario");

            migrationBuilder.DropTable(
                name: "Autenticado");
        }
    }
}
