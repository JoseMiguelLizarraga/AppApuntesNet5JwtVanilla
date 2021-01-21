
drop database if exists app_apuntes_net5;
create database app_apuntes_net5 default character set utf8 default collate utf8_general_ci;
use app_apuntes_net5;  

-- ======================================================================================>>>>>
-- phpMyAdmin SQL Dump
-- version 4.8.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 21-01-2021 a las 21:48:22
-- Versión del servidor: 10.1.31-MariaDB
-- Versión de PHP: 7.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `app_apuntes_net5`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `apuntes_categoria`
--

CREATE TABLE `apuntes_categoria` (
  `id` int(11) NOT NULL,
  `titulo` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `apuntes_categoria`
--

INSERT INTO `apuntes_categoria` (`id`, `titulo`) VALUES
(1, 'Sin Clasificación'),
(2, 'Ciencia'),
(3, 'Filosofía'),
(4, 'Programación'),
(5, 'Contabilidad Basica');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `apuntes_detalle_tema`
--

CREATE TABLE `apuntes_detalle_tema` (
  `id` int(11) NOT NULL,
  `ruta_foto` longtext CHARACTER SET utf8mb4,
  `contenido` longtext CHARACTER SET utf8mb4,
  `titulo` longtext CHARACTER SET utf8mb4,
  `tema_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `apuntes_detalle_tema`
--

INSERT INTO `apuntes_detalle_tema` (`id`, `ruta_foto`, `contenido`, `titulo`, `tema_id`) VALUES
(1, NULL, '<p><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; string mySqlConnectionStr = Configuration.GetConnectionString(\"DefaultConnection\");<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; services.AddDbContextPool&lt;ApplicationDbContext&gt;(options =&gt; <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; );<br data-mce-bogus=\"1\"></p>', 'Conexion a MySQL', 1),
(2, 'Apuntes_Programacion/Aprendiendo_MySQL/FotoMySQL.png', '<p>Aparece un delfin<br data-mce-bogus=\"1\"></p>', '   Logo de MySQL', 2),
(3, NULL, '<p>La <b>alegoría de la caverna</b> (también conocida por el nombre de <b>mito de la caverna</b>, aunque en realidad solamente es una <a href=\"https://es.wikipedia.org/wiki/Alegor%C3%ADa\" title=\"Alegoría\">alegoría</a> de intenciones pedagógico-filosóficas, no un <a href=\"https://es.wikipedia.org/wiki/Mito\" title=\"Mito\">mito</a>\n pues no aparece reflejado como tal en los escritos de Platón ni en \nninguna otra obra antigua, ni siquiera entre los mitógrafos) se \nconsidera la más célebre alegoría de la historia de la filosofía<sup id=\"cite_ref-1\" class=\"reference separada\"><a href=\"https://es.wikipedia.org/wiki/Alegor%C3%ADa_de_la_caverna#cite_note-1\">1</a></sup>​ junto con la del <a href=\"https://es.wikipedia.org/wiki/Teor%C3%ADa_de_la_reminiscencia#El_Mito_del_Carro_Alado\" title=\"Teoría de la reminiscencia\">carro alado</a>.<sup id=\"cite_ref-2\" class=\"reference separada\"><a href=\"https://es.wikipedia.org/wiki/Alegor%C3%ADa_de_la_caverna#cite_note-2\">2</a></sup>​\n Su importancia se debe tanto a la utilidad de la narración para \nexplicar los aspectos más importantes del pensamiento platónico como a \nla riqueza de sus sugerencias filosóficas.\n</p>', '  Alegoría de la Caverna', 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `apuntes_tema`
--

CREATE TABLE `apuntes_tema` (
  `id` int(11) NOT NULL,
  `categoria_id` int(11) NOT NULL,
  `titulo` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `apuntes_tema`
--

INSERT INTO `apuntes_tema` (`id`, `categoria_id`, `titulo`) VALUES
(1, 4, 'Net core 5'),
(2, 4, 'MySQL'),
(3, 3, 'Platón');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetroleclaims`
--

CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4,
  `ClaimValue` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetroles`
--

CREATE TABLE `aspnetroles` (
  `Id` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserclaims`
--

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4,
  `ClaimValue` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserlogins`
--

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `ProviderKey` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4,
  `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetuserroles`
--

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `RoleId` varchar(95) CHARACTER SET utf8mb4 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetusers`
--

CREATE TABLE `aspnetusers` (
  `Id` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4,
  `SecurityStamp` longtext CHARACTER SET utf8mb4,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4,
  `PhoneNumber` longtext CHARACTER SET utf8mb4,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `aspnetusers`
--

INSERT INTO `aspnetusers` (`Id`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
('d9519ec7-0ad0-488f-8716-682738fad0dc', 'ejemplo@gmail.com', 'EJEMPLO@GMAIL.COM', 'ejemplo@gmail.com', 'EJEMPLO@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEGsNJsLffssLVdXHF6O6LjO7lP4gDWhEkwfJZZ1HnG4aaurVi1XzBHTfr2z+wOfIjw==', 'ICGAUV42FGFWTOYU4OJ5UQ64ZRQT4UHI', '2741ff37-dc82-4b5c-a800-a1c5b7cb6d00', NULL, 0, 0, NULL, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aspnetusertokens`
--

CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `LoginProvider` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `Name` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cargo`
--

CREATE TABLE `cargo` (
  `id` int(11) NOT NULL,
  `visible` tinyint(1) NOT NULL,
  `activo` tinyint(1) NOT NULL,
  `descripcion` longtext CHARACTER SET utf8mb4,
  `authority` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `username` longtext CHARACTER SET utf8mb4,
  `apellido_materno` longtext CHARACTER SET utf8mb4,
  `visible` tinyint(1) NOT NULL,
  `apellido_paterno` longtext CHARACTER SET utf8mb4,
  `activo` tinyint(1) NOT NULL,
  `nombre` longtext CHARACTER SET utf8mb4,
  `telefono` longtext CHARACTER SET utf8mb4,
  `largo_password` int(11) NOT NULL,
  `clave_pdt` longtext CHARACTER SET utf8mb4,
  `password` longtext CHARACTER SET utf8mb4,
  `rut` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario_cargo`
--

CREATE TABLE `usuario_cargo` (
  `id` int(11) NOT NULL,
  `cargo_id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `apuntes_categoria`
--
ALTER TABLE `apuntes_categoria`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `apuntes_detalle_tema`
--
ALTER TABLE `apuntes_detalle_tema`
  ADD PRIMARY KEY (`id`),
  ADD KEY `IX_apuntes_detalle_tema_tema_id` (`tema_id`);

--
-- Indices de la tabla `apuntes_tema`
--
ALTER TABLE `apuntes_tema`
  ADD PRIMARY KEY (`id`),
  ADD KEY `IX_apuntes_tema_categoria_id` (`categoria_id`);

--
-- Indices de la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`);

--
-- Indices de la tabla `aspnetroles`
--
ALTER TABLE `aspnetroles`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FK_AspNetUserClaims_AspNetUsers_UserId` (`UserId`);

--
-- Indices de la tabla `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `FK_AspNetUserLogins_AspNetUsers_UserId` (`UserId`);

--
-- Indices de la tabla `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `FK_AspNetUserRoles_AspNetRoles_RoleId` (`RoleId`);

--
-- Indices de la tabla `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indices de la tabla `cargo`
--
ALTER TABLE `cargo`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuario_cargo`
--
ALTER TABLE `usuario_cargo`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_usuario_cargo_cargo_cargo_id` (`cargo_id`),
  ADD KEY `FK_usuario_cargo_usuario_usuario_id` (`usuario_id`);

--
-- Indices de la tabla `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `apuntes_categoria`
--
ALTER TABLE `apuntes_categoria`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `apuntes_detalle_tema`
--
ALTER TABLE `apuntes_detalle_tema`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `apuntes_tema`
--
ALTER TABLE `apuntes_tema`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `cargo`
--
ALTER TABLE `cargo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario_cargo`
--
ALTER TABLE `usuario_cargo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `apuntes_detalle_tema`
--
ALTER TABLE `apuntes_detalle_tema`
  ADD CONSTRAINT `FK_apuntes_detalle_tema_apuntes_tema_tema_id` FOREIGN KEY (`tema_id`) REFERENCES `apuntes_tema` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `apuntes_tema`
--
ALTER TABLE `apuntes_tema`
  ADD CONSTRAINT `FK_apuntes_tema_apuntes_categoria_categoria_id` FOREIGN KEY (`categoria_id`) REFERENCES `apuntes_categoria` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `usuario_cargo`
--
ALTER TABLE `usuario_cargo`
  ADD CONSTRAINT `FK_usuario_cargo_cargo_cargo_id` FOREIGN KEY (`cargo_id`) REFERENCES `cargo` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_usuario_cargo_usuario_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
