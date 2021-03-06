﻿// <auto-generated />
using System;
using AppApuntesNet5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AppApuntesNet5.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210121180135_EjemploMigracion")]
    partial class EjemploMigracion
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("AppApuntesNet5.Dto.UsuarioAutenticado", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.ApuntesCategoria", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("titulo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("titulo");

                    b.HasKey("id");

                    b.ToTable("apuntes_categoria");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.ApuntesDetalleTema", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("apuntesTemaId")
                        .HasColumnType("int")
                        .HasColumnName("tema_id");

                    b.Property<string>("contenido")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("contenido");

                    b.Property<string>("rutaFoto")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("ruta_foto");

                    b.Property<string>("titulo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("titulo");

                    b.HasKey("id");

                    b.HasIndex("apuntesTemaId");

                    b.ToTable("apuntes_detalle_tema");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.ApuntesTema", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("apuntesCategoriaId")
                        .HasColumnType("int")
                        .HasColumnName("categoria_id");

                    b.Property<string>("titulo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("titulo");

                    b.HasKey("id");

                    b.HasIndex("apuntesCategoriaId");

                    b.ToTable("apuntes_tema");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.Cargo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool>("activo")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("activo");

                    b.Property<string>("authority")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("authority");

                    b.Property<string>("descripcion")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("descripcion");

                    b.Property<bool>("visible")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("visible");

                    b.HasKey("id");

                    b.ToTable("cargo");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.Usuario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool>("activo")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("activo");

                    b.Property<string>("apellidoMaterno")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("apellido_materno");

                    b.Property<string>("apellidoPaterno")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("apellido_paterno");

                    b.Property<string>("clavePdt")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("clave_pdt");

                    b.Property<int>("largoPassword")
                        .HasColumnType("int")
                        .HasColumnName("largo_password");

                    b.Property<string>("nombre")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("nombre");

                    b.Property<string>("password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("password");

                    b.Property<string>("rut")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("rut");

                    b.Property<string>("telefono")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("telefono");

                    b.Property<string>("username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasColumnName("username");

                    b.Property<bool>("visible")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("visible");

                    b.HasKey("id");

                    b.ToTable("usuario");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.UsuarioCargo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("cargoId")
                        .HasColumnType("int")
                        .HasColumnName("cargo_id");

                    b.Property<int>("usuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.HasKey("id");

                    b.HasIndex("cargoId");

                    b.HasIndex("usuarioId");

                    b.ToTable("usuario_cargo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(95) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.ApuntesDetalleTema", b =>
                {
                    b.HasOne("AppApuntesNet5.Models.ApuntesTema", "apuntesTema")
                        .WithMany("listaApuntesDetalleTema")
                        .HasForeignKey("apuntesTemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("apuntesTema");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.ApuntesTema", b =>
                {
                    b.HasOne("AppApuntesNet5.Models.ApuntesCategoria", "apuntesCategoria")
                        .WithMany("listaApuntesTema")
                        .HasForeignKey("apuntesCategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("apuntesCategoria");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.UsuarioCargo", b =>
                {
                    b.HasOne("AppApuntesNet5.Models.Cargo", "cargo")
                        .WithMany("listaUsuarioCargo")
                        .HasForeignKey("cargoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppApuntesNet5.Models.Usuario", "usuario")
                        .WithMany("listaUsuarioCargo")
                        .HasForeignKey("usuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cargo");

                    b.Navigation("usuario");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AppApuntesNet5.Dto.UsuarioAutenticado", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AppApuntesNet5.Dto.UsuarioAutenticado", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppApuntesNet5.Dto.UsuarioAutenticado", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AppApuntesNet5.Dto.UsuarioAutenticado", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppApuntesNet5.Models.ApuntesCategoria", b =>
                {
                    b.Navigation("listaApuntesTema");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.ApuntesTema", b =>
                {
                    b.Navigation("listaApuntesDetalleTema");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.Cargo", b =>
                {
                    b.Navigation("listaUsuarioCargo");
                });

            modelBuilder.Entity("AppApuntesNet5.Models.Usuario", b =>
                {
                    b.Navigation("listaUsuarioCargo");
                });
#pragma warning restore 612, 618
        }
    }
}
