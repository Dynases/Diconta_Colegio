USE [BDDicon_Colegio]
GO

/****** Object:  UserDefinedTableType [dbo].[Ventas_Type]    Script Date: 03/07/2019 22:26:12 ******/
DROP TYPE [dbo].[Ventas_Type]
GO

/****** Object:  UserDefinedTableType [dbo].[Ventas_Type]    Script Date: 03/07/2019 22:26:12 ******/
CREATE TYPE [dbo].[Ventas_Type] AS TABLE(
	[codigobanco] [int] NULL,
	[codigoestudiante] [int] NULL,
	[estudiante] [nvarchar](200) NULL,
	[codigoservicio] [int] NULL,
	[servicio] [nvarchar](200) NULL,
	[montodeposito] [decimal](18, 2) NULL,
	[fechadeposito] [date] NULL,
	[nrocuota] [int] NULL,
	[esfactura] [bit] NULL,
	[nrodocumento] [nvarchar](30) NULL,
	[codigocontrol] [nvarchar](20) NULL,
	[estado] [int] NULL,
	[sdmoneda] [int] NULL,
	[moneda] [nvarchar](70) NULL
)
GO


