Option Strict Off
Imports Logica.AccesoLogica
Imports Janus.Windows.GridEX
Imports DevComponents.DotNetBar
Imports System.IO
Imports DevComponents.DotNetBar.SuperGrid
Imports System.Drawing
Imports DevComponents.DotNetBar.Controls
Imports System.Threading
Imports System.Drawing.Text
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing
Imports System.Math

Public Class F1_AsientosContables

#Region "Variables Globales"
    Dim RutaGlobal As String = gs_CarpetaRaiz
    Dim RutaTemporal As String = "C:\Temporal"
    Dim Modificado As Boolean = False
    Dim nameImg As String = "Default.jpg"
    Public _nameButton As String
    Public _tab As SuperTabItem
    Public _modulo As SideNavItem
    Dim NumiCertificacion As Integer = 6
    Public _ListVentas As DataTable
    Dim NumiAdministracion As Integer = 7

    Dim conRedondeo As Boolean = False


#Region "Variables Multiproposito"
    '    VariableMultiproposito
    Dim Lavadero As Integer = 42
    Dim Remolque As Integer = 55
    Dim Administracion As Integer = 56
    Dim Escuela As Integer = 1
    Dim Certificacion As Integer = 43
    Dim Hotel As Integer = 44
    Dim Linea As Integer = 0
#End Region

#End Region
    'L_prIntegracionBancos
#Region "Metodos SobreEscritos"


    Private Sub _prCargarMovimiento()
        Dim dt As New DataTable
        dt = L_prIntegracionGeneral()
        grmovimientos.DataSource = dt
        grmovimientos.RetrieveStructure()
        grmovimientos.AlternatingColors = True

        'a.ifnumi ,a.ifto001numi,comprobante .oanumdoc  ,a.iftc ,a.iffechai ,a.iffechaf ,a.ifest 
        With grmovimientos.RootTable.Columns("ifnumi")
            .Width = 100
            .Caption = "CODIGO"
            .Visible = True

        End With

        With grmovimientos.RootTable.Columns("ifto001numi")
            .Width = 100
            .Visible = True
            .Caption = "COD COMPROBANTE"
        End With
        With grmovimientos.RootTable.Columns("oanumdoc")
            .Width = 110
            .Visible = True
            .Caption = "NRO DOCUMENTO"
        End With

        With grmovimientos.RootTable.Columns("iftc")
            .Width = 110
            .Visible = True
            .Caption = "TIPO DE CAMBIO"
        End With
        With grmovimientos.RootTable.Columns("iffechai")
            .Width = 110
            .Visible = True
            .Caption = "FECHA I".ToUpper
            .FormatString = "dd/MM/yyyy"
        End With
        With grmovimientos.RootTable.Columns("iffechaf")
            .Width = 110
            .Visible = True
            .Caption = "FECHA F".ToUpper
            .FormatString = "dd/MM/yyyy"
        End With

        With grmovimientos.RootTable.Columns("ifest")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With

        Dim dtSuc As DataTable
        dtSuc = L_fnListarAlmacenDosificacion()

        With grmovimientos.RootTable.Columns("ifsuc")
            .Width = 200
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Caption = "SUCURSAL"

            .HasValueList = True
            'Set EditType to Combo or DropDownList.
            'In a MultipleValues Column, the dropdown will appear with a CheckBox
            'at the left of each item to let the user select multiple items
            .EditType = EditType.DropDownList
            .ValueList.PopulateValueList(dtSuc.DefaultView, "cod", "desc")
            .CompareTarget = ColumnCompareTarget.Text
            .DefaultGroupInterval = GroupInterval.Text
            .AllowSort = False

        End With
        With grmovimientos
            .DefaultFilterRowComparison = FilterConditionOperator.Equal
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
        End With

        If (dt.Rows.Count <= 0) Then
            L_prIntegracionDetalle(-1)
        End If
    End Sub

    Public Sub _prArmarDetalleDt(ByRef dt As DataTable)
        Dim tabla As DataTable = dt.Copy
        tabla.Rows.Clear()
        Dim cuenta As Integer = 0
        For i As Integer = 0 To dt.Rows.Count - 1 Step 1
            Dim aux As Integer = dt.Rows(i).Item("canumi")
            If (aux <> cuenta) Then
                Linea = Linea + 1
                Dim dtObtenerCuenta As DataTable = L_prCuentaDiferencia(aux)  ''
                tabla.Rows.Add(dtObtenerCuenta.Rows(0).Item("canumi"), dtObtenerCuenta.Rows(0).Item("cacta"), dtObtenerCuenta.Rows(0).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, Administracion, 0)

                tabla.Rows.Add(dtObtenerCuenta.Rows(1).Item("canumi"), dtObtenerCuenta.Rows(1).Item("cacta"), dtObtenerCuenta.Rows(1).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, Administracion, 0)

                Dim debe As Double = IIf(IsDBNull(dt.Rows(i).Item("debe")), 0, dt.Rows(i).Item("debe"))
                Dim haber As Double = IIf(IsDBNull(dt.Rows(i).Item("haber")), 0, dt.Rows(i).Item("haber"))
                Dim debeSus As Double = IIf(IsDBNull(dt.Rows(i).Item("debesus")), 0, dt.Rows(i).Item("debesus"))
                Dim haberSus As Double = IIf(IsDBNull(dt.Rows(i).Item("habersus")), 0, dt.Rows(i).Item("habersus"))
                If (debe = 0 And haber = 0 And debeSus = 0 And haberSus = 0) Then
                    tabla.Rows.Add(dtObtenerCuenta.Rows(1).Item("canumi"), DBNull.Value, dt.Rows(i).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, 0, 0)

                Else
                    tabla.Rows.Add(dt.Rows(i).Item("canumi"), DBNull.Value, dt.Rows(i).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, dt.Rows(i).Item("tc"), dt.Rows(i).Item("debe"), dt.Rows(i).Item("haber"), dt.Rows(i).Item("debesus"), dt.Rows(i).Item("habersus"), dt.Rows(i).Item("variable"), Linea)
                End If



            Else
                Linea = Linea + 1
                '             cuenta.canumi , cuenta.cacta As nro, detalle.obobs, 0 As chporcen, 0 As chdebe , 0 As chhaber, i.iftc  as tc
                ',detalle .obdebebs  as debe,obhaberbs  as haber,obdebeus  as debesus
                ',obhaberus  as habersus,obaux1  as variable,oblin  as linea

                Dim debe As Integer = IIf(IsDBNull(dt.Rows(i).Item("debe")), 0, dt.Rows(i).Item("debe"))
                Dim haber As Integer = IIf(IsDBNull(dt.Rows(i).Item("haber")), 0, dt.Rows(i).Item("haber"))

                If (debe = 0 And haber = 0) Then
                    tabla.Rows.Add(dt.Rows(i).Item("canumi"), DBNull.Value, dt.Rows(i).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, 0, 0)

                Else
                    tabla.Rows.Add(dt.Rows(i).Item("canumi"), DBNull.Value, dt.Rows(i).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, dt.Rows(i).Item("tc"), dt.Rows(i).Item("debe"), dt.Rows(i).Item("haber"), dt.Rows(i).Item("debesus"), dt.Rows(i).Item("habersus"), dt.Rows(i).Item("variable"), Linea)
                End If

            End If
            cuenta = aux
        Next
        dt = tabla
    End Sub
    Private Sub _prCargarDetalleMovimiento(_numi As String)
        Dim dt As New DataTable
        dt = L_prIntegracionDetalle(_numi)
        _prArmarDetalleDt(dt)
        grComprobante.DataSource = dt
        grComprobante.RetrieveStructure()
        grComprobante.AlternatingColors = True
        ' a.icid ,a.icibid ,a.iccprod ,b.cicdprod1  as producto,a.iccant ,
        'a.icsector  ,Cast(null as image ) as img,1 as estado,
        '(Sum(inv.iccven)  +a.iccant ) as stock

        With grComprobante.RootTable.Columns("canumi")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("variable")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("linea")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("nro")
            .Width = 120
            .Caption = "NRO CUENTA"
            .Visible = True
        End With
        With grComprobante.RootTable.Columns("cadesc")
            .Width = 500
            .Caption = "DESCRIPCION"
            .Visible = True
        End With
        With grComprobante.RootTable.Columns("chporcen")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("chdebe")
            .Width = 180
            .Caption = "DEBE"
            .Visible = False
        End With
        With grComprobante.RootTable.Columns("chhaber")
            .Width = 180
            .Caption = "HABER"
            .Visible = False
        End With
        With grComprobante.RootTable.Columns("tc")
            .Width = 70
            .Caption = "TC"
            .Visible = True
            .FormatString = "0.00"
        End With
        With grComprobante.RootTable.Columns("debe")
            .Width = 100
            .Caption = "DEBE BS"
            .Visible = True
            .TextAlignment = TextAlignment.Far
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .AggregateFunction = AggregateFunction.Sum

        End With
        With grComprobante.RootTable.Columns("haber")
            .Width = 100
            .Caption = "HABER BS"
            .Visible = True
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .TextAlignment = TextAlignment.Far
            .AggregateFunction = AggregateFunction.Sum

        End With

        With grComprobante.RootTable.Columns("debesus")
            .Width = 100
            .Caption = "DEBE SUS"
            .Visible = True
            .TextAlignment = TextAlignment.Far
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .AggregateFunction = AggregateFunction.Sum

        End With
        With grComprobante.RootTable.Columns("habersus")
            .Width = 100
            .Caption = "HABER SUS"
            .Visible = True
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .TextAlignment = TextAlignment.Far
            .AggregateFunction = AggregateFunction.Sum

        End With
        With grComprobante
            .TotalRowFormatStyle.BackColor = Color.Gold
            .TotalRowPosition = TotalRowPosition.BottomFixed
            .TotalRow = InheritableBoolean.True

            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
        End With
        With grComprobante.RootTable.Columns("chmoneda")
            .Width = 100

            .Visible = False
        End With
        _prAplicarCondiccionJanus()
    End Sub


