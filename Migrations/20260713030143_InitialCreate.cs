using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pedidos360.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "CATEGORIA_PRODUCTO",
                columns: table => new
                {
                    ID_CATEGORIA_PRODUCTO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIA_PRODUCTO", x => x.ID_CATEGORIA_PRODUCTO);
                });

            migrationBuilder.CreateTable(
                name: "ESTADOS",
                columns: table => new
                {
                    ID_ESTADO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADOS", x => x.ID_ESTADO);
                });

            migrationBuilder.CreateTable(
                name: "PROVINCIA",
                columns: table => new
                {
                    ID_PROVINCIA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE_PROVINCIA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROVINCIA", x => x.ID_PROVINCIA);
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
                name: "CLIENTES",
                columns: table => new
                {
                    CEDULA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ID_ESTADO = table.Column<int>(type: "int", nullable: false),
                    NOMBRE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    APELLIDO_PATERNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    APELLIDO_MATERNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTES", x => x.CEDULA);
                    table.ForeignKey(
                        name: "FK_CLIENTES_ESTADOS_ID_ESTADO",
                        column: x => x.ID_ESTADO,
                        principalTable: "ESTADOS",
                        principalColumn: "ID_ESTADO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTOS",
                columns: table => new
                {
                    ID_PRODUCTO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBRE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ID_CATEGORIA_PRODUCTO = table.Column<int>(type: "int", nullable: false),
                    PRECIO_UNITARIO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IMPUESTO = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    STOCK = table.Column<int>(type: "int", nullable: false),
                    IMAGEN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ID_ESTADO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTOS", x => x.ID_PRODUCTO);
                    table.ForeignKey(
                        name: "FK_PRODUCTOS_CATEGORIA_PRODUCTO_ID_CATEGORIA_PRODUCTO",
                        column: x => x.ID_CATEGORIA_PRODUCTO,
                        principalTable: "CATEGORIA_PRODUCTO",
                        principalColumn: "ID_CATEGORIA_PRODUCTO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTOS_ESTADOS_ID_ESTADO",
                        column: x => x.ID_ESTADO,
                        principalTable: "ESTADOS",
                        principalColumn: "ID_ESTADO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CANTON",
                columns: table => new
                {
                    ID_CANTON = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PROVINCIA = table.Column<int>(type: "int", nullable: false),
                    NOMBRE_CANTON = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTON", x => x.ID_CANTON);
                    table.ForeignKey(
                        name: "FK_CANTON_PROVINCIA_ID_PROVINCIA",
                        column: x => x.ID_PROVINCIA,
                        principalTable: "PROVINCIA",
                        principalColumn: "ID_PROVINCIA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORREOS_CLIENTES",
                columns: table => new
                {
                    CEDULA = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CORREO = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORREOS_CLIENTES", x => new { x.CEDULA, x.CORREO });
                    table.ForeignKey(
                        name: "FK_CORREOS_CLIENTES_CLIENTES_CEDULA",
                        column: x => x.CEDULA,
                        principalTable: "CLIENTES",
                        principalColumn: "CEDULA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDOS",
                columns: table => new
                {
                    ID_PEDIDO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_CLIENTE = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ID_ESTADO = table.Column<int>(type: "int", nullable: false),
                    ID_ROL = table.Column<int>(type: "int", nullable: false),
                    FECHA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SUBTOTAL = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IMPUESTO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DESCUENTO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TOTAL = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDIDOS", x => x.ID_PEDIDO);
                    table.ForeignKey(
                        name: "FK_PEDIDOS_CLIENTES_ID_CLIENTE",
                        column: x => x.ID_CLIENTE,
                        principalTable: "CLIENTES",
                        principalColumn: "CEDULA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PEDIDOS_ESTADOS_ID_ESTADO",
                        column: x => x.ID_ESTADO,
                        principalTable: "ESTADOS",
                        principalColumn: "ID_ESTADO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TELEFONOS_CLIENTES",
                columns: table => new
                {
                    CEDULA = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TELEFONO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TELEFONOS_CLIENTES", x => new { x.CEDULA, x.TELEFONO });
                    table.ForeignKey(
                        name: "FK_TELEFONOS_CLIENTES_CLIENTES_CEDULA",
                        column: x => x.CEDULA,
                        principalTable: "CLIENTES",
                        principalColumn: "CEDULA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DISTRITO",
                columns: table => new
                {
                    ID_DISTRITO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_CANTON = table.Column<int>(type: "int", nullable: false),
                    NOMBRE_DISTRITO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISTRITO", x => x.ID_DISTRITO);
                    table.ForeignKey(
                        name: "FK_DISTRITO_CANTON_ID_CANTON",
                        column: x => x.ID_CANTON,
                        principalTable: "CANTON",
                        principalColumn: "ID_CANTON",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DETALLE_PEDIDO",
                columns: table => new
                {
                    ID_DETALLE_PEDIDO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PEDIDO = table.Column<int>(type: "int", nullable: false),
                    ID_PRODUCTO = table.Column<int>(type: "int", nullable: false),
                    CANTIDAD = table.Column<int>(type: "int", nullable: false),
                    PRECIO_UNITARIO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DESCUENTO = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IMPUESTO_PORCENTAJE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TOTAL_LINEA = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DETALLE_PEDIDO", x => x.ID_DETALLE_PEDIDO);
                    table.ForeignKey(
                        name: "FK_DETALLE_PEDIDO_PEDIDOS_ID_PEDIDO",
                        column: x => x.ID_PEDIDO,
                        principalTable: "PEDIDOS",
                        principalColumn: "ID_PEDIDO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DETALLE_PEDIDO_PRODUCTOS_ID_PRODUCTO",
                        column: x => x.ID_PRODUCTO,
                        principalTable: "PRODUCTOS",
                        principalColumn: "ID_PRODUCTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DIRECCIONES",
                columns: table => new
                {
                    CEDULA = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ID_PROVINCIA = table.Column<int>(type: "int", nullable: false),
                    ID_CANTON = table.Column<int>(type: "int", nullable: false),
                    ID_DISTRITO = table.Column<int>(type: "int", nullable: false),
                    OTRAS_SENAS = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIRECCIONES", x => x.CEDULA);
                    table.ForeignKey(
                        name: "FK_DIRECCIONES_CANTON_ID_CANTON",
                        column: x => x.ID_CANTON,
                        principalTable: "CANTON",
                        principalColumn: "ID_CANTON",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DIRECCIONES_CLIENTES_CEDULA",
                        column: x => x.CEDULA,
                        principalTable: "CLIENTES",
                        principalColumn: "CEDULA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DIRECCIONES_DISTRITO_ID_DISTRITO",
                        column: x => x.ID_DISTRITO,
                        principalTable: "DISTRITO",
                        principalColumn: "ID_DISTRITO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DIRECCIONES_PROVINCIA_ID_PROVINCIA",
                        column: x => x.ID_PROVINCIA,
                        principalTable: "PROVINCIA",
                        principalColumn: "ID_PROVINCIA",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_CANTON_ID_PROVINCIA",
                table: "CANTON",
                column: "ID_PROVINCIA");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_ID_ESTADO",
                table: "CLIENTES",
                column: "ID_ESTADO");

            migrationBuilder.CreateIndex(
                name: "IX_DETALLE_PEDIDO_ID_PEDIDO",
                table: "DETALLE_PEDIDO",
                column: "ID_PEDIDO");

            migrationBuilder.CreateIndex(
                name: "IX_DETALLE_PEDIDO_ID_PRODUCTO",
                table: "DETALLE_PEDIDO",
                column: "ID_PRODUCTO");

            migrationBuilder.CreateIndex(
                name: "IX_DIRECCIONES_ID_CANTON",
                table: "DIRECCIONES",
                column: "ID_CANTON");

            migrationBuilder.CreateIndex(
                name: "IX_DIRECCIONES_ID_DISTRITO",
                table: "DIRECCIONES",
                column: "ID_DISTRITO");

            migrationBuilder.CreateIndex(
                name: "IX_DIRECCIONES_ID_PROVINCIA",
                table: "DIRECCIONES",
                column: "ID_PROVINCIA");

            migrationBuilder.CreateIndex(
                name: "IX_DISTRITO_ID_CANTON",
                table: "DISTRITO",
                column: "ID_CANTON");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDOS_ID_CLIENTE",
                table: "PEDIDOS",
                column: "ID_CLIENTE");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDOS_ID_ESTADO",
                table: "PEDIDOS",
                column: "ID_ESTADO");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTOS_ID_CATEGORIA_PRODUCTO",
                table: "PRODUCTOS",
                column: "ID_CATEGORIA_PRODUCTO");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTOS_ID_ESTADO",
                table: "PRODUCTOS",
                column: "ID_ESTADO");
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
                name: "CORREOS_CLIENTES");

            migrationBuilder.DropTable(
                name: "DETALLE_PEDIDO");

            migrationBuilder.DropTable(
                name: "DIRECCIONES");

            migrationBuilder.DropTable(
                name: "TELEFONOS_CLIENTES");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PEDIDOS");

            migrationBuilder.DropTable(
                name: "PRODUCTOS");

            migrationBuilder.DropTable(
                name: "DISTRITO");

            migrationBuilder.DropTable(
                name: "CLIENTES");

            migrationBuilder.DropTable(
                name: "CATEGORIA_PRODUCTO");

            migrationBuilder.DropTable(
                name: "CANTON");

            migrationBuilder.DropTable(
                name: "ESTADOS");

            migrationBuilder.DropTable(
                name: "PROVINCIA");
        }
    }
}
