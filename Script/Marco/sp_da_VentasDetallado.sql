
/****** Object:  StoredProcedure [dbo].[sp_da_VentasDetallado]    Script Date: 04/07/2019 5:54:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_da_VentasDetallado] (@tipo int, @fecha1 date=null, @fecha2 date=null, @sector int = -1, @almacen int = -1 ,@uact nvarchar(10)='', @servicio int = -1)
AS

BEGIN
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))

	DECLARE @newFecha date
	set @newFecha=GETDATE()

	
	IF @tipo=1 --MOSTRAR TODOS
	BEGIN
		BEGIN TRY
			select b.vcnumi  as fvanfac, FORMAT(b.vcfdoc , 'dd/MM/yyyy') as fvafec,
			(select top 1 cbdol from TC002 where cbfecha=b.vcfdoc ) as tc,
			b.vcfactura as vcfactura,concat(cliente.alnombre ,' ', cliente.alapellido_p ,' ',cliente .alapellido_m ) as fvadescli1, IIF (b.vcmoneda =1, sum(d.vdtotdesc), sum(d.vdtotdesc)*6.96) as fvasubtotal, sum(d.vddesc) as fvadesc,IIF (b.vcmoneda =1, sum(d.vdtotdesc), sum(d.vdtotdesc)*6.96) as totfact,
			IIF (b.vcmoneda =1,sum(d.vdtotdesc * 0.13),sum(d.vdtotdesc * 0.13)*6.96) as fvadebfis, --sum(d.vdptot) as fvasubtotal,
			IIF(b.vcmoneda =1,sum((d.vdtotdesc - d.vddesc) - (d.vdtotdesc * 0.13)),sum((d.vdtotdesc - d.vddesc) - (d.vdtotdesc * 0.13))*6.96) as vneta,
			b.vctipo, b.vcsector as cenum,'COFRICO' as cedesc1, b.vcalm, b.vcobs, d.vdserv , e.sddesc, b.vcnumi, sum(d.vdcmin) as vdcmin,
			(select  lib.cndesc1  from TC0051 as lib where lib.cncod1=10 and lib.cncod2 =1 and lib.cnnum=b.vctipo ) as tipo
			from TV002 as b,  TV0021 d,TS005  e,DBDiSchool.dbo.AL001  cliente
			where b.vcnumi=d.vdvc2numi and d.vdserv=e.sdnumi and cliente.alnumi  =b.vcclie 
			and b.vcfdoc  >= @fecha1 and b.vcfdoc  <= @fecha2 and b.vcalm=@almacen  
			
			group by b.vcfdoc,b.vcmoneda ,cliente.alnombre ,cliente.alapellido_p ,cliente.alapellido_m  ,  vcfactura, vctipo, vcsector,cliente.alnumi ,  vcalm, vcobs, vdserv, sddesc, vcnumi

			
				

		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end

	IF @tipo=2 --MOSTRAR POR SECTOR
	BEGIN
		BEGIN TRY
			select b.vcnumi  as fvanfac, FORMAT(b.vcfdoc , 'dd/MM/yyyy') as fvafec,
			(select top 1 cbdol from TC002 where cbfecha=b.vcfdoc ) as tc,
			b.vcfactura as vcfactura,cliente.yddesc as fvadescli1, sum(d.vdtotdesc) as fvasubtotal, sum(d.vddesc) as fvadesc, sum(d.vdtotdesc) as totfact,
			sum(d.vdtotdesc * 0.13) as fvadebfis, --sum(d.vdptot) as fvasubtotal,
			sum((d.vdtotdesc - d.vddesc) - (d.vdtotdesc * 0.13)) as vneta,
			b.vctipo, b.vcsector as cenum,'COFRICO' as cedesc1, b.vcalm, b.vcobs, d.vdserv , e.sddesc, b.vcnumi, sum(d.vdcmin) as vdcmin,
			(select  lib.cndesc1  from TC0051 as lib where lib.cncod1=10 and lib.cncod2 =1 and lib.cnnum=b.vctipo ) as tipo
			from  TV002 as b, VR_da_Sectores as c, TV0021 d,TS005  e,TY004  cliente
			where cliente.ydnumi =b.vcclie  and c.cenum=b.vcsector and b.vcnumi=d.vdvc2numi and d.vdserv=e.sdnumi
			and b.vcfdoc  >= @fecha1 and b.vcfdoc  <= @fecha2 and b.vcalm=@almacen  
		and vcsector = @sector
			group by  vcfactura,b.vcfdoc , cliente .yddesc , vctipo, vcsector, cedesc1, vcalm, vcobs, vdserv, sddesc, vcnumi
		

		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end

	IF @tipo=3 --MOSTRAR POR SERVICIO
	BEGIN
		BEGIN TRY
			select b.vcnumi  as fvanfac, FORMAT(b.vcfdoc , 'dd/MM/yyyy') as fvafec,
			(select top 1 cbdol from TC002 where cbfecha=b.vcfdoc ) as tc,
			b.vcfactura as vcfactura,cliente.yddesc as fvadescli1, sum(d.vdtotdesc) as fvasubtotal, sum(d.vddesc) as fvadesc, sum(d.vdtotdesc) as totfact,
			sum(d.vdtotdesc * 0.13) as fvadebfis, --sum(d.vdptot) as fvasubtotal,
			sum((d.vdtotdesc - d.vddesc) - (d.vdtotdesc * 0.13)) as vneta,
			b.vctipo, b.vcsector as cenum,'COFRICO' as cedesc1, b.vcalm, b.vcobs, d.vdserv , e.sddesc, b.vcnumi, sum(d.vdcmin) as vdcmin,
			(select  lib.cndesc1  from TC0051 as lib where lib.cncod1=10 and lib.cncod2 =1 and lib.cnnum=b.vctipo ) as tipo
			from  TV002 as b, VR_da_Sectores as c, TV0021 d,TS005  e,TY004 cliente
			where cliente.ydnumi =b.vcclie  and c.cenum=b.vcsector and b.vcnumi=d.vdvc2numi and d.vdserv=e.sdnumi
			and b.vcfdoc  >= @fecha1 and b.vcfdoc  <= @fecha2 and b.vcalm=@almacen  
			and e.sdnumi = @servicio
			group by cliente.yddesc , vcfactura, b.vcfdoc , vctipo, vcsector, cedesc1, vcalm, vcobs, vdserv, sddesc, vcnumi


		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end
	IF @tipo=4 --MOSTRAR POR SECTOR Y SERVICIO
	BEGIN
		BEGIN TRY
			select b.vcnumi  as fvanfac, FORMAT(b.vcfdoc , 'dd/MM/yyyy') as fvafec,
			(select top 1 cbdol from TC002 where cbfecha=b.vcfdoc ) as tc,
			b.vcfactura as vcfactura,cliente.yddesc as fvadescli1, sum(d.vdtotdesc) as fvasubtotal, sum(d.vddesc) as fvadesc, sum(d.vdtotdesc) as totfact,
			sum(d.vdtotdesc * 0.13) as fvadebfis, --sum(d.vdptot) as fvasubtotal,
			sum((d.vdtotdesc - d.vddesc) - (d.vdtotdesc * 0.13)) as vneta,
			b.vctipo, b.vcsector as cenum,'COFRICO' as cedesc1, b.vcalm, b.vcobs, d.vdserv , e.sddesc, b.vcnumi, sum(d.vdcmin) as vdcmin,
			(select  lib.cndesc1  from TC0051 as lib where lib.cncod1=10 and lib.cncod2 =1 and lib.cnnum=b.vctipo ) as tipo
			from  TV002 as b, VR_da_Sectores as c, TV0021 d,TS005  e,TY004 cliente
			where cliente.ydnumi =b.vcclie  and c.cenum=b.vcsector and b.vcnumi=d.vdvc2numi and d.vdserv=e.sdnumi
			and b.vcfdoc  >= @fecha1 and b.vcfdoc  <= @fecha2 and b.vcalm=@almacen  
			 and e.sdnumi = @servicio 
			Group by b.vcfdoc , vcfactura, cliente.yddesc , vctipo, vcsector, cedesc1, vcalm, vcobs, vdserv, sddesc, vcnumi




		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end

	IF @tipo=5
	BEGIN
		BEGIN TRY
			select sdnumi as ednumi,sddesc as eddesc from TS005 
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end
End