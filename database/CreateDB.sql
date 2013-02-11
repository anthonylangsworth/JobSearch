﻿USE [master]
GO
/****** Object:  Database [JobSearch]    Script Date: 2/11/2013 11:13:36 PM ******/
CREATE DATABASE [JobSearch]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JobSearch', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\JobSearch.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'JobSearch_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\JobSearch_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [JobSearch] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JobSearch].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JobSearch] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JobSearch] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JobSearch] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JobSearch] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JobSearch] SET ARITHABORT OFF 
GO
ALTER DATABASE [JobSearch] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JobSearch] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [JobSearch] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JobSearch] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JobSearch] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JobSearch] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JobSearch] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JobSearch] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JobSearch] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JobSearch] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JobSearch] SET  DISABLE_BROKER 
GO
ALTER DATABASE [JobSearch] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JobSearch] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JobSearch] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JobSearch] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JobSearch] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JobSearch] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JobSearch] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JobSearch] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JobSearch] SET  MULTI_USER 
GO
ALTER DATABASE [JobSearch] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JobSearch] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JobSearch] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JobSearch] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [JobSearch]
GO
/****** Object:  Table [dbo].[Activity]    Script Date: 2/11/2013 11:13:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobOpeningId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
	[Start] [datetime] NOT NULL,
	[Duration] [datetimeoffset](7) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Completed] [bit] NOT NULL,
 CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contact]    Script Date: 2/11/2013 11:13:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Organization] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobOpening]    Script Date: 2/11/2013 11:13:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOpening](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Organization] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
 CONSTRAINT [PK_JobOpening] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobOpeningContact]    Script Date: 2/11/2013 11:13:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobOpeningContact](
	[JobOpeningId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
 CONSTRAINT [PK_JobOpeningContact] PRIMARY KEY CLUSTERED 
(
	[JobOpeningId] ASC,
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([Id])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_Contact]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_JobOpening] FOREIGN KEY([JobOpeningId])
REFERENCES [dbo].[JobOpening] ([Id])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_JobOpening]
GO
ALTER TABLE [dbo].[JobOpeningContact]  WITH CHECK ADD  CONSTRAINT [FK_JobOpeningContact_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([Id])
GO
ALTER TABLE [dbo].[JobOpeningContact] CHECK CONSTRAINT [FK_JobOpeningContact_Contact]
GO
ALTER TABLE [dbo].[JobOpeningContact]  WITH CHECK ADD  CONSTRAINT [FK_JobOpeningContact_JobOpening] FOREIGN KEY([JobOpeningId])
REFERENCES [dbo].[JobOpening] ([Id])
GO
ALTER TABLE [dbo].[JobOpeningContact] CHECK CONSTRAINT [FK_JobOpeningContact_JobOpening]
GO
USE [master]
GO
ALTER DATABASE [JobSearch] SET  READ_WRITE 
GO
