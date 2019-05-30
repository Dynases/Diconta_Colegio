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
Imports CrystalDecisions.Shared
Imports Datos
Imports Facturacion

Public Class F0_CargaVentasManuales
    Dim namearchivo As String = ""
    Public Sub _Limpiar()
        Dim dt As DataTable = L_fnGeneralFormatoMigracion()
        dt.Rows().Clear()
        _prCargarGrilla(dt)
    End Sub
    Sub _prCargarArchivo()

        Dim dt As DataTable = L_fnGeneralFormatoMigracion()
        dt.Rows().Clear()
        Dim codigoestudiante As Integer
        Dim estudiante As String = ""
        Dim servicio As Integer
        Dim MontoDeposito As Double
        Dim fechadeposito As String
        Dim nrocuota As Integer
        Dim nrodocumento As String
        Dim codigocontrol As String
        Dim codigobanco As Integer
        Dim esfactura As Boolean = False
        Dim nameServicio As String = ""
        Dim openFD As New OpenFileDialog()
        With openFD
            .Title = "Seleccionar archivos para Migrar"
            .Filter = "Todos los archivos (*.*)|*.*"
            .Multiselect = False
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                namearchivo = .SafeFileName
                Dim Archivo() As String = IO.File.ReadAllLines(.FileName)
                For i As Integer = 0 To Archivo.Length - 1 Step 1
                    esfactura = False
                    Try
                        estudiante = ""
                        codigobanco = Archivo(i).Substring(0, Archivo(i).IndexOf(",")).Trim
                        codigoestudiante = Archivo(i).Split(",")(2).Trim
                        ''estudiante = Archivo(i).Split(",")(3).Trim
                        servicio = Archivo(i).Split(",")(4).Trim
                        MontoDeposito = Archivo(i).Split(",")(5).Trim
                        fechadeposito = Archivo(i).Split(",")(6).Trim
                        nrocuota = Archivo(i).Split(",")(7).Trim
                        nrodocumento = Archivo(i).Split(",")(8).Trim
                        codigocontrol = Archivo(i).Split(",")(9).Trim

                        If (codigocontrol <> String.Empty) Then
                            esfactura = True
                        End If
                        '                  Select Case 0 As codigobanco,0 As codigoestudiante,'' as estudiante,
                        '      0 as codigoservicio,'' as servicio,cast(0 as decimal(18,2)) as montodeposito,
                        'cast('05-06-2019' as date) as fechadeposito,0 as nrocuota, cast(0 as bit ) as esfactura
                        '      ,'' as nrodocumento,'' as codigocontrol, 0 as estado
                        Dim dt2 As DataTable = L_fnObtenerServicio(servicio)
                        If (dt2.Rows.Count > 0) Then
                            nameServicio = dt2.Rows(0).Item(0)
                        End If
                        Dim dtalumno As DataTable = L_fnGeneralobtenerNAmeAlumno(codigoestudiante)
                        If (dtalumno.Rows.Count > 0) Then
                            estudiante = dtalumno.Rows(0).Item(0)
                            dt.Rows.Add(codigobanco, codigoestudiante, estudiante, servicio, nameServicio, MontoDeposito, fechadeposito, nrocuota, esfactura, nrodocumento, codigocontrol, 0)
                        End If


                    Catch ex As Exception
                        Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
                        ToastNotification.Show(Me, "EL formato del archivo seleccionado no es el correcto. Por favor seleccione otro archivo".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
                        Return

                    End Try

                Next



            End If
        End With
        _prCargarGrilla(dt)
    End Sub

    Private Sub _prCargarGrilla(dt As DataTable)

        grMigracion.DataSource = dt
        grMigracion.RetrieveStructure()
        grMigracion.AlternatingColors = True
        '                  Select Case 0 As codigobanco,0 As codigoestudiante,'' as estudiante,
        '      0 as codigoservicio,'' as servicio,cast(0 as decimal(18,2)) as montodeposito,
        'cast('05-06-2019' as date) as fechadeposito,0 as nrocuota, cast(0 as bit ) as esfactura
        '      ,'' as nrodocumento,'' as codigocontrol, 0 as estado
        With grMigracion.RootTable.Columns("codigobanco")
            .Width = 100
            .Caption = "Cod Banco"
            .Visible = True
            .TextAlignment = TextAlignment.Far
        End With
        With grMigracion.RootTable.Columns("codigoestudiante")
            .Width = 100
            .Visible = True
            .Caption = "Cod Estudiante"
        End With
        With grMigracion.RootTable.Columns("estudiante")
            .Width = 300
            .Visible = True
            .Caption = "Estudiante"
        End With
        With grMigracion.RootTable.Columns("codigoservicio")
            .Width = 100
            .Visible = True
            .Caption = "Cod Servicio"
        End With
        With grMigracion.RootTable.Columns("servicio")
            .Width = 200
            .Visible = True
            .Caption = "Servicio"
        End With
        With grMigracion.RootTable.Columns("montodeposito")
            .Width = 120
            .Visible = True
            .Caption = "Monto Deposito"
            .FormatString = "0.00"
        End With
        With grMigracion.RootTable.Columns("fechadeposito")
            .Width = 100
            .Caption = "Fecha Deposito"
            .FormatString = "dd/MM/yyyy"
            .Visible = True
        End With
        With grMigracion.RootTable.Columns("nrocuota")
            .Width = 100
            .Caption = "Nro Cuota"
            .Visible = True
        End With
        With grMigracion.RootTable.Columns("esfactura")
            .Width = 90
            .Visible = True
            .Caption = "Es Factura?"
        End With

        With grMigracion.RootTable.Columns("nrodocumento")
            .Width = 100
            .Visible = True
            .Caption = "Factura/Recibo"
        End With




        With grMigracion.RootTable.Columns("codigocontrol")
            .Width = 120
            .Caption = "Cod Control"
            .Visible = True
        End With

        With grMigracion.RootTable.Columns("estado")
            .Width = 90
            .Visible = False
        End With
        With grMigracion
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .GroupByBoxVisible = False

            'diseño de la grilla

        End With
        LblPaginacion.Text = Str(dt.Rows.Count) + " Datos a Migrar"
    End Sub
    Private Sub btCargarArchivo_Click(sender As Object, e As EventArgs) Handles btCargarArchivo.Click
        _prCargarArchivo()
    End Sub

    Private Sub btCargarDatos_Click(sender As Object, e As EventArgs) Handles btCargarDatos.Click
        Dim numi As String = ""
        Dim bandera As Boolean = False
        Try
            Dim i As Integer = CType(grMigracion.DataSource, DataTable).Rows.Count

        Catch ex As Exception
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me, "No existen Datos para ser migrados".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            Return
        End Try

        If (CType(grMigracion.DataSource, DataTable).Rows.Count <= 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me, "No existen Datos para ser migrados".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            Return
        End If
        Dim dt As DataTable = L_fnGeneralobtenerventas(namearchivo)
        If (dt.Rows.Count > 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me, "El archivo : ".ToUpper + namearchivo + " Ya ha sido migrado, por favor seleccione otro archivo de migración", img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            Return
        End If
        Dim res As Boolean = L_fnGrabarMigracion(CType(grMigracion.DataSource, DataTable), namearchivo)


        If res Then

            Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
            ToastNotification.Show(Me, "La migracion fue realizado exitosamente".ToUpper,
                                      img, 2000,
                                      eToastGlowColor.Green,
                                      eToastPosition.TopCenter
                                      )


            _Limpiar()

        Else

            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me, "La Venta no pudo ser registrado por que existen datos invalidos.".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)



        End If
    End Sub

    Private Sub btSalir_Click(sender As Object, e As EventArgs) Handles btSalir.Click
        Me.Close()

    End Sub

    Private Sub F0_CargaVentasManuales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "MIGRAR VENTAS"
    End Sub
End Class