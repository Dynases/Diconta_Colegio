USE [BDDicon_Colegio]
GO
/****** Object:  StoredProcedure [dbo].[sp_dg_TV002]    Script Date: 04/07/2019 5:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--drop procedure sp_Mam_TV002
ALTER PROCEDURE [dbo].[sp_dg_TV002] (@tipo int,@vcnumi int=-1,@vcidcore int=-1,@vcsector int=-1,
@vcSecNumi int=-1,@vcnumivehic int=-1,@vcalm int =-1,
@vcfdoc date=null,@vcclie int=-1,
@vcfvcr date=null,@vctipo int=-1,@vcest int=-1,@vcobs nvarchar(50)='',@vcdesc decimal(18,2)=0,
@vctotal decimal(18,2)=0,@vcuact nvarchar(10)='',
@numiVenta int=-1,@tipoC int=-1,@sucursalC int=-1,@ClienteCredito int=-1,@sucursal int=-1,@numerofactura int=-1,
@TV0022 TV0022Type Readonly,@TV0023 TV0022Type_Cabana Readonly,
@fecha1 date=null,@fecha2 date=null)

AS
BEGIN
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))
	DECLARE @newFecha date
	set @newFecha=GETDATE()

	



	IF @tipo=3 --para reporte de ventas 
		BEGIN
			BEGIN TRY
				select vcnumi,vcidcore,vcsector,cndesc1  as cedesc1,vcSecNumi,vcnumivehic,vcalm,FORMAT(vcfdoc,'dd/MM/yyyy') as vcfdoc,vcclie,vcfvcr,vctipo,vcest,vcobs,vcdesc,
				IIF (vcmoneda =1,vctotal,vctotal*6.96) as vctotal,vcclietc9
				from TV002,TC0051
				where cncod1  = 8 AND cncod2  = 1 and vcsector=cnnum  and
					  vcfdoc>=@fecha1 and vcfdoc<=@fecha2 



			
			END TRY
			BEGIN CATCH
				INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
					   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@vcuact)
			END CATCH

	END


End













