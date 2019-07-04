
/****** Object:  StoredProcedure [dbo].[sp_da_TV0021]    Script Date: 04/07/2019 5:48:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[sp_da_TV0021] (@tipo int, @fec1 date = null, @fec2 date = null, @almacen int = -1, @sector int = -1,@servicio int = -1,@uact nvarchar(10)='')
as
begin 
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))

	DECLARE @newFecha date
	set @newFecha=GETDATE()


	IF @tipo=1 --MOSTRAR TODOS
	BEGIN
		BEGIN TRY
			select distinct tce4.sdcod  as  edcod, tce4.sdnumi as ednumi, tce4.sddesc as eddesc,
			(select sum(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 
		) as cant,

			(select SUM(vddesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ) as tdesc, 

			IIF(tv2.vcmoneda =1,(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as punit,

			IIF (tv2.vcmoneda=1,(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvbrt, 

			IIF (tv2.vcmoneda =1,(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0),(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0)*6.96) as timp,

			IIF (tv2.vcmoneda =1,(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvn,

			 0 as costuni, 0 as tcos, tv2.vcalm, tv2.vcsector, vr.cedesc1
			from TV0021 tv21, TS005 tce4, TV002 tv2, VR_da_Sectores vr
			where tv21.vdserv=tce4.sdnumi  and tv21.vdvc2numi=tv2.vcnumi and tv2.vcsector=vr.cenum and tv2.vcsector <> -10 and
			tv2.vcalm=@almacen and tv2.vcfdoc >= @fec1 and tv2.vcfdoc <=@fec2
			and tv2.vcest=0 

		
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end

	IF @tipo=2 --MOSTRAR SECTOR
	BEGIN
		BEGIN TRY
				select distinct tce4.sdcod  as  edcod, tce4.sdnumi as ednumi, tce4.sddesc as eddesc,
			(select sum(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 
		) as cant,

			(select SUM(vddesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ) as tdesc, 

			IIF(tv2.vcmoneda =1,(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as punit,

			IIF (tv2.vcmoneda=1,(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvbrt, 

			IIF (tv2.vcmoneda =1,(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0),(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0)*6.96) as timp,

			IIF (tv2.vcmoneda =1,(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvn,
			 0 as costuni, 0 as tcos, tv2.vcalm, vr.cenum, vr.cedesc1
			from TV0021 tv21, TS005 tce4, TV002 tv2, VR_da_Sectores vr
			where tv21.vdserv=tce4.sdnumi and tv21.vdvc2numi=tv2.vcnumi and tv2.vcsector=vr.cenum and
			tv2.vcalm=@almacen and tv2.vcfdoc >= @fec1 and tv2.vcfdoc <=@fec2 and tv2.vcsector=@sector
			and tv2.vcest=0 
		
		

		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end


	IF @tipo=4 --MOSTRAR SERVICIO
	BEGIN
		BEGIN TRY
				select distinct tce4.sdcod  as  edcod, tce4.sdnumi as ednumi, tce4.sddesc as eddesc,
			(select sum(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 
		) as cant,

			(select SUM(vddesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ) as tdesc, 

			IIF(tv2.vcmoneda =1,(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as punit,

			IIF (tv2.vcmoneda=1,(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvbrt, 

			IIF (tv2.vcmoneda =1,(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0),(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0)*6.96) as timp,

			IIF (tv2.vcmoneda =1,(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvn,

			 0 as costuni, 0 as tcos, tv2.vcalm, tv2.vcsector, vr.cedesc1
			from TV0021 tv21, TS005 tce4, TV002 tv2, VR_da_Sectores vr
			where tv21.vdserv=tce4.sdnumi and tv21.vdvc2numi=tv2.vcnumi and tv2.vcsector=vr.cenum and
			tv2.vcalm=@almacen and tv2.vcfdoc >= @fec1 and tv2.vcfdoc <=@fec2 and tce4.sdnumi = @servicio
			and tv2.vcest=0
		
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end
	IF @tipo=5 --MOSTRAR SECTOR Y SERVICIO
	BEGIN
		BEGIN TRY
				select distinct tce4.sdcod  as  edcod, tce4.sdnumi as ednumi, tce4.sddesc as eddesc,
			(select sum(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 
		) as cant,

			(select SUM(vddesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ) as tdesc, 

			IIF(tv2.vcmoneda =1,(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdpbas)/count(vdcmin) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as punit,

			IIF (tv2.vcmoneda=1,(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select SUM(vdtotdesc) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvbrt, 

			IIF (tv2.vcmoneda =1,(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0),(select (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0)*6.96) as timp,

			IIF (tv2.vcmoneda =1,(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 ),(select (SUM(vdtotdesc)) - (sum(vdtotdesc) * 0.13) from TV002 x, TV0021 y  where x.vcnumi=y.vdvc2numi and y.vdserv=tce4.sdnumi and 
			x.vcalm=@almacen and x.vcfdoc >=@fec1 and x.vcfdoc <=@fec2 and x.vcest=0 )*6.96) as tvn,

			 0 as costuni, 0 as tcos, tv2.vcalm, vr.cenum, vr.cedesc1
			from TV0021 tv21, TS005 tce4, TV002 tv2, VR_da_Sectores vr
			where tv21.vdserv=tce4.sdnumi and tv21.vdvc2numi=tv2.vcnumi and tv2.vcsector=vr.cenum and
			tv2.vcalm=@almacen and tv2.vcfdoc >= @fec1 and tv2.vcfdoc <=@fec2 and tv2.vcsector=@sector and tce4.sdnumi=@servicio
			and tv2.vcest=0 
		

			
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),3,@newFecha,@newHora,@uact)
		END CATCH
	end
end


