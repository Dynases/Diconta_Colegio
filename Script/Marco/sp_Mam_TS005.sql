USE [BDDicon_Colegio]
GO
/****** Object:  StoredProcedure [dbo].[sp_Mam_TS005]    Script Date: 03/07/2019 22:35:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--drop procedure sp_Mam_TS005
ALTER PROCEDURE [dbo].[sp_Mam_TS005] (@tipo int,@sdnumi int=-1,@sdcod nvarchar(20)='',@sddesc nvarchar(100)='',@sdprec decimal(18,2)=0,
@sdtipo int=-1,@sdsuc int=-1,@sdest int=-1,@sdmoneda int=-1,@sdemison int=-1,@sduact nvarchar(10)='')
AS
BEGIN
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))

	DECLARE @newFecha date
	set @newFecha=GETDATE()

	IF @tipo=-1 --ELIMINAR REGISTRO
	BEGIN
		BEGIN TRY 
			DELETE from TS005   where sdnumi  =@sdnumi           
			select @sdnumi as newNumi  --Consultar que hace newNumi
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),-1,@newFecha,@newHora,@sduact)
		END CATCH
	END

	IF @tipo=1 --NUEVO REGISTRO
	BEGIN
		BEGIN TRY 
			set @sdnumi=IIF((select COUNT(sdnumi) from TS005)=0,0,(select MAX(sdnumi) from TS005))+1
			INSERT INTO TS005  VALUES(@sdnumi ,@sdcod ,@sddesc ,@sdprec ,@sdtipo ,@sdsuc,@sdest,@sdmoneda,@sdemison )

			--insert into SI001 values('TC008',@cinumi ,1,@newFecha ,@newHora ,@ciuact )
            
			select @sdnumi as newNumi
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),1,@newFecha,@newHora,@sduact)
		END CATCH
	END
	
	IF @tipo=2--MODIFICACION
	BEGIN
		BEGIN TRY 
			UPDATE TS005   SET sdcod =@sdcod ,sddesc =@sddesc ,sdprec =@sdprec ,sdtipo =@sdtipo ,
			sdsuc =@sdsuc ,sdest =@sdest, sdmoneda=@sdmoneda,sdemision= @sdemison
			 Where sdnumi  = @sdnumi 
   

			select @sdnumi as newNumi
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),2,@newFecha,@newHora,@sduact)
		END CATCH
	END

	IF @tipo=3 --MOSTRAR TODOS
	BEGIN
		BEGIN TRY
	select a.sdnumi ,a.sdcod ,a.sddesc,a.sdprec ,a.sdtipo ,c.cndesc1 as tipo,a.sdsuc ,'SUCURSAL PRINCIPAL' as sucursal ,Cast(a.sdest as bit)as sdest, a.sdmoneda, a.sdemision   
	from TS005 as a
	inner join TC0051 as c on c.cncod1 =8 and c.cncod2 =1 and cnnum =a.sdtipo 
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@sduact)
		END CATCH

END


End






