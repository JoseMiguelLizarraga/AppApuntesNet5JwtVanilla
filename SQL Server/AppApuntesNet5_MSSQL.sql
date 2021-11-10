
DROP DATABASE IF EXISTS AppApuntesNet5;
CREATE DATABASE AppApuntesNet5;
GO
USE AppApuntesNet5
GO

-- Apuntes

CREATE TABLE apuntes_categoria(
	id int IDENTITY NOT NULL,
	titulo VARCHAR(50) NOT NULL,
	CONSTRAINT PK_apuntes_categoria PRIMARY KEY(id)
);

CREATE TABLE apuntes_tema(
	id int IDENTITY NOT NULL,
	titulo VARCHAR(50) NOT NULL,
	categoria_id int NOT NULL,
	CONSTRAINT PK_apuntes_tema PRIMARY KEY(id),
	CONSTRAINT FK_tema_categoria FOREIGN KEY(categoria_id) REFERENCES apuntes_categoria(id)
);

CREATE TABLE apuntes_detalle_tema(
	id int IDENTITY NOT NULL,
	titulo VARCHAR(50) NOT NULL,
	contenido nvarchar(max) NULL,
	ruta_foto nvarchar(max) NULL,
	tema_id int NOT NULL,
	CONSTRAINT PK_apuntes_detalle_tema PRIMARY KEY(id),
	CONSTRAINT FK_detalle_tema FOREIGN KEY(tema_id) REFERENCES apuntes_tema(id)
);

-- ==============================================================================================>>>>>

CREATE TABLE usuario(
	id int IDENTITY NOT NULL,
	username VARCHAR(50) NOT NULL,
	password VARCHAR(1000) NOT NULL,
	activo bit NOT NULL,
	visible bit NOT NULL,
	CONSTRAINT PK_usuario PRIMARY KEY(id)
);

CREATE TABLE cargo(
	id int IDENTITY NOT NULL,
	authority VARCHAR(100) NULL,
	descripcion VARCHAR(50) NULL,
	activo bit NOT NULL,
	visible bit NOT NULL,
	CONSTRAINT PK_cargo PRIMARY KEY(id)
);

CREATE TABLE usuario_cargo(
	id int IDENTITY NOT NULL,
	usuario_id int NOT NULL,
	cargo_id int NOT NULL,
	CONSTRAINT PK_usuario_cargo PRIMARY KEY(id),
	CONSTRAINT FK_usuario_cargo_usuario FOREIGN KEY(usuario_id) REFERENCES usuario(id),
	CONSTRAINT FK_usuario_cargo_cargo FOREIGN KEY(cargo_id) REFERENCES cargo(id)
);

-- ==============================================================================================>>>>>

INSERT INTO apuntes_categoria (titulo) VALUES
('Sin Clasificación'),
('Ciencia'),
('Filosofía'),
('Programación'),
('Contabilidad Basica');


INSERT INTO apuntes_tema (categoria_id, titulo) VALUES
(4, 'Net core 5'),
(4, 'MySQL'),
(3, 'Platón');


INSERT INTO apuntes_detalle_tema (ruta_foto, contenido, titulo, tema_id) VALUES
(NULL, '<p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; string mySqlConnectionStr = Configuration.GetConnectionString(\"DefaultConnection\");<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; services.AddDbContextPool&lt;ApplicationDbContext&gt;(options =&gt; <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; );<br data-mce-bogus=\"1\"></p>', 'Conexion a MySQL', 1),
('Apuntes_Programacion/Aprendiendo_MySQL/FotoMySQL.png', '<p>Aparece un delfin<br data-mce-bogus=\"1\"></p>', '   Logo de MySQL', 2),
(NULL, '<p>La <b>alegoría de la caverna</b> (también conocida por el nombre de <b>mito de la caverna</b>, aunque en realidad solamente es una <a href=\"https://es.wikipedia.org/wiki/Alegor%C3%ADa\" title=\"Alegoría\">alegoría</a> de intenciones pedagógico-filosóficas, no un <a href=\"https://es.wikipedia.org/wiki/Mito\" title=\"Mito\">mito</a>\n pues no aparece reflejado como tal en los escritos de Platón ni en \nninguna otra obra antigua, ni siquiera entre los mitógrafos) se \nconsidera la más célebre alegoría de la historia de la filosofía<sup id=\"cite_ref-1\" class=\"reference separada\"><a href=\"https://es.wikipedia.org/wiki/Alegor%C3%ADa_de_la_caverna#cite_note-1\">1</a></sup>​ junto con la del <a href=\"https://es.wikipedia.org/wiki/Teor%C3%ADa_de_la_reminiscencia#El_Mito_del_Carro_Alado\" title=\"Teoría de la reminiscencia\">carro alado</a>.<sup id=\"cite_ref-2\" class=\"reference separada\"><a href=\"https://es.wikipedia.org/wiki/Alegor%C3%ADa_de_la_caverna#cite_note-2\">2</a></sup>​\n Su importancia se debe tanto a la utilidad de la narración para \nexplicar los aspectos más importantes del pensamiento platónico como a \nla riqueza de sus sugerencias filosóficas.\n</p>', '  Alegoría de la Caverna', 3);


-- ==============================================================================================>>>>>
-- Tablas usadas por autenticacion JWT

/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 09-11-2021 16:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 09-11-2021 16:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 09-11-2021 16:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 09-11-2021 16:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 09-11-2021 16:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 09-11-2021 16:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 09-11-2021 16:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO

-- ==============================================================================================>>>>>
-- Insertar un usuario

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'37cea47f-e672-4cbe-8819-83cfb0f07194', N'ejemplo@gmail.com', N'EJEMPLO@GMAIL.COM', N'ejemplo@gmail.com', N'EJEMPLO@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEELjvdCJ936owjHF6EjHmsrSQ7yAuT7BOGJ8mZ3Nkl2cRbwFKYNhjciXtWmo/5RxoA==', N'KDJ3HY7V5XBBQQTUEYNXOYJLFWIWH4WJ', N'b7ca300a-257f-4b00-a179-6fa8186ca374', NULL, 0, 0, NULL, 1, 0)
GO