USE AppApuntesNet5
GO

CREATE TABLE apuntes_categoria(
	id INT IDENTITY NOT NULL,
	titulo VARCHAR(50) NOT NULL,
	logo VARBINARY(max) NULL, 
	tipo_logo VARCHAR(30) NULL, 
	CONSTRAINT PK_apuntes_categoria PRIMARY KEY(id)
);

CREATE TABLE apuntes_tema(
	id INT IDENTITY NOT NULL,
	titulo VARCHAR(50) NOT NULL,
	categoria_id INT NOT NULL,
	CONSTRAINT PK_apuntes_tema PRIMARY KEY(id),
	CONSTRAINT FK_tema_categoria FOREIGN KEY(categoria_id) REFERENCES apuntes_categoria(id)
);

CREATE TABLE apuntes_detalle_tema(
	id INT IDENTITY NOT NULL,
	titulo VARCHAR(50) NOT NULL,
	contenido NVARCHAR(max) NULL,
	ruta_foto NVARCHAR(max) NULL,
	tema_id INT NOT NULL,
	CONSTRAINT PK_apuntes_detalle_tema PRIMARY KEY(id),
	CONSTRAINT FK_detalle_tema FOREIGN KEY(tema_id) REFERENCES apuntes_tema(id)
);

-- Crear indices unicos	

CREATE UNIQUE INDEX apuntes_categoria_titulo_UINDEX ON apuntes_categoria (titulo);			-- El titulo de apuntes_categoria sera unico
CREATE UNIQUE INDEX apuntes_tema_titulo_UINDEX ON apuntes_tema (titulo);					-- El titulo de apuntes_tema sera unico
CREATE UNIQUE INDEX apuntes_detalle_tema_titulo_UINDEX ON apuntes_detalle_tema (titulo);	-- El titulo de apuntes_detalle_tema sera unico
GO

-- ==============================================================================================>>>>>

INSERT apuntes_categoria (titulo, logo, tipo_logo) VALUES ('Sin Clasificación', NULL, NULL)
INSERT apuntes_categoria (titulo, logo, tipo_logo) VALUES ('Ciencia', NULL, NULL)
INSERT apuntes_categoria (titulo, logo, tipo_logo) VALUES ('Filosofía', NULL, NULL)
INSERT apuntes_categoria (titulo, logo, tipo_logo) VALUES ('Programación', 0x89504E470D0A1A0A0000000D49484452000000130000001008020000007BA6D335000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000007549444154384FCD8DCB0D80300C433B0847F6DF8C198AA9A3905F8BE0C4532F76FCD4D6BF529BCD23ADA768313DB6DDBE528E95D5AEEF06EC39505CC6396B8457CEC81D7098698A4C072F4CF6B2FE9399652D65BD30B9231A653AF021C9F6AD4C30938306620659CE1A282A80A9455A4FDD3ED3FB0922CF36B9227035BD0000000049454E44AE426082, N'data:image/png;base64')
INSERT apuntes_categoria (titulo, logo, tipo_logo) VALUES ('Contabilidad Basica', NULL, NULL)


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

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

-- ==============================================================================================>>>>>
-- Insertar un usuario

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'37cea47f-e672-4cbe-8819-83cfb0f07194', N'ejemplo@gmail.com', N'EJEMPLO@GMAIL.COM', N'ejemplo@gmail.com', N'EJEMPLO@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEELjvdCJ936owjHF6EjHmsrSQ7yAuT7BOGJ8mZ3Nkl2cRbwFKYNhjciXtWmo/5RxoA==', N'KDJ3HY7V5XBBQQTUEYNXOYJLFWIWH4WJ', N'b7ca300a-257f-4b00-a179-6fa8186ca374', NULL, 0, 0, NULL, 1, 0)
GO