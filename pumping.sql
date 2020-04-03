USE [master]
GO
/****** Object:  Database [pumping]    Script Date: 03.04.2020 14:45:22 ******/
CREATE DATABASE [pumping]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'pumping', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\pumping.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'pumping_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\pumping_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [pumping] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [pumping].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [pumping] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [pumping] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [pumping] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [pumping] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [pumping] SET ARITHABORT OFF 
GO
ALTER DATABASE [pumping] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [pumping] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [pumping] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [pumping] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [pumping] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [pumping] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [pumping] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [pumping] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [pumping] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [pumping] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [pumping] SET  DISABLE_BROKER 
GO
ALTER DATABASE [pumping] SET AUTO_UPDATE_STATISTICS_ASYNC ON 
GO
ALTER DATABASE [pumping] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [pumping] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [pumping] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [pumping] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [pumping] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [pumping] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [pumping] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [pumping] SET  MULTI_USER 
GO
ALTER DATABASE [pumping] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [pumping] SET DB_CHAINING OFF 
GO
ALTER DATABASE [pumping] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [pumping] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [pumping]
GO
/****** Object:  User [en]    Script Date: 03.04.2020 14:45:22 ******/
CREATE USER [en] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [en]
GO
/****** Object:  Table [dbo].[db_objects]    Script Date: 03.04.2020 14:45:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[db_objects](
	[ObjectId] [int] IDENTITY(1,1) NOT NULL,
	[Address] [varchar](150) NOT NULL,
	[Identity] [varchar](50) NOT NULL,
 CONSTRAINT [PK_db_objects] PRIMARY KEY CLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PumpParameters]    Script Date: 03.04.2020 14:45:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PumpParameters](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Identity] [varchar](50) NOT NULL,
	[RecvDate] [datetime] NOT NULL,
	[TotalEnergy] [real] NULL,
	[Amperage1] [real] NULL,
	[Amperage2] [real] NULL,
	[Amperage3] [real] NULL,
	[Voltage1] [real] NULL,
	[Voltage2] [real] NULL,
	[Voltage3] [real] NULL,
	[CurrentElectricPower] [real] NULL,
	[TotalWaterRate] [real] NULL,
	[Presure] [real] NULL,
	[Alarm] [int] NULL,
	[Errors] [varchar](300) NULL,
 CONSTRAINT [PK_PumpParameters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_db_objects_Address]    Script Date: 03.04.2020 14:45:23 ******/
CREATE NONCLUSTERED INDEX [IX_db_objects_Address] ON [dbo].[db_objects]
(
	[Address] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_db_objects_Identity]    Script Date: 03.04.2020 14:45:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_db_objects_Identity] ON [dbo].[db_objects]
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PumpParamaters_RecvDate]    Script Date: 03.04.2020 14:45:23 ******/
CREATE NONCLUSTERED INDEX [IX_PumpParamaters_RecvDate] ON [dbo].[PumpParameters]
(
	[RecvDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_PumpParametersIdentity]    Script Date: 03.04.2020 14:45:23 ******/
CREATE NONCLUSTERED INDEX [IX_PumpParametersIdentity] ON [dbo].[PumpParameters]
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [pumping] SET  READ_WRITE 
GO
