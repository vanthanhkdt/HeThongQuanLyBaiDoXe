USE [DBBaiDoXe]
GO

/****** Object:  Table [dbo].[TBUsers]    Script Date: 11/12/2019 12:19:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBUsers](
	[STT] [bigint] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](max) NOT NULL,
	[MaSo] [nvarchar](50) NOT NULL,
	[MatKhau] [nvarchar](max) NOT NULL,
	[KhoaLop] [nvarchar](max) NOT NULL,
	[MaTheGui] [nvarchar](50) NOT NULL,
	[PhanQuyen] [int] NOT NULL,
	[ChoPhepHoatDong] [bit] NOT NULL,
	[NguoiThem] [nvarchar](max) NOT NULL,
	[NgayThem] [datetime] NOT NULL,
	[SoDuKhaDung] [bigint] NOT NULL,
	[DangGui] [bit] NOT NULL,
	[TruyCapLanCuoi] [datetime] NULL,
	[ThoiGianGuiCuoi] [datetime] NULL,
	[HinhAnh] [image] NULL,
	[DonGia] [bigint] NULL,
 CONSTRAINT [PK_TBUsers] PRIMARY KEY CLUSTERED 
(
	[MaTheGui] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


USE [DBBaiDoXe]
GO

/****** Object:  Table [dbo].[TBRegistration]    Script Date: 11/12/2019 12:18:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBRegistration](
	[STT] [bigint] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](max) NOT NULL,
	[MaSo] [nvarchar](50) NOT NULL,
	[MatKhau] [nvarchar](max) NOT NULL,
	[KhoaLop] [nvarchar](max) NOT NULL,
	[MaTheGui] [nvarchar](50) NOT NULL,
	[PhanQuyen] [int] NOT NULL,
	[LyDo] [nvarchar](max) NOT NULL,
	[DaXuLy] [bit] NULL,
	[DaNop] [bigint] NULL,
	[HinhAnh] [image] NULL,
 CONSTRAINT [PK_TBRegistration] PRIMARY KEY CLUSTERED 
(
	[MaTheGui] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


USE [DBBaiDoXe]
GO

/****** Object:  Table [dbo].[TBPhanQuyen]    Script Date: 11/12/2019 12:18:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBPhanQuyen](
	[STT] [int] IDENTITY(1,1) NOT NULL,
	[PhanQuyen] [nvarchar](50) NOT NULL,
	[MaPhanQuyen] [int] NOT NULL,
 CONSTRAINT [PK_TBPhanQuyen] PRIMARY KEY CLUSTERED 
(
	[MaPhanQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [DBBaiDoXe]
GO

/****** Object:  Table [dbo].[TBCardList]    Script Date: 11/12/2019 12:18:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBCardList](
	[STT] [bigint] IDENTITY(1,1) NOT NULL,
	[SoSeri] [nvarchar](max) NOT NULL,
	[MaThe] [nvarchar](max) NOT NULL,
	[GiaTri] [nvarchar](max) NOT NULL,
	[NgayKichHoat] [datetime] NULL,
	[DaKichHoat] [bit] NOT NULL,
	[TaiKhoanKichHoat] [nvarchar](50) NOT NULL,
	[ChoPhepHoatDong] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


USE [DBBaiDoXe]
GO

/****** Object:  Table [dbo].[TBActivities]    Script Date: 11/12/2019 12:18:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBActivities](
	[STT] [bigint] IDENTITY(1,1) NOT NULL,
	[MaSo] [nvarchar](50) NOT NULL,
	[HoatDong] [int] NOT NULL,
	[ThanhCong] [bit] NOT NULL,
	[NoiDung] [nvarchar](max) NULL,
	[MaTheNap] [nvarchar](max) NULL,
	[ThoiGian] [datetime] NOT NULL,
	[SoTienNap] [nvarchar](max) NULL,
	[HinhAnh] [nchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


