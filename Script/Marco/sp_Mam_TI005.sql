USE [BDDicon_Colegio]
GO
/****** Object:  StoredProcedure [dbo].[sp_Mam_TI005]    Script Date: 04/07/2019 5:29:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE [dbo].[sp_Mam_TI005](@tipo int,@oanumi int=-1,@oanumdoc nvarchar(10)='',@oatip int=-1,@oaano int=-1,@oames int=-1,
									 @oanum int=-1,@oafdoc date=null,@oatc decimal(18, 2)=-1,@oaglosa nvarchar(100)='',@oaobs nvarchar(50)='',
									 @oaemp int=-1,@fecha1 date=null,@fecha2 date=null,@uact nvarchar(10),@TI005 dbo.Mam_TI005Type Readonly,
									 @ifnumi int =-1,@ifto001numi int=-1,@iftc decimal(18,2)=null,
									 @iffechai date=null,@iffechaf date=null,@ifest int=-1,@Estado dbo.Mam_EstadoVentasType Readonly,@ifsuc int=-1, 
									 @Banco dbo.mam_BancoType Readonly)
AS
BEGIN
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))
	 declare @numiTo001 int
	DECLARE @newFecha date
	set @newFecha=GETDATE()
	declare @oanumibanco int
	DECLARE @fecha datetime

	set @fecha=@oafdoc
	set @fecha=DATEADD(HOUR,DATEPART (HOUR, GETDATE())  ,@fecha)
	set @fecha=DATEADD(MINUTE,DATEPART (MINUTE, GETDATE())  ,@fecha)
	set @fecha=DATEADD(SECOND,DATEPART (SECOND, GETDATE())  ,@fecha)

	
		IF @tipo=-1 --MOSTRAR CUENTAS
	BEGIN
		BEGIN TRY	
		declare @numiComprobante int 
		set @numiComprobante=(select top 1 ifto001numi from TI005 where ifnumi=@ifnumi)
				declare @numiComprobante2 int 
		set @numiComprobante2=(select top 1 ifto001numibanco from TI005 where ifnumi=@ifnumi)

		update TV002 set vcidcore=0
		from TV002 inner join TI005 as a on a.ifnumi=@ifnumi 
		and vcidcore =a.ifto001numi

		delete from TO0011 where obnumito1=@numiComprobante
		delete from TO001 where oanumi=@numiComprobante
		delete from TO0011 where obnumito1=@numiComprobante2
		delete from TO001 where oanumi=@numiComprobante2
		delete from TI005 where ifnumi=@ifnumi
		delete from TI0051 where ikifnumi=@ifnumi
		SELECT @oanumi AS newNumi
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH

END

	IF @tipo=1 --NUEVO REGISTRO
	BEGIN
	
		BEGIN TRY 
		
			set @oanum=IIF((select COUNT(oanum) from TO001 where oatip=@oatip and oaano=@oaano and oames=@oames and oaemp=@oaemp)=0,0,(select MAX(oanum) from TO001 where oatip=@oatip and oaano=@oaano and oames=@oames and oaemp=@oaemp))+1
			declare @tipoCompr nvarchar(1)=IIF(@oatip=1,'I',IIF(@oatip=2,'E','T'));
			set @oanumdoc=CONCAT(RIGHT(CAST(@oaano as nvarchar),2),IIF(@oames<10,concat('0',cast(@oames as nvarchar)),cast(@oames as nvarchar)),'-',@tipoCompr,'-',CAST(@oanum as nvarchar));
			
			declare @NameSucursal nvarchar(100)='Sucursal Principal'

			INSERT INTO TO001 VALUES(@oanumdoc,@oatip,@oaano,@oames,@oanum,@iffechai,@oatc,@NameSucursal ,@oaobs,@oaemp)


			declare @variablePropoSucursal int =(select top 1 a.sfnumivarp  from TS007 as a where sfnumisuc=@ifsuc )
			set @oanumi=@@IDENTITY
			-- INSERTO EL DETALLE 
			INSERT INTO TO0011(obnumito1,oblin,obcuenta,obaux1,obaux2,obaux3,obobs,obobs2,obcheque,obtc,obdebebs,obhaberbs,obdebeus,obhaberus)
			SELECT @oanumi,td.linea ,td.canumi ,td.variable ,@variablePropoSucursal,0,td.cadesc ,'','',@iftc ,
				   td.debe ,td.haber,td.debesus ,td.habersus  FROM @TI005  AS td where td.linea >0;

     
	        set @numiTo001=(select  top 1 a.oanumi   from  TO001 as a where a.oanumdoc =@oanumdoc )
			set @ifnumi=IIF((select COUNT(ifnumi) from TI005)=0,0,(select MAX(ifnumi) from TI005))+1




			update TV002 set TV002.vcidcore =@oanumi 
			from TV002 inner join @Estado as td on td.numi =TV002.vcnumi 
			SELECT @oanumi AS newNumi

			
			declare @numibanco int = iif((select count(*) from ti0051)=0,1,(select MAX(iknumi) from TI0051))

			INSERT INTO TI0051(iknumi, ikifnumi, ikbanco, ikmonto)
			select @numibanco, @ifnumi,td2.canumi, td2.camonto from @banco as td2 where td2.camonto > 0;

			-- Grabar Asiento Deposito Banco
			set @oanum=IIF((select COUNT(oanum) from TO001 where oatip=3 and oaano=@oaano and oames=@oames and oaemp=@oaemp)=0,0,(select MAX(oanum) from TO001 where oatip=3 and oaano=@oaano and oames=@oames and oaemp=@oaemp))+1
			set @tipoCompr = 'T';
			set @oanumdoc=CONCAT(RIGHT(CAST(@oaano as nvarchar),2),IIF(@oames<10,concat('0',cast(@oames as nvarchar)),cast(@oames as nvarchar)),'-T-',CAST(@oanum as nvarchar));
			
			--declare @NameSucursal nvarchar(100)=(select ISnull(a.cadesc,'')from DBDies .dbo.TC001 as a where a.canumi =@ifsuc) 

			INSERT INTO TO001 VALUES(@oanumdoc,3,@oaano,@oames,@oanum,@iffechai,@oatc,@NameSucursal ,@oaobs,@oaemp)

			--declare @variablePropoSucursal int =(select top 1 a.sfnumivarp  from TS007 as a where sfnumisuc=@ifsuc )
			set @oanumibanco=@@IDENTITY   -----Aqui debo poner el numi del asiento del banco
			-- INSERTO EL DETALLE 
			--Declare @montous decimal(18,2) = 
			INSERT INTO TO0011(obnumito1,oblin,obcuenta,obaux1,obaux2,obaux3,obobs,obobs2,obcheque,obtc,obdebebs,obhaberbs,obdebeus,obhaberus)
			SELECT @oanumibanco,td.canumi ,td.ctanumi ,56 ,@variablePropoSucursal,0,td.canombre ,'','',@iftc ,
				   td.camonto ,0,(td.camonto/@iftc) ,0  FROM @banco  AS td where td.camonto >0;

			Declare @suma as decimal(18,2) = (select sum(td.camonto) from @banco as td)
			INSERT INTO TO0011(obnumito1,oblin,obcuenta,obaux1,obaux2,obaux3,obobs,obobs2,obcheque,obtc,obdebebs,obhaberbs,obdebeus,obhaberus)
			VALUES(@oanumibanco,20 ,8 ,56 ,@variablePropoSucursal,0,'TRASPASO A BANCOS' ,'','',@iftc ,
				   0, @suma ,0,(@suma/@iftc))

			-- Grabar Deposito Banco
			insert into TI005 values(@ifnumi,@oanumi ,@iftc ,@iffechai ,@iffechaf ,@ifest,@ifsuc,@oanumibanco ,@newFecha, @newHora,@uact)
			
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),1,@newFecha,@newHora,@uact)

		
		END CATCH
	END
	IF @tipo=3 --MOSTRAR CUENTAS
	BEGIN
		BEGIN TRY	

		select a.ifnumi ,a.ifto001numi,comprobante .oanumdoc  ,a.iftc ,a.iffechai ,a.iffechaf ,a.ifest ,a.ifsuc 
		from  TI005 as a 
		inner join TO001 as comprobante on comprobante .oanumi =a.ifto001numi 
		order by a.ifnumi asc
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH

END

IF @tipo=4 --VER DETALLE
	BEGIN
		BEGIN TRY	
  select cuenta .canumi ,cuenta .cacta as nro,detalle.obobs as cadesc ,0 as chporcen,0 as chdebe ,0 as chhaber,i.iftc  as tc
   ,detalle .obdebebs  as debe,obhaberbs  as haber,obdebeus  as debesus
   ,obhaberus  as habersus,obaux1  as variable,oblin  as linea
  from TO001 as a inner join TO0011 as detalle on detalle .obnumito1 =a.oanumi 
  inner join TC001 as cuenta on cuenta .canumi =detalle .obcuenta 
 inner join TI005 as i on i.ifto001numi =a.oanumi 
 and i.ifnumi =@ifnumi
 order by detalle .oblin asc

		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH

END

IF @tipo=5 --Bancos
	BEGIN
		BEGIN TRY	
  select banco.canumi ,cast ('' as image) as img ,banco .canombre ,banco .cacuenta ,banco.caimage ,cast(0 as decimal(18,2)) as camonto, 0 as caestado, c.canumi as ctanumi
  from BA001 as banco, tc001 c 
  where banco.canumi <>1
  and banco.cacuenta = c.cacta 
 order by banco.canumi asc

		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH

END

IF @tipo=6 --Bancos Integracion
	BEGIN
		BEGIN TRY	
  select *
  from TI0051 where ikifnumi = @ifnumi

		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH

END
--IF @tipo=7 --Bancos Integracion
--	BEGIN
--		BEGIN TRY	
--  select banco.canumi ,cast ('' as image) as img ,banco .canombre ,banco .cacuenta ,banco.caimage ,isnull(detalle.ikmonto,0)  as camonto, 1 as caestado
--  from DBDies .dbo.BA001 as banco inner join TI0051 
--  as detalle on detalle .ikbanco =banco.canumi and detalle.ikifnumi =@ifnumi 
-- order by banco.canumi asc


--		END TRY
--		BEGIN CATCH
--			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
--				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
--		END CATCH

--END

IF @tipo=8 --VER DETALLE ASIENTO DEL BANCO
	BEGIN
		BEGIN TRY	
  select cuenta .canumi ,cuenta .cacta as nro,detalle.obobs as cadesc ,0 as chporcen,0 as chdebe ,0 as chhaber,i.iftc  as tc
   ,detalle .obdebebs  as debe,obhaberbs  as haber,obdebeus  as debesus
   ,obhaberus  as habersus,obaux1  as variable,oblin  as linea
  from TO001 as a inner join TO0011 as detalle on detalle .obnumito1 =a.oanumi 
  inner join TC001 as cuenta on cuenta .canumi =detalle .obcuenta 
 inner join TI005 as i on i.ifto001numibanco  =a.oanumi 
 and i.ifnumi =@ifnumi
 order by detalle .oblin asc

		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH

END
END