#End Region
#Region "METODOS PRIVADOS"

    Private Sub _IniciarTodo()
        ''L_prAbrirConexion(gs_Ip, gs_UsuarioSql, gs_ClaveSql, gs_NombreBD)

        'SuperTabItemBuscador.Visible = False
        MSuperTabControl.SelectedTabIndex = 0
        Me.WindowState = FormWindowState.Maximized
        Me.Text = "COMPROBANTES DE SERVICIOS"
        Dim blah As New Bitmap(New Bitmap(My.Resources.compra), 20, 20)
        Dim ico As Icon = Icon.FromHandle(blah.GetHicon())
        Me.Icon = ico
        _prAsignarPermisos()
        _prCargarMovimiento()
        _prInhabiliitar()
    End Sub




    Public Sub _prArmarCuadre(ByRef dt As DataTable)
        Try
            Dim totaldebe As Double = dt.Compute("Sum(debe)", "")
            Dim totalhaber As Double = dt.Compute("Sum(haber)", "")
            Dim totaldebesus As Double = dt.Compute("Sum(debesus)", "")
            Dim totalhabersus As Double = dt.Compute("Sum(habersus)", "")
            Dim restantedebe As Double = 0
            Dim restanteHaber As Double = 0
            Dim RestanteDebeSus As Double = 0
            Dim RestanteHaberSus As Double = 0
            If (totaldebe > totalhaber) Then
                restanteHaber = totaldebe - totalhaber
            Else
                restantedebe = totalhaber - totaldebe
            End If
            If (totaldebesus > totalhabersus) Then
                RestanteHaberSus = totaldebesus - totalhabersus
            Else
                RestanteDebeSus = totalhabersus - totaldebesus
            End If
            If (restantedebe > 0 Or restanteHaber > 0 Or RestanteDebeSus > 0 Or RestanteHaberSus > 0) Then
                Dim dtObtenerCuenta As DataTable = L_prCuentaDiferencia(652)  '''3=Lavadero
                dt.Rows.Add(dtObtenerCuenta.Rows(0).Item("canumi"), dtObtenerCuenta.Rows(0).Item("cacta"), dtObtenerCuenta.Rows(0).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, Administracion, 0)
                dt.Rows.Add(dtObtenerCuenta.Rows(1).Item("canumi"), dtObtenerCuenta.Rows(1).Item("cacta"), dtObtenerCuenta.Rows(1).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, Administracion, 0)
                Linea = Linea + 1
                dt.Rows.Add(dtObtenerCuenta.Rows(1).Item("canumi"), DBNull.Value, "Ajuste de Cambio", DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, IIf(restantedebe = 0, DBNull.Value, restantedebe), IIf(restanteHaber = 0, DBNull.Value, restanteHaber), IIf(RestanteDebeSus = 0, DBNull.Value, RestanteDebeSus), IIf(RestanteHaberSus = 0, DBNull.Value, RestanteHaberSus), Administracion, Linea)

                conRedondeo = True
            Else
                conRedondeo = False

            End If
        Catch ex As Exception

        End Try



    End Sub




    Public Sub _prArmarEscuela(ByRef dt As DataTable)
        Dim dtServicioTotal As DataTable = L_prServicioObtenerTotalServiciosLavadero(1, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1)  '''3=Lavadero
        If (dtServicioTotal.Rows(0).Item("total") > 0) Then
            Dim dtCuentaPadres As DataTable = L_prListarCuentasServicioGeneral(gi_empresaNumi, 1, 1)
            Dim numeroCuenta As Integer = 0
            For j As Integer = 0 To dtCuentaPadres.Rows.Count - 1 Step 1
                Dim numiCuenta As Integer = dtCuentaPadres.Rows(j).Item("canumi")
                dt.Rows.Add(numiCuenta, dtCuentaPadres.Rows(j).Item("cacta"), dtCuentaPadres.Rows(j).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, Escuela, 0)

                Dim dtCuentaServ As DataTable = L_prServicioListarCuentasServicioGeneral(dtCuentaPadres.Rows(j).Item("cacta"), gi_empresaNumi, 1, 1)
                For i As Integer = 0 To dtCuentaServ.Rows.Count - 1 Step 1
                    Dim numiCuentaHijo As Integer = dtCuentaServ.Rows(i).Item("canumi")
                    Dim dttotc As DataTable
                    If (dtCuentaServ.Rows(i).Item("seest") = 1) Then

                        dttotc = L_prObtenerTotalPorCuenta(dtCuentaServ.Rows(i).Item("senrocuenta"), dtCuentaServ.Rows(i).Item("seref"), tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1)
                        If (dttotc.Rows(0).Item("total") > 0) Then
                            Dim dtnombre As DataTable = L_prObtenerNombreCuenta(dtCuentaServ.Rows(i).Item("senrocuenta"))
                            If (dtnombre.Rows.Count > 0) Then

                                If (numeroCuenta <> dtCuentaServ.Rows(i).Item("senrocuenta")) Then
                                    Dim totalDescuento As Double = (dttotc.Rows(0).Item("total"))

                                    dt.Rows.Add(numiCuentaHijo, dtCuentaServ.Rows(i).Item("senrocuenta"), dtnombre.Rows(0).Item("cadesc"), DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, Escuela, 0)
                                    numeroCuenta = dtCuentaServ.Rows(i).Item("senrocuenta")
                                    Linea = Linea + 1
                                    Dim totales As Double = Round(to3Decimales(totalDescuento), 2)
                                    Dim TotalSus As Double = Round(to3Decimales(totales / (tbTipoCambio.Value)), 2)

                                    Dim cantidad As Integer = dttotc.Rows(0).Item("cantidad")
                                    dt.Rows.Add(numiCuentaHijo, DBNull.Value, "POR INGRESO DE " + Str(cantidad) + " " + dtCuentaServ.Rows(i).Item("seref") + " DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, DBNull.Value, totales, DBNull.Value, TotalSus, Escuela, Linea)

                                Else

                                    Dim totalDescuento As Double = (dttotc.Rows(0).Item("total"))
                                    Linea = Linea + 1
                                    Dim totales As Double = Round(to3Decimales(totalDescuento), 2)
                                    Dim TotalSus As Double = Round(to3Decimales(totales / (tbTipoCambio.Value)), 2)

                                    Dim cantidad As Integer = dttotc.Rows(0).Item("cantidad")
                                    dt.Rows.Add(numiCuentaHijo, DBNull.Value, "POR INGRESO DE " + Str(cantidad) + " " + dtCuentaServ.Rows(i).Item("seref") + " DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, DBNull.Value, totales, DBNull.Value, TotalSus, Escuela, Linea)
                                End If



                            End If


                        End If

                    End If
                Next
            Next

        End If

    End Sub

    Sub _prCargarNumiVentas(_categoria As Integer)

        Dim dtServicioTotal As DataTable = L_prServicioObtenerNumiPorCategoria(_categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1)
        If (dtServicioTotal.Rows.Count > 0) Then
            _ListVentas.Merge(dtServicioTotal)
        End If


    End Sub

    Private Sub _prCargarTablaComprobantes()
        Dim k As Integer
        Dim dt As New DataTable
        dt = L_prServicioListarCuentas()  ''Ok
        Dim tabla As DataTable = dt.Copy
        tabla.Rows.Clear()
        Dim dtServicios As New DataTable
        dtServicios = L_prlistarCategoriasActivos() ''Ok
        Dim BanderaCuentaPorCobrar As Boolean = False
        Dim contador As Integer = 0 ''Contador para sacar los numi de las ventas 

        tbFechaF.Value = tbFechaI.Value

        For i As Integer = 0 To dt.Rows.Count - 1

            ''canumi , nro,cadesc ,chporcen,chdebe ,chhaber 
            Dim porcentaje As Double = dt.Rows(i).Item("chporcen")

            Dim dtcc = L_prServicioBuscarPadreCuenta(dt.Rows(i).Item("canumi"))
            If (dtcc.Rows.Count > 0) Then
                tabla.ImportRow(dtcc.Rows(0))
            End If
            tabla.ImportRow(dt.Rows(i))
            Dim numiCuenta As Integer = dt.Rows(i).Item("canumi")
            Dim moneda As Integer = dt.Rows(i).Item("chmoneda")
            contador += 1
            Dim numicuentaatc As Integer = 21 'ATC
            Dim Numicuentatran As Integer

            If (porcentaje > 0 And dt.Rows(i).Item("chdebe")) Then

                For j As Integer = 0 To dtServicios.Rows.Count - 1 Step 1
                    Dim categoria As Integer = dtServicios.Rows(j).Item("cenum")
                    Dim dtServicioTotal As DataTable
                    If porcentaje = 3 Then 'porcentaje = 3 CODIGO DANNY PARA SEPARAR TOTALES PARA IT
                        dtServicioTotal = L_prServicioObtenerTotalPorCategoriaSoloFacturale(categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1)  ''Ok
                    Else
                        dtServicioTotal = L_prServicioObtenerTotalPorCategoriaTodosConRecibo(categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1, moneda)  ''Ok
                    End If
                    If (dtServicioTotal.Rows.Count > 0) Then
                        If (contador = 1) Then
                            _prCargarNumiVentas(categoria)
                        End If
                        Dim total As Double = dtServicioTotal.Rows(0).Item("total")
                        If (total > 0) Then
                            If (porcentaje = 3) Then

                                If (categoria = 1) Then  ''''Servicios Escuela 
                                    Dim conversion As Double = (total * (porcentaje / 100))
                                    conversion = to3Decimales(conversion)
                                    Dim totales As Double = Round(conversion, 2)
                                    Dim TotalSus As Double = Round(to3Decimales(totales / (tbTipoCambio.Value)), 2)
                                    Linea = Linea + 1
                                    tabla.Rows.Add(numiCuenta, DBNull.Value, "IT " + dtServicios.Rows(j).Item("cedesc1") + " DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, totales, DBNull.Value, TotalSus, DBNull.Value, Escuela, Linea)
                                End If

                            Else
                                BanderaCuentaPorCobrar = True
                                Dim dtserviciostotalcuentacobrar As DataTable 'Dim dtServiciosTotalCuentaCobrar = L_prServicioObtenerTotalPorCategoriaCuentasCobrar(1, categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1)
                                'total = dtserviciostotalcuentacobrar.Rows(0).Item("total")

                                If (categoria = 1) Then ''''''''Escuela
                                    dtserviciostotalcuentacobrar = L_prServicioObtenerTotalPorCategoriaCuentasCobrar(1, categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1, moneda)
                                    total = dtserviciostotalcuentacobrar.Rows(0).Item("total")
                                    ' totalus = dtserviciostotalcuentacobrar.Rows(0).Item("totalus")
                                    If total > 0 Then  'Efectivo

                                        Linea = Linea + 1
                                        Dim conversion As Double = (total * (porcentaje / 100))
                                        conversion = to3Decimales(conversion)
                                        Dim totales, totalsus As Double
                                        If moneda = 1 Then
                                            totales = Round(conversion, 2)
                                            totalsus = Round(to3Decimales(totales / (tbTipoCambio.Value)), 2)
                                        Else
                                            totalsus = Round(conversion, 2)
                                            totales = Round(to3Decimales(totalsus * (tbTipoCambio.Value)), 2)
                                        End If
                                        '''''Variable Multiproposito de Lavadero
                                        tabla.Rows.Add(numiCuenta, DBNull.Value, "POR INGRESO DE " + dtServicios.Rows(j).Item("cedesc1") + " DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, totales, DBNull.Value, TotalSus, DBNull.Value, Escuela, Linea)

                                    End If

                                    'If totalus > 0 Then  'Efectivo

                                    '    Linea = Linea + 1
                                    '    Dim conversion As Double = (total * (porcentaje / 100))
                                    '    conversion = to3Decimales(conversion)
                                    '    Dim totales As Double = Round(conversion * (tbTipoCambio.Value), 2)
                                    '    Dim TotalSus As Double = Round(conversion, 2)
                                    '    '''''Variable Multiproposito de Lavadero
                                    '    tabla.Rows.Add(numiCuenta, DBNull.Value, "POR INGRESO DE " + dtServicios.Rows(j).Item("cedesc1") + " DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, totales, DBNull.Value, TotalSus, DBNull.Value, Escuela, Linea)

                                    'End If
                                    dtserviciostotalcuentacobrar = L_prServicioObtenerTotalPorCategoriaCuentasCobrar(3, categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1, moneda)
                                    total = dtserviciostotalcuentacobrar.Rows(0).Item("total")

                                    If total > 0 Then  'ATC

                                        Linea = Linea + 1
                                        Dim conversion As Double = (total * (porcentaje / 100))
                                        conversion = to3Decimales(conversion)
                                        Dim totales As Double = Round(conversion, 2)
                                        Dim TotalSus As Double = Round(to3Decimales(totales / (tbTipoCambio.Value)), 2)
                                        '''''Variable Multiproposito de Lavadero
                                        tabla.Rows.Add(numicuentaatc, 11110301, "POR INGRESO ATC DE " + dtServicios.Rows(j).Item("cedesc1") + " DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, totales, DBNull.Value, TotalSus, DBNull.Value, Escuela, Linea)
                                    End If

                                    dtserviciostotalcuentacobrar = L_prServicioObtenerTotalPorCategoriaCuentasCobrartra(4, categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1)
                                    total = dtserviciostotalcuentacobrar.Rows.Count

                                    If total > 0 Then  'TRANSFERENCIA
                                        For k = 0 To dtserviciostotalcuentacobrar.Rows.Count - 1
                                            total = dtserviciostotalcuentacobrar.Rows(k).Item("total")
                                            numiCuenta = dtserviciostotalcuentacobrar.Rows(k).Item("canumi")
                                            Numicuentatran = dtserviciostotalcuentacobrar.Rows(k).Item("cacuenta")
                                            Linea = Linea + 1
                                            Dim conversion As Double = (total * (porcentaje / 100))
                                            conversion = to3Decimales(conversion)
                                            Dim totales As Double = Round(conversion, 2)
                                            Dim TotalSus As Double = Round(to3Decimales(totales / (tbTipoCambio.Value)), 2)
                                            '''''Variable Multiproposito de Lavadero
                                            tabla.Rows.Add(numiCuenta, Numicuentatran, "POR INGRESO TRASPASO DE " + dtServicios.Rows(j).Item("cedesc1") + " DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, totales, DBNull.Value, TotalSus, DBNull.Value, Escuela, Linea)
                                        Next
                                    End If

                                End If

                                '

                            End If

                        End If
                    End If


                Next

                btnGrabar.Enabled = True



            End If
            If (porcentaje > 0 And dt.Rows(i).Item("chhaber")) Then
                Dim SubTotal As Double = 0
                For j As Integer = 0 To dtServicios.Rows.Count - 1 Step 1
                    Dim categoria As Integer = dtServicios.Rows(j).Item("cenum")
                    Dim dtServicioTotal As DataTable = L_prServicioObtenerTotalPorCategoriaSoloFacturale(categoria, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1)
                    Dim total As Double = dtServicioTotal.Rows(0).Item("total")


                    SubTotal = SubTotal + (total * (porcentaje / 100))



                Next
                If (porcentaje = 13) Then
                    Linea = Linea + 1
                    Dim totales As Double = Round(to3Decimales(SubTotal), 2)
                    Dim TotalSus As Double = Round((totales / (tbTipoCambio.Value)), 2)

                    tabla.Rows.Add(numiCuenta, DBNull.Value, "IVA DF INGRESOS DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, DBNull.Value, totales, DBNull.Value, TotalSus, Administracion, Linea)
                Else
                    Linea = Linea + 1
                    Dim totales As Double = Round(to3Decimales(SubTotal), 2)
                    Dim TotalSus As Double = Round((totales / (tbTipoCambio.Value)), 2)
                    tabla.Rows.Add(numiCuenta, DBNull.Value, "IT POR PAGAR INGRESOS DEL " + tbFechaI.Value.ToString("dd/MM/yyyy") + " AL " + tbFechaF.Value.ToString("dd/MM/yyyy"), DBNull.Value, DBNull.Value, DBNull.Value, tbTipoCambio.Value, DBNull.Value, totales, DBNull.Value, TotalSus, Administracion, Linea)
                End If

            End If


        Next

        '_prArmarSocios(tabla)
        '_prArmarLavadero(tabla)
        '_prArmarRemolque(tabla)
        _prArmarEscuela(tabla)
        '_prArmarCertificacion(tabla)
        '_prArmarCabanas(tabla)
        '_prArmarAdministracionn(tabla)
        _prArmarCuadre(tabla)

        ''canumi , nro, cadesc, chporcen, chdebe, chhaber 
        grComprobante.DataSource = tabla
        grComprobante.RetrieveStructure()


        Dim dtt As DataTable = _ListVentas

        With grComprobante.RootTable.Columns("canumi")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("variable")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("linea")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("nro")
            .Width = 120
            .Caption = "NRO CUENTA"
            .Visible = True
        End With
        With grComprobante.RootTable.Columns("cadesc")
            .Width = 580
            .Caption = "DESCRIPCION"
            .Visible = True
        End With
        With grComprobante.RootTable.Columns("chporcen")
            .Width = 100

            .Visible = False
        End With
        With grComprobante.RootTable.Columns("chdebe")
            .Width = 180
            .Caption = "DEBE"
            .Visible = False
        End With
        With grComprobante.RootTable.Columns("chhaber")
            .Width = 180
            .Caption = "HABER"
            .Visible = False
        End With
        With grComprobante.RootTable.Columns("tc")
            .Width = 70
            .Caption = "TC"
            .Visible = True
            .FormatString = "0.00"
        End With
        With grComprobante.RootTable.Columns("debe")
            .Width = 100
            .Caption = "DEBE BS"
            .Visible = True
            .TextAlignment = TextAlignment.Far
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .AggregateFunction = AggregateFunction.Sum

        End With
        With grComprobante.RootTable.Columns("haber")
            .Width = 100
            .Caption = "HABER BS"
            .Visible = True
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .TextAlignment = TextAlignment.Far
            .AggregateFunction = AggregateFunction.Sum

        End With

        With grComprobante.RootTable.Columns("debesus")
            .Width = 100
            .Caption = "DEBE SUS"
            .Visible = True
            .TextAlignment = TextAlignment.Far
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .AggregateFunction = AggregateFunction.Sum

        End With
        With grComprobante.RootTable.Columns("habersus")
            .Width = 100
            .Caption = "HABER SUS"
            .Visible = True
            .FormatString = "0.00"
            .TotalFormatString = "0.00"
            .TextAlignment = TextAlignment.Far
            .AggregateFunction = AggregateFunction.Sum

        End With
        With grComprobante
            .TotalRowFormatStyle.BackColor = Color.Gold
            .TotalRowPosition = TotalRowPosition.BottomFixed
            .TotalRow = InheritableBoolean.True

            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
        End With
        With grComprobante.RootTable.Columns("chmoneda")
            .Width = 100

            .Visible = False
        End With
        Dim aux As DataTable = CType(grComprobante.DataSource, DataTable)

        'Lb_efec.Text = IIf(IsDBNull(aux.Compute("Sum(debe)", "")), 0, aux.Compute("Sum(debe)", ""))
        'Lb_Saldo.Text = IIf(IsDBNull(aux.Compute("Sum(debe)", "")), 0, aux.Compute("Sum(debe)", ""))
        _prAplicarCondiccionJanus()
    End Sub

    Public Sub _prAplicarCondiccionJanus()





        Dim fc As GridEXFormatCondition
        fc = New GridEXFormatCondition(grComprobante.RootTable.Columns("tc"), ConditionOperator.Equal, DBNull.Value)

        fc.FormatStyle.FontBold = TriState.True
        fc.FormatStyle.FontSize = 9
        fc.FormatStyle.FontUnderline = TriState.True

        grComprobante.RootTable.FormatConditions.Add(fc)

    End Sub


    Private Sub _prAsignarPermisos()

        Dim dtRolUsu As DataTable = L_prRolDetalleGeneral(gi_userRol, _nameButton)

        Dim show As Boolean = dtRolUsu.Rows(0).Item("ycshow")
        Dim add As Boolean = dtRolUsu.Rows(0).Item("ycadd")
        Dim modif As Boolean = dtRolUsu.Rows(0).Item("ycmod")
        Dim del As Boolean = dtRolUsu.Rows(0).Item("ycdel")

        If add = False Then
            btnNuevo.Visible = False
        End If
        If modif = False Then
            btnModificar.Visible = False
        End If
        If del = False Then
            btnEliminar.Visible = False
        End If
    End Sub
    Private Sub _prInhabiliitar()
        tbNumi.ReadOnly = True


        tbTipoCambio.IsInputReadOnly = True
        btnModificar.Enabled = True
        btnGrabar.Enabled = False
        btnNuevo.Enabled = True
        btnEliminar.Enabled = True
        PanelNavegacion.Enabled = True
        btnNuevoTipoCambio.Visible = False

        conRedondeo = False

    End Sub
    Private Sub _prhabilitar()
        ''  tbCliente.ReadOnly = False  por que solo podra seleccionar Cliente
        ''  tbVendedor.ReadOnly = False
        'btnGrabar.Enabled = True

        tbNumi.ReadOnly = True
        btActualizar.Visible = True
        btnNuevoTipoCambio.Visible = True
        tbTipoCambio.IsInputReadOnly = False
        tbNumi.Clear()

    End Sub

    Private Sub F1_ServicioCuentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _IniciarTodo()
    End Sub



    Public Function _fnVisualizarRegistros() As Boolean
        Return btnGrabar.Enabled = True
    End Function
    Public Function _ValidarCampos() As Boolean
        If (tbTipoCambio.Value <= 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "Por Favor Coloque un Tipo de Cambio Valido".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            tbTipoCambio.Focus()
            Return False

        End If

        If (grComprobante.RowCount <= 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "No Existen Datos Para Guardar".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            grComprobante.Focus()
            Return False
        End If


        If conRedondeo = True Then
            'validar que el redondeo no sea mas alto que el permitido by DANNY
            'verificar si esta cuadrado
            'cargar datos globales
            grComprobante.MoveLast()
            Dim redonDebeBs As Double = IIf(IsDBNull(grComprobante.GetValue("debe")) = True, 0, grComprobante.GetValue("debe"))
            Dim redonDebeSus As Double = IIf(IsDBNull(grComprobante.GetValue("debesus")) = True, 0, grComprobante.GetValue("debesus"))
            Dim redonHaberBs As Double = IIf(IsDBNull(grComprobante.GetValue("haber")) = True, 0, grComprobante.GetValue("haber"))
            Dim redonHaberSus As Double = IIf(IsDBNull(grComprobante.GetValue("habersus")) = True, 0, grComprobante.GetValue("habersus"))


            If redonDebeBs > 0 Or redonDebeSus > 0 Or redonHaberBs > 0 Or redonHaberSus > 0 Then
                Dim dtGlobal As DataTable = L_prConfigGeneralEmpresa(1)
                Dim _difMaximaAjuste As Double
                If dtGlobal.Rows.Count > 0 Then
                    _difMaximaAjuste = dtGlobal.Rows(0).Item("cfdifmax")
                End If

                'Dim diferenciaBs As Double

                Dim diferencia As Double = redonDebeSus - redonHaberSus
                'verifico si la diferencia es para el debe
                If diferencia < 0 Then 'si es negativo la diferencia es para el haber
                    diferencia = diferencia * -1
                End If


                If diferencia > _difMaximaAjuste Then 'si es verdadero,pregunto sin desea hacer el ajuste automatico
                    Dim info As New TaskDialogInfo("advertencia".ToUpper, eTaskDialogIcon.Exclamation, "ajuste de cambio mayor al permitido, ¿desea grabar de todos modos?".ToUpper, "".ToUpper, eTaskDialogButton.Yes Or eTaskDialogButton.No, eTaskDialogBackgroundColor.Blue)
                    Dim result As eTaskDialogResult = TaskDialog.Show(info)
                    If result = eTaskDialogResult.Yes Then

                    Else
                        Return False

                    End If

                    'ToastNotification.Show(Me, "No se puede grabar la integracion por que el ajuste de cambio es mayor al permitido".ToUpper, My.Resources.WARNING, 3000, eToastGlowColor.Blue, eToastPosition.TopCenter)

                End If

            End If


        End If




        Return True
    End Function

    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click

        If _ValidarCampos() = False Then
            Exit Sub
        End If

        If (tbNumi.Text = String.Empty) Then
            _prGuardarModificado()

        Else
            'If (tbCodigo.Text <> String.Empty) Then
            '    _prGuardarModificado()
            '    ''    _prInhabiliitar()

            'End If
        End If

        ''    _prInhabiliitar()
    End Sub
    Private Function to3Decimales(num As Double) As Double
        Dim res As Double = 0
        Dim numeroString As String = num.ToString()
        Dim posicionPuntoDecimal As Integer = numeroString.IndexOf(".")

        If posicionPuntoDecimal > 0 Then
            Dim cantidadDecimales As Integer = numeroString.Substring(posicionPuntoDecimal).Count - 1
            If cantidadDecimales >= 3 Then
                numeroString = numeroString.Substring(0, posicionPuntoDecimal + 4)
                res = Convert.ToDouble(numeroString)
            Else
                res = num
            End If

        Else
            res = num
        End If
        Return res
    End Function
    Private Sub _prGuardarModificado()
        's
        'L_prComprobanteGrabarIntegracion(ByRef _numi As String, _numDoc As String, _tipo As String, _anio As String, _mes As String, _num As String, _fecha As String, _tipoCambio As String, _glosa As String, _obs As String, _numiEmpresa As String, _detalle As DataTable, _ifnumi As String, _ifto001numi As Integer, _iftc As Double,
        '                                                    _iffechai As String, _iffechaf As String, _ifest As Integer

        'codigo danny*****************************
        Dim dtDetalle As DataTable = CType(grComprobante.DataSource, DataTable)

        Dim Reg As DataRow


        For Each fila As DataRow In dtDetalle.Rows
            If IsDBNull(fila.Item("debe")) = True Then
                fila.Item("debe") = 0
            End If
            If IsDBNull(fila.Item("haber")) = True Then
                fila.Item("haber") = 0
            End If
            If IsDBNull(fila.Item("debesus")) = True Then
                fila.Item("debesus") = 0
            End If
            If IsDBNull(fila.Item("habersus")) = True Then
                fila.Item("habersus") = 0
            End If
        Next
        '******************************************
        Dim numiComprobante As String = ""
        Dim res As Boolean = L_prComprobanteGrabarIntegracion(numiComprobante, "", 1, tbFechaI.Value.Year.ToString, tbFechaI.Value.Month.ToString, "", tbFechaI.Value.Date.ToString("yyyy/MM/dd"), tbTipoCambio.Value.ToString, "", "", gi_empresaNumi, dtDetalle, "", 0, tbTipoCambio.Value, tbFechaI.Value.ToString("yyyy/MM/dd"), tbFechaF.Value.ToString("yyyy/MM/dd"), 1, _ListVentas, 1)
        If res Then
            Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
            ToastNotification.Show(Me, "El Asiento Contable fue generado Exitosamente".ToUpper,
                                      img, 2000,
                                      eToastGlowColor.Green,
                                      eToastPosition.TopCenter
                                      )
            '' _prCargarTablaServicios()

            'imprimir el reporte de comprobante
            _prImprimirComprobante(numiComprobante)

            _prCargarMovimiento()


            _prInhabiliitar()
            If grmovimientos.RowCount > 0 Then

                _prMostrarRegistro(0)

            End If

        Else
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me, "Los codigos no pudieron ser modificados".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)

        End If
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        btnNuevo.Enabled = False
        btnModificar.Enabled = False
        btnGrabar.Enabled = True
        btnEliminar.Enabled = False

    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        _modulo.Select()
        _tab.Close()
    End Sub
    Public Sub _prMostrarRegistro(_N As Integer)
        '' a.ifnumi , a.ifto001numi, comprobante.oanumdoc, a.iftc, a.iffechai, a.iffechaf, a.ifest 
        With grmovimientos
            tbNumi.Text = .GetValue("ifnumi")
            tbFechaI.Value = .GetValue("iffechai")
            tbFechaF.Value = .GetValue("iffechaf")
            tbTipoCambio.Value = .GetValue("iftc")

        End With

        _prCargarDetalleMovimiento(tbNumi.Text)

        LblPaginacion.Text = Str(grmovimientos.Row + 1) + "/" + grmovimientos.RowCount.ToString

    End Sub

    Private Sub _prSalir()
        If btnGrabar.Enabled = False Then
            _prInhabiliitar()
            If grmovimientos.RowCount > 0 Then

                _prMostrarRegistro(0)

            End If
        Else

            _modulo.Select()
            _tab.Close()

        End If
    End Sub

    Sub _prLimpiar()
        conRedondeo = False
        tbTipoCambio.Value = 0
        tbFechaI.Value = Now.Date
        tbFechaF.Value = Now.Date
        If (Not IsDBNull(grComprobante)) Then
            If (grComprobante.RowCount > 0) Then
                CType(grComprobante.DataSource, DataTable).Rows.Clear()
            End If

        End If
        _prCrearColumns()

    End Sub
    Sub _prCrearColumns()
        If (Not IsNothing(_ListVentas)) Then
            _ListVentas.Columns.Clear()
        End If
        _ListVentas = New DataTable
        _ListVentas.Columns.Add("vcnumi", Type.GetType("System.Int32"))

    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _prhabilitar()
        _prLimpiar()

        btnNuevo.Enabled = False
        btnModificar.Enabled = False
        btnGrabar.Enabled = True
        btnEliminar.Enabled = False

        Dim dtTipoCambio As DataTable = L_prTipoCambioGeneralPorFecha(Now.ToString("yyyy/MM/dd"))
        If dtTipoCambio.Rows.Count = 0 Then
            _existTipoCambio = False
            tbTipoCambio.Value = 0
            tbTipoCambio.BackgroundStyle.BackColor = Color.Red
            btnNuevoTipoCambio.Visible = True
        Else
            _existTipoCambio = True
            tbTipoCambio.Value = dtTipoCambio.Rows(0).Item("cbdol")
            tbTipoCambio.BackgroundStyle.BackColor = Color.White
            MEP.SetError(tbTipoCambio, "")
            btnNuevoTipoCambio.Visible = False

        End If


    End Sub

    Private Sub btActualizar_Click(sender As Object, e As EventArgs) Handles btActualizar.Click

        If (IsNothing(tbTipoCambio.Value) Or tbTipoCambio.ToString = String.Empty Or tbTipoCambio.Value = 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "NO HAY TIPO DE CAMBIO REGISTRADO. POR FAVOR REGISTRE EL TIPO DE CAMBIO".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)

            Return

        End If

        _prCrearColumns()

        _prCargarTablaComprobantes()

    End Sub

    Private Sub grComprobante_KeyDown(sender As Object, e As KeyEventArgs) Handles grComprobante.KeyDown
        If (e.KeyData = Keys.Enter) Then
            If (GrDatos.Visible = True) Then
                GrDatos.Visible = False

                Panel2.Visible = False
            Else
                GrDatos.Visible = True

                Panel2.Visible = True
            End If
        End If
    End Sub

    Private Sub btnNuevoTipoCambio_Click(sender As Object, e As EventArgs)
        _prAñadirTipoCambio()
    End Sub
    Private Sub _prAñadirTipoCambio()
        Dim dtRolUsu As DataTable = L_prRolDetalleGeneral(gi_userRol, "btConfTipoCambio")

        Dim add As Boolean = dtRolUsu.Rows(0).Item("ycadd")

        If add = True Then
            Dim frm As New F0_TipoCambio_Nuevo
            frm.tbFecha.Value = tbFechaI.Value
            frm.ShowDialog()
            tbFechaI.Value = DateAdd(DateInterval.Day, -1, tbFechaI.Value)
            tbFechaI.Value = DateAdd(DateInterval.Day, 1, tbFechaI.Value)
        Else
            ToastNotification.Show(Me, "el usario no cuenta con los permisos para adicionar tipo de cambio".ToUpper, My.Resources.WARNING, 3000, eToastGlowColor.Blue, eToastPosition.TopCenter)
        End If

    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        _prImprimir()
    End Sub
    Private Sub _prImprimir()
        Dim objrep As New R_ComprobanteIntegracion
        Dim dt As New DataTable
        dt = CType(grComprobante.DataSource, DataTable)

        'ahora lo mando al visualizador
        P_Global.Visualizador = New Visualizador
        objrep.SetDataSource(dt)
        objrep.SetParameterValue("fecha", tbFechaI.Value.ToString("dd/MM/yyyy"))
        objrep.SetParameterValue("tc", tbTipoCambio.Value)
        objrep.SetParameterValue("titulo", "COMPROBANTE DE INGRESO")
        objrep.SetParameterValue("titulo2", "COLEGIO ANGLO AMERICANO ") '+ gs_empresaDesc.ToUpper)
        objrep.SetParameterValue("glosa", "Sucursal Principal")
        'cargar el numero de comprobante
        Dim dtNum As DataTable = L_prObtenerNumFacturaGeneral(1, tbFechaI.Value.Year, tbFechaI.Value.Month, 1)
        If dtNum.Rows.Count > 0 Then
            objrep.SetParameterValue("numero", dtNum.Rows(0).Item("oanumdoc").ToString)
        Else
            objrep.SetParameterValue("numero", "")

        End If

        objrep.SetParameterValue("nit", "12345678") ' gs_empresaNit.ToUpper)


        P_Global.Visualizador.CRV1.ReportSource = objrep 'Comentar
        P_Global.Visualizador.Show() 'Comentar
        P_Global.Visualizador.BringToFront() 'Comentar

    End Sub

    Private Sub _prImprimirComprobante(numiComprobante)
        Dim objrep As New R_Comprobante
        Dim dt As New DataTable
        dt = L_prComprobanteReporteComprobante(numiComprobante)

        'ahora lo mando al visualizador
        P_Global.Visualizador = New Visualizador
        objrep.SetDataSource(dt)
        objrep.SetParameterValue("fechaDesde", "")
        objrep.SetParameterValue("fechaHasta", "")
        objrep.SetParameterValue("titulo", "Colegio Anglo Americas")
        objrep.SetParameterValue("nit", "")
        objrep.SetParameterValue("ultimoRegistro", 0)

        P_Global.Visualizador.CRV1.ReportSource = objrep 'Comentar
        P_Global.Visualizador.Show() 'Comentar
        P_Global.Visualizador.BringToFront() 'Comentar

    End Sub
    Public Sub _PrimerRegistro()
        Dim _MPos As Integer
        If grmovimientos.RowCount > 0 Then
            _MPos = 0
            ''   _prMostrarRegistro(_MPos)
            grmovimientos.Row = _MPos
        End If
    End Sub

    Private Sub grmovimientos_SelectionChanged(sender As Object, e As EventArgs) Handles grmovimientos.SelectionChanged
        If (grmovimientos.RowCount >= 0 And grmovimientos.Row >= 0) Then

            _prMostrarRegistro(grmovimientos.Row)
        End If
    End Sub

    Private Sub btnSiguiente_Click(sender As Object, e As EventArgs) Handles btnSiguiente.Click
        Dim _pos As Integer = grmovimientos.Row
        If _pos < grmovimientos.RowCount - 1 Then
            _pos = grmovimientos.Row + 1
            '' _prMostrarRegistro(_pos)
            grmovimientos.Row = _pos
        End If
    End Sub

    Private Sub btnPrimero_Click(sender As Object, e As EventArgs) Handles btnPrimero.Click
        _PrimerRegistro()
    End Sub

    Private Sub btnUltimo_Click(sender As Object, e As EventArgs) Handles btnUltimo.Click
        Dim _pos As Integer = grmovimientos.Row
        If grmovimientos.RowCount > 0 Then
            _pos = grmovimientos.RowCount - 1
            ''  _prMostrarRegistro(_pos)
            grmovimientos.Row = _pos
        End If
    End Sub

    Private Sub btnAnterior_Click(sender As Object, e As EventArgs) Handles btnAnterior.Click
        Dim _MPos As Integer = grmovimientos.Row
        If _MPos > 0 And grmovimientos.RowCount > 0 Then
            _MPos = _MPos - 1
            ''  _prMostrarRegistro(_MPos)
            grmovimientos.Row = _MPos
        End If
    End Sub

    Private Sub btnNuevoTipoCambio_Click_1(sender As Object, e As EventArgs) Handles btnNuevoTipoCambio.Click
        _prAñadirTipoCambio()
    End Sub

    Private Sub tbFechaI_ValueChanged(sender As Object, e As EventArgs) Handles tbFechaI.ValueChanged
        'verifico el tipo de cambio de la fecha elegida
        Dim dtTipoCambio As DataTable = L_prTipoCambioGeneralPorFecha(tbFechaI.Value.ToString("yyyy/MM/dd"))
        If dtTipoCambio.Rows.Count = 0 Then
            '_existTipoCambio = False
            tbTipoCambio.Value = Nothing
            tbTipoCambio.Text = ""
            tbTipoCambio.BackgroundStyle.BackColor = Color.Red
            btnNuevoTipoCambio.Visible = True
        Else
            '_existTipoCambio = True
            tbTipoCambio.Value = dtTipoCambio.Rows(0).Item("cbdol")
            tbTipoCambio.BackgroundStyle.BackColor = Color.White
            btnNuevoTipoCambio.Visible = False
            MEP.SetError(tbTipoCambio, "")


        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim ef = New Efecto
        ef.tipo = 2
        ef.Context = "¿esta seguro de eliminar el asiento contable?".ToUpper
        ef.Header = "mensaje principal".ToUpper
        ef.ShowDialog()
        Dim bandera As Boolean = False
        bandera = ef.band
        If (bandera = True) Then
            Dim mensajeError As String = ""
            Dim res As Boolean = L_fnEliminarAsientoContable(tbNumi.Text, mensajeError)
            If res Then


                Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)

                ToastNotification.Show(Me, "Código de Venta ".ToUpper + tbNumi.Text + " eliminado con Exito.".ToUpper,
                                          img, 2000,
                                          eToastGlowColor.Green,
                                          eToastPosition.TopCenter)

                _prCargarMovimiento()


                _prInhabiliitar()
                If grmovimientos.RowCount > 0 Then

                    _prMostrarRegistro(0)

                End If


            Else
                Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
                ToastNotification.Show(Me, mensajeError, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            End If
        End If

    End Sub



    Private Sub GrDatos_Click(sender As Object, e As EventArgs) Handles GrDatos.Click

    End Sub


#End Region


End Class