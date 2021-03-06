USE [RC_DB_File]
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'CreateDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'FileGuid'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'FileGuid'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'AppKey'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'AppKey'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'Title'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'Title'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'Id'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'CreateUser'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'CreateDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'Sort'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'Sort'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'IsDel'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'IsLog'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'IsLog'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'IsEnable'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'Count'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'Count'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'AppSecret'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'AppSecret'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'AppKey'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'AppKey'
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_RC_Api_Log_GID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RC_File_Log] DROP CONSTRAINT [DF_RC_Api_Log_GID]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_RC_Api_File_inputdate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RC_File_FileUpload] DROP CONSTRAINT [DF_RC_Api_File_inputdate]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_RC_Api_Account_GID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RC_File_Account] DROP CONSTRAINT [DF_RC_Api_Account_GID]
END
GO
/****** Object:  Table [dbo].[RC_File_Log]    Script Date: 2018/9/2 10:28:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RC_File_Log]') AND type in (N'U'))
DROP TABLE [dbo].[RC_File_Log]
GO
/****** Object:  Table [dbo].[RC_File_FileUpload]    Script Date: 2018/9/2 10:28:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RC_File_FileUpload]') AND type in (N'U'))
DROP TABLE [dbo].[RC_File_FileUpload]
GO
/****** Object:  Table [dbo].[RC_File_Account]    Script Date: 2018/9/2 10:28:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RC_File_Account]') AND type in (N'U'))
DROP TABLE [dbo].[RC_File_Account]
GO
/****** Object:  Table [dbo].[RC_File_Account]    Script Date: 2018/9/2 10:28:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RC_File_Account]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RC_File_Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GID] [uniqueidentifier] NULL,
	[Name] [nvarchar](500) NULL,
	[AppKey] [nvarchar](200) NULL,
	[AppSecret] [nvarchar](200) NULL,
	[Count] [bigint] NULL,
	[IsEnable] [bit] NULL,
	[IsLog] [bit] NULL,
	[IsDel] [bit] NULL,
	[Sort] [bigint] NULL,
	[CreateDate] [datetime] NULL,
	[CreateUser] [nvarchar](64) NULL,
	[MaxSize] [int] NULL,
	[Extension] [nvarchar](200) NULL,
	[SavePath] [nvarchar](2000) NULL,
 CONSTRAINT [PK_RC_Api_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RC_File_FileUpload]    Script Date: 2018/9/2 10:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RC_File_FileUpload]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RC_File_FileUpload](
	[guid] [nvarchar](40) NOT NULL,
	[type] [nvarchar](10) NULL,
	[name] [nvarchar](500) NULL,
	[fullpath] [nvarchar](1000) NULL,
	[parentpath] [nvarchar](600) NULL,
	[extension] [nvarchar](30) NULL,
	[length] [nvarchar](30) NULL,
	[creationtime] [nvarchar](20) NULL,
	[level] [int] NULL,
	[isroot] [int] NULL,
	[createdate] [datetime] NULL,
	[tablename] [nvarchar](50) NULL,
	[tablekeyid] [nvarchar](50) NULL,
 CONSTRAINT [PK_RC_File_FileUpload] PRIMARY KEY NONCLUSTERED 
(
	[guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[RC_File_Log]    Script Date: 2018/9/2 10:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RC_File_Log]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RC_File_Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GID] [uniqueidentifier] NULL,
	[Title] [nvarchar](500) NULL,
	[AppKey] [nvarchar](50) NULL,
	[FileGuid] [nvarchar](500) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_RC_Api_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_RC_Api_Account_GID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RC_File_Account] ADD  CONSTRAINT [DF_RC_Api_Account_GID]  DEFAULT (newid()) FOR [GID]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_RC_Api_File_inputdate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RC_File_FileUpload] ADD  CONSTRAINT [DF_RC_Api_File_inputdate]  DEFAULT (getdate()) FOR [createdate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_RC_Api_Log_GID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RC_File_Log] ADD  CONSTRAINT [DF_RC_Api_Log_GID]  DEFAULT (newid()) FOR [GID]
END
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'AppKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AppKey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'AppKey'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'AppSecret'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AppSecret' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'AppSecret'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'Count'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'调用次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'Count'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'IsEnable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'IsLog'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开启日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'IsLog'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'IsDel'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否被删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'Sort'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'Sort'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'CreateDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', N'COLUMN',N'CreateUser'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account', @level2type=N'COLUMN',@level2name=N'CreateUser'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Account', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用服务账户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Account'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'Title'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'Title'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'AppKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'调用者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'AppKey'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'FileGuid'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'调用方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'FileGuid'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', N'COLUMN',N'CreateDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_description' , N'SCHEMA',N'dbo', N'TABLE',N'RC_File_Log', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_description', @value=N'接口调用日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RC_File_Log'
GO
