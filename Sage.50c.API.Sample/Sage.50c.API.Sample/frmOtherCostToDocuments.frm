VERSION 5.00
Object = "{49CBFCC0-1337-11D2-9BBF-00A024695830}#1.0#0"; "tinumb8.ocx"
Object = "{BEEECC20-4D5F-4F8B-BFDC-5D9B6FBDE09D}#1.0#0"; "vsFlex8.ocx"
Object = "{0BA686C6-F7D3-101A-993E-0000C0EF6F5E}#1.0#0"; "threed32.ocx"
Object = "{A8E5842E-102B-4289-9D57-3B3F5B5E15D3}#17.3#0"; "Codejock.Controls.v17.3.0.ocx"
Object = "{945E8FCC-830E-45CC-AF00-A012D5AE7451}#17.3#0"; "Codejock.DockingPane.v17.3.0.ocx"
Object = "{7D81BB89-0980-41C2-A648-FC7C9A005C9A}#1.2#0"; "Sage.50c.Menu.18.ocx"
Begin VB.Form frmOtherCostToDocuments 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Repartiçao Outros Custos"
   ClientHeight    =   3945
   ClientLeft      =   2235
   ClientTop       =   1950
   ClientWidth     =   9705
   BeginProperty Font 
      Name            =   "Segoe UI"
      Size            =   9
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmOtherCostToDocuments.frx":0000
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3945
   ScaleWidth      =   9705
   ShowInTaskbar   =   0   'False
   Begin S50cMenu18.MenuBar abMenu 
      Height          =   2385
      Left            =   6960
      TabIndex        =   5
      Top             =   30
      Width           =   1905
      _ExtentX        =   3360
      _ExtentY        =   4207
   End
   Begin VB.PictureBox picData 
      Appearance      =   0  'Flat
      BackColor       =   &H00F6F6F6&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   4005
      Left            =   0
      ScaleHeight     =   4005
      ScaleWidth      =   6855
      TabIndex        =   4
      TabStop         =   0   'False
      Top             =   0
      Width           =   6855
      Begin Threed.SSPanel pnlInstallements 
         Height          =   2595
         Left            =   150
         TabIndex        =   2
         Top             =   1140
         Width           =   5925
         _Version        =   65536
         _ExtentX        =   10451
         _ExtentY        =   4577
         _StockProps     =   15
         BackColor       =   16185078
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "Segoe UI"
            Size            =   96
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         BorderWidth     =   2
         BevelOuter      =   0
         Begin VSFlex8Ctl.VSFlexGrid vsfDocumentList 
            Height          =   2415
            Left            =   60
            TabIndex        =   3
            Top             =   60
            Width           =   5805
            _cx             =   10239
            _cy             =   4260
            Appearance      =   0
            BorderStyle     =   0
            Enabled         =   -1  'True
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "Segoe UI"
               Size            =   9
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            MousePointer    =   0
            BackColor       =   -2147483643
            ForeColor       =   -2147483640
            BackColorFixed  =   -2147483633
            ForeColorFixed  =   -2147483630
            BackColorSel    =   -2147483635
            ForeColorSel    =   -2147483634
            BackColorBkg    =   -2147483643
            BackColorAlternate=   -2147483643
            GridColor       =   -2147483633
            GridColorFixed  =   -2147483632
            TreeColor       =   -2147483632
            FloodColor      =   192
            SheetBorder     =   -2147483642
            FocusRect       =   1
            HighLight       =   1
            AllowSelection  =   0   'False
            AllowBigSelection=   0   'False
            AllowUserResizing=   0
            SelectionMode   =   0
            GridLines       =   1
            GridLinesFixed  =   2
            GridLineWidth   =   1
            Rows            =   1
            Cols            =   6
            FixedRows       =   1
            FixedCols       =   0
            RowHeightMin    =   0
            RowHeightMax    =   0
            ColWidthMin     =   0
            ColWidthMax     =   0
            ExtendLastCol   =   -1  'True
            FormatString    =   $"frmOtherCostToDocuments.frx":000C
            ScrollTrack     =   0   'False
            ScrollBars      =   3
            ScrollTips      =   0   'False
            MergeCells      =   0
            MergeCompare    =   0
            AutoResize      =   0   'False
            AutoSizeMode    =   0
            AutoSearch      =   0
            AutoSearchDelay =   2
            MultiTotals     =   -1  'True
            SubtotalPosition=   1
            OutlineBar      =   0
            OutlineCol      =   0
            Ellipsis        =   0
            ExplorerBar     =   0
            PicturesOver    =   0   'False
            FillStyle       =   0
            RightToLeft     =   0   'False
            PictureType     =   0
            TabBehavior     =   0
            OwnerDraw       =   0
            Editable        =   2
            ShowComboButton =   1
            WordWrap        =   0   'False
            TextStyle       =   0
            TextStyleFixed  =   0
            OleDragMode     =   0
            OleDropMode     =   0
            DataMode        =   0
            VirtualData     =   -1  'True
            DataMember      =   ""
            ComboSearch     =   3
            AutoSizeMouse   =   -1  'True
            FrozenRows      =   0
            FrozenCols      =   0
            AllowUserFreezing=   0
            BackColorFrozen =   0
            ForeColorFrozen =   0
            WallPaperAlignment=   9
            AccessibleName  =   ""
            AccessibleDescription=   ""
            AccessibleValue =   ""
            AccessibleRole  =   24
         End
      End
      Begin Threed.SSPanel pnlTotal 
         Height          =   870
         Left            =   180
         TabIndex        =   0
         Top             =   180
         Width           =   5895
         _Version        =   65536
         _ExtentX        =   10398
         _ExtentY        =   1535
         _StockProps     =   15
         BackColor       =   12040889
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "Segoe UI"
            Size            =   9
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         BorderWidth     =   2
         BevelOuter      =   0
         Font3D          =   3
         Begin TDBNumber6Ctl.TDBNumber numTotalAmount 
            Height          =   810
            Left            =   30
            TabIndex        =   1
            TabStop         =   0   'False
            Top             =   30
            Width           =   5835
            _Version        =   65536
            _ExtentX        =   10292
            _ExtentY        =   1429
            Calculator      =   "frmOtherCostToDocuments.frx":00E5
            Caption         =   "frmOtherCostToDocuments.frx":0105
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "Segoe UI"
               Size            =   27.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            DropDown        =   "frmOtherCostToDocuments.frx":0171
            Keys            =   "frmOtherCostToDocuments.frx":018F
            Spin            =   "frmOtherCostToDocuments.frx":01D9
            AlignHorizontal =   1
            AlignVertical   =   2
            Appearance      =   0
            BackColor       =   16777215
            BorderStyle     =   0
            BtnPositioning  =   0
            ClipMode        =   0
            ClearAction     =   0
            DecimalPoint    =   ","
            DisplayFormat   =   "####0;;Null"
            EditMode        =   0
            Enabled         =   -1
            ErrorBeep       =   0
            ForeColor       =   -2147483640
            Format          =   "####0"
            HighlightText   =   0
            MarginBottom    =   1
            MarginLeft      =   1
            MarginRight     =   1
            MarginTop       =   1
            MaxValue        =   99999
            MinValue        =   -99999
            MousePointer    =   0
            MoveOnLRKey     =   0
            NegativeColor   =   255
            OLEDragMode     =   0
            OLEDropMode     =   0
            ReadOnly        =   -1
            Separator       =   "."
            ShowContextMenu =   -1
            ValueVT         =   1305542661
            Value           =   0
            MaxValueVT      =   977469445
            MinValueVT      =   1465647109
         End
      End
      Begin XtremeSuiteControls.PushButton btnAdd 
         CausesValidation=   0   'False
         Height          =   345
         Left            =   6510
         TabIndex        =   6
         Top             =   1440
         WhatsThisHelpID =   1064
         Width           =   345
         _Version        =   1114115
         _ExtentX        =   609
         _ExtentY        =   609
         _StockProps     =   79
         BackColor       =   5986648
         Appearance      =   2
         Picture         =   "frmOtherCostToDocuments.frx":0201
         IconWidth       =   16
         Icon            =   "frmOtherCostToDocuments.frx":079B
      End
      Begin XtremeSuiteControls.PushButton btnDelete 
         CausesValidation=   0   'False
         Height          =   345
         Left            =   6510
         TabIndex        =   7
         Top             =   1860
         WhatsThisHelpID =   1042
         Width           =   345
         _Version        =   1114115
         _ExtentX        =   609
         _ExtentY        =   609
         _StockProps     =   79
         BackColor       =   5986648
         Appearance      =   2
         Picture         =   "frmOtherCostToDocuments.frx":0C05
         IconWidth       =   16
         Icon            =   "frmOtherCostToDocuments.frx":119F
      End
   End
   Begin XtremeDockingPane.DockingPane DockingPane 
      Left            =   7440
      Top             =   2880
      _Version        =   1114115
      _ExtentX        =   635
      _ExtentY        =   635
      _StockProps     =   0
   End
End
Attribute VB_Name = "frmOtherCostToDocuments"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private strMessageTitle As String

Private oBSOITemTransaction As BSOItemTransaction
Private oDocumentList As SimpleDocumentList

Private dTotalOtherCost As Double
Private dCurrencyID As String
Private dCurrencyExchange As Double
Private dCurrencyFactor As Double
Private objDSODocument As DSODocument
Private objDSODocumentSeries As DSODocumentSeries
Private objDSOItemTransaction As DSOItemTransaction
Private dShareValue As Double

Private Enum OCColPosition
    ocTransSerial = 0
    ocTransDocument = 1
    ocTransDocNumber = 2
    ocTotalPercentage = 3
    ocTotalNetAmount = 4
    ocCurrencyID = 5
    ocCurrencyExchange = 6
    ocCurrencyFactor = 7
    ocShareCostType = 8
End Enum

Private Sub abMenu_ToolClick(ByVal ToolID As String, ByVal ToolOption As String)
    Select Case UCase$(ToolID)
        Case "MISAVE"
            If validate Then
                Finish True
            End If
        Case "MIEXIT"
            Finish False
    End Select
End Sub

Private Sub btnAdd_Click()
    With vsfDocumentList
        If .Rows = 1 Then
            .Rows = 2
            .SetFocus
            .Select 1, 0
            
            .TextMatrix(.Row, ocTransSerial) = oBSOITemTransaction.Transaction.TransSerial
            .TextMatrix(.Row, ocTransDocument) = oBSOITemTransaction.Transaction.TransDocument
            .TextMatrix(vsfDocumentList.Row, ocTotalPercentage) = 100 - GetTotalRate()
            .TextMatrix(vsfDocumentList.Row, ocTotalNetAmount) = dTotalOtherCost - GetTotalAmount()
            .TextMatrix(vsfDocumentList.Row, ocCurrencyID) = dCurrencyID
            .TextMatrix(vsfDocumentList.Row, ocCurrencyExchange) = dCurrencyExchange
            .TextMatrix(vsfDocumentList.Row, ocCurrencyFactor) = dCurrencyFactor
            .TextMatrix(vsfDocumentList.Row, ocShareCostType) = "A"
            
            .Col = 2
            .SetFocus
        Else
          'Bug 15596: Repartição de custos - só deixa acrescentar um documento se a percentagem for inferior a 100% - A validação deveria ser feita apenas no total
          '  If GetTotalAmount - dTotalOtherCost <> 0 Then
                .Rows = .Rows + 1
                .SetFocus
                .Select .Rows - 1, 0
                .TextMatrix(.Row, ocTransSerial) = oBSOITemTransaction.Transaction.TransSerial
                .TextMatrix(.Row, ocTransDocument) = oBSOITemTransaction.Transaction.TransDocument
                If GetTotalRate() <= 100 Then
                    .TextMatrix(vsfDocumentList.Row, ocTotalPercentage) = 100 - GetTotalRate()
                    .TextMatrix(vsfDocumentList.Row, ocTotalNetAmount) = dTotalOtherCost - GetTotalAmount()
                    .TextMatrix(vsfDocumentList.Row, ocCurrencyID) = dCurrencyID
                    .TextMatrix(vsfDocumentList.Row, ocCurrencyExchange) = dCurrencyExchange
                    .TextMatrix(vsfDocumentList.Row, ocCurrencyFactor) = dCurrencyFactor
                    .TextMatrix(vsfDocumentList.Row, ocShareCostType) = "A"
                    
                Else
                    .TextMatrix(vsfDocumentList.Row, ocTotalPercentage) = 0
                    .TextMatrix(vsfDocumentList.Row, ocTotalNetAmount) = 0
                    .TextMatrix(vsfDocumentList.Row, ocCurrencyID) = dCurrencyID
                    .TextMatrix(vsfDocumentList.Row, ocCurrencyExchange) = dCurrencyExchange
                    .TextMatrix(vsfDocumentList.Row, ocCurrencyFactor) = dCurrencyFactor
                    .TextMatrix(vsfDocumentList.Row, ocShareCostType) = "A"
                End If
                
                .Col = 2
                .SetFocus
          '  End If
        End If
    End With
End Sub

Private Sub btnDelete_Click()
    With vsfDocumentList
        If .Row >= 1 Then
            
            DeleteDetails .TextMatrix(.Row, ocTransSerial), .TextMatrix(.Row, ocTransDocument), .ValueMatrix(.Row, ocTransDocNumber)
            
            .RemoveItem .Row
        End If
    End With
End Sub

Private Sub DeleteDetails(ByVal Serial As String, ByVal Document As String, ByVal DocNumber As Double)
    Dim objDocumentDetails As SimpleDocument
    Dim objDocumentoListDetails As SimpleItemDetail
    
    Set objDocumentDetails = New SimpleDocument
    Set objDocumentoListDetails = New SimpleItemDetail
    
    If oDocumentList.IsInCollection(Serial, Document, DocNumber) Then
        oDocumentList(Serial, Document, DocNumber).Details.RemoveAll
    End If
End Sub

Private Sub FindDocument()
    Dim oQuickSearch As QuickSearch
    Dim objDocumentList As SimpleDocumentList
    Dim oParmValues As clsCollection
    Dim dblDocNumber As Double
    
    On Error GoTo errHandler
    Static bIsFinding As Boolean
    
    If Not bIsFinding Then
        bIsFinding = True
    
        'Set objFrmQuickSearch = New frmQuickSearch
        
        Me.Enabled = False
        
        Set oParmValues = New clsCollection
        
        oParmValues.Add vsfDocumentList.TextMatrix(vsfDocumentList.Row, ocTransSerial), "@TransSerial"
        oParmValues.Add vsfDocumentList.TextMatrix(vsfDocumentList.Row, ocTransDocument), "@TransDocument"
        
        Set oQuickSearch = CreateQuickSearch(QSV_BuyTransaction, False)
        Set oQuickSearch.Parameters = oParmValues
        
        'objFrmQuickSearch.setParametersValues oParmValues
        'objFrmQuickSearch.GetIdString enumQuickSearchViews.QSV_BuyTransaction
        
        Me.Enabled = True
        
        If oQuickSearch.SelectValue() Then
            
            dblDocNumber = oQuickSearch.ValueSelectedLong()
            
            Set objDocumentList = GetDocuments(False)
            With vsfDocumentList
                If objDocumentList.IsInCollection(.TextMatrix(.Row, ocTransSerial), .TextMatrix(.Row, ocTransDocument), dblDocNumber) Then
                    MsgBox gLng.GS(3950011), vbInformation, strMessageTitle
                    .EditCell
                ElseIf isSameDocument(dblDocNumber) Then
                    MsgBox gLng.GS(3950010), vbInformation, strMessageTitle
                    .EditCell
                ElseIf HasDocumentShareOtherCost(dblDocNumber) Then
                    MsgBox gLng.GS(3950013), vbInformation, strMessageTitle
                    .EditCell
                Else
                    .TextMatrix(.Row, ocTransDocNumber) = dblDocNumber
                End If
            End With
            Set objDocumentList = Nothing
            
        End If
            
        vsfDocumentList.SetFocus
        
        Set oQuickSearch = Nothing
        Set oParmValues = Nothing

        bIsFinding = False
    End If
    
    Exit Sub

errHandler:
    bIsFinding = False
    Set oParmValues = Nothing
    Set oQuickSearch = Nothing
    Set objDocumentList = Nothing
    
    MsgBox Err.Description, vbInformation, strMessageTitle
    Me.Enabled = True
    
End Sub

Private Sub Translate()
    With gLng
        strMessageTitle = gLng.GS(0, SystemSettings.Application.LongName)
        
        Me.Caption = .GS(3950000)
        
        vsfDocumentList.Cols = 9
        vsfDocumentList.TextMatrix(0, ocTransSerial) = .GS(3950001)
        vsfDocumentList.TextMatrix(0, ocTransDocument) = .GS(3950002)
        vsfDocumentList.TextMatrix(0, ocTransDocNumber) = .GS(3950003)
        vsfDocumentList.TextMatrix(0, ocTotalPercentage) = .GS(3950004)
        vsfDocumentList.TextMatrix(0, ocTotalNetAmount) = .GS(3950005)
        vsfDocumentList.TextMatrix(0, ocShareCostType) = "Tipo" '*lng*
        vsfDocumentList.ColHidden(ocCurrencyID) = True
        vsfDocumentList.ColHidden(ocCurrencyExchange) = True
        vsfDocumentList.ColHidden(ocCurrencyFactor) = True
    End With
End Sub

Private Sub btnDelete_GotFocus()
    'btnDelete.Outline = True
End Sub

Private Sub btnDelete_LostFocus()
    'btnDelete.Outline = False
End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
    If KeyCode = vbKeyEscape Then
        Finish False
    ElseIf KeyCode = vbKeyF10 Then
        KeyCode = 0
        If validate() Then
            Finish True
        End If
    End If
End Sub

Private Sub Form_Load()
    Dim objUIItem As UIHandler
    Set objDSODocument = New DSODocument
    Set objDSODocumentSeries = New DSODocumentSeries
    Set objDSOItemTransaction = New DSOItemTransaction
    
    If SystemSettings.StartUpInfo.UseXPStyle Then
        vsfDocumentList.Appearance = flexXPThemes
    End If
    
    Set objUIItem = New UIHandler
    SystemSettings.Application.UI.UpdateStyles Me
    Set objUIItem = Nothing
    
    CreateMenuBar
    
    SystemSettings.Application.UI.DockingPane.Format DockingPane, picData, abMenu
    
    Translate
    FormatVsf
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
    If UnloadMode = vbFormControlMenu Then
        Cancel = True
        Finish False
    End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Set oBSOITemTransaction = Nothing
    Set oDocumentList = Nothing
    Set objDSODocument = Nothing
    Set objDSODocumentSeries = Nothing
    Set objDSOItemTransaction = Nothing
End Sub

Private Sub vsfDocumentList_AfterEdit(ByVal Row As Long, ByVal Col As Long)
    Dim dblValue As Double
    Dim dblRate As Double
    Dim objDocumentList As SimpleDocumentList
    
    With vsfDocumentList
        If Col = 0 Or Col = 1 Then
            .TextMatrix(Row, 2) = 0
        ElseIf Col = 2 Then
            If vsfDocumentList.ValueMatrix(Row, Col) <> 0 Then
            
                Set objDocumentList = GetDocuments(False)
                With vsfDocumentList
                    If .ValueMatrix(Row, 2) = 0 Then
                        MsgBox gLng.GS(3950008), vbInformation, strMessageTitle
                        .EditCell
                    ElseIf objDocumentList.IsInCollection(.TextMatrix(Row, ocTransSerial), .TextMatrix(Row, ocTransDocument), .ValueMatrix(Row, ocTransDocNumber)) Then
                        MsgBox gLng.GS(3950011), vbInformation, strMessageTitle
                        .EditCell
                    ElseIf Not existDocument(Row) Then
                        MsgBox gLng.GS(3950009), vbInformation, strMessageTitle
                        .EditCell
                    ElseIf isSameDocument(.ValueMatrix(Row, ocTransDocNumber)) Then
                        MsgBox gLng.GS(3950010), vbInformation, strMessageTitle
                        .EditCell
                    ElseIf HasDocumentShareOtherCost(.ValueMatrix(Row, ocTransDocNumber)) Then
                        MsgBox gLng.GS(3950013), vbInformation, strMessageTitle
                        .EditCell
                    End If
                End With
            End If
        ElseIf Col = 3 Then
            dblRate = MyVal(.TextMatrix(Row, ocTotalPercentage), SystemSettings.StartUpInfo.OSDecimalSeparator)
            dblValue = dTotalOtherCost * (dblRate / 100)
            .TextMatrix(Row, ocTotalNetAmount) = dblValue
        ElseIf Col = 4 Then
            dblValue = MyVal(.TextMatrix(Row, ocTotalNetAmount), SystemSettings.StartUpInfo.OSDecimalSeparator)
            If dTotalOtherCost = 0 Then
                dblRate = 0
            Else
                dblRate = (dblValue / dTotalOtherCost) * 100
            End If
            
            .TextMatrix(Row, ocTotalPercentage) = dblRate
        End If
    End With
    
    Set objDocumentList = Nothing
    
End Sub

Private Sub vsfDocumentList_AfterRowColChange(ByVal OldRow As Long, ByVal OldCol As Long, ByVal NewRow As Long, ByVal NewCol As Long)
    If OldCol = 2 Then
'        If Not existDocument(OldRow) Then
'            MsgBox gLnG.GS(3950009), vbInformation, strMessageTitle
'            With vsfDocumentList
'                .Row = OldRow
'                .Col = OldCol
'                .EditCell
'            End With
'        Else
            vsfDocumentList.ColComboList(2) = "..."
'        End If
    End If
End Sub

Private Sub vsfDocumentList_BeforeEdit(ByVal Row As Long, ByVal Col As Long, Cancel As Boolean)
    'Cancel = (Col = 1)
End Sub

Private Sub vsfDocumentList_CellButtonClick(ByVal Row As Long, ByVal Col As Long)
   Dim oCurrentDocumentList As SimpleDocumentList
 
   With vsfDocumentList
        If Col = ocTransDocNumber Then
            
            FindDocument
        
        ElseIf Col = ocTotalPercentage Or Col = ocShareCostType Then
            
            OpenBuyShareOtherCostsDetails
        
        End If
    End With
     
    Set oCurrentDocumentList = Nothing
End Sub

Private Sub vsfDocumentList_DblClick()

    Select Case vsfDocumentList.Col
        
        Case ocTransDocNumber
            
            vsfDocumentList.ColComboList(ocTransDocNumber) = vbNullString
        
        Case ocTotalNetAmount, ocTotalPercentage
            
            OpenBuyShareOtherCostsDetails
      
    End Select
    
End Sub

Private Sub OpenBuyShareOtherCostsDetails()
    Dim oCurrentDocumentList As SimpleDocumentList
    Dim Row As Long
   
    Row = vsfDocumentList.Row
    
    If SystemFeatures.CanRun(featBuySharedCosts) Then
        If CDbl(vsfDocumentList.ValueMatrix(Row, ocTotalNetAmount)) > 0 And CDbl(vsfDocumentList.ValueMatrix(Row, ocTransDocNumber)) > 0 Then
                If Not isLoaded(frmBuyShareOtherCostsDetails) Then
                    Load frmBuyShareOtherCostsDetails
                End If
                      
                Set oCurrentDocumentList = GetDocuments(True)
                Set frmBuyShareOtherCostsDetails.BuyShareOtherCost = oCurrentDocumentList
                
                If frmBuyShareOtherCostsDetails.ShowModal(CDbl(vsfDocumentList.ValueMatrix(vsfDocumentList.RowSel, ocTotalNetAmount)), vsfDocumentList.RowSel) = True Then
                    Set oBSOITemTransaction.Transaction.BuyShareOtherCostList = frmBuyShareOtherCostsDetails.GetBuyShareOtherCostsDocumentList()
                    Set oDocumentList = oBSOITemTransaction.Transaction.BuyShareOtherCostList
                
                    vsfDocumentList.TextMatrix(vsfDocumentList.Row, ocShareCostType) = "M"
                End If
        End If
    End If

    Set oCurrentDocumentList = Nothing

End Sub

Public Property Set BSOItemTransaction(ByVal Value As BSOItemTransaction)
    
    Set oBSOITemTransaction = Value
    Set oDocumentList = oBSOITemTransaction.Transaction.BuyShareOtherCostList
    dTotalOtherCost = oBSOITemTransaction.Transaction.TotalNetAmount
    dCurrencyID = oBSOITemTransaction.Transaction.BaseCurrency.CurrencyId
    dCurrencyExchange = oBSOITemTransaction.Transaction.BaseCurrency.SaleExchange
    dCurrencyFactor = oBSOITemTransaction.Transaction.BaseCurrency.EuroConversionRate
End Property

Private Function Finish(ByVal Status As Boolean)
    
    If Status Then
        Set oDocumentList = GetDocuments(True)
    End If
    
    Me.Hide
    
End Function

Private Function validateDetails(ByVal Serial As String, ByVal Document As String, ByVal DocNumber As Double, ByVal ShareAmount As Double) As Boolean
    Dim objDocumentDetails As SimpleDocument
    Dim objDocumentoListDetails As SimpleItemDetail
    Dim SumUnitPrice As Double
    Dim result As Boolean
    Dim HasDetails As Boolean
    
    Set objDocumentDetails = New SimpleDocument
    Set objDocumentoListDetails = New SimpleItemDetail
    
    If oDocumentList.IsInCollection(Serial, Document, DocNumber) Then
        For Each objDocumentoListDetails In oDocumentList(Serial, Document, DocNumber).Details
            SumUnitPrice = SumUnitPrice + objDocumentoListDetails.UnitPrice
            HasDetails = True
        Next
    End If
    
    If HasDetails = True Then
        result = (Math.Round(SumUnitPrice, 2) = Math.Round(ShareAmount, 2))
     Else
        result = True
    End If
    
    validateDetails = result
End Function

Private Function validate() As Boolean
    Dim dblTotal As Double
    Dim i As Integer
    Dim dblRowValue As Double
    Dim blnReturn As Boolean
    Dim intRound As Integer
    Dim objDocumentList As SimpleDocumentList

    intRound = oBSOITemTransaction.Transaction.BaseCurrency.DecimalPlaces
    
    With vsfDocumentList
        For i = 1 To .Rows - 1
            dblRowValue = MyVal(.TextMatrix(i, ocTotalNetAmount), SystemSettings.StartUpInfo.OSDecimalSeparator)
            If dblRowValue = 0 Then
                MsgBox gLng.GS(3950006), vbInformation, strMessageTitle
                validate = False
                Exit Function
            End If
            dblTotal = dblTotal + dblRowValue
            If .ValueMatrix(i, ocTransDocNumber) = 0 Then
                MsgBox gLng.GS(3950008), vbInformation, strMessageTitle
                .Row = i
                .Col = ocTransDocNumber
                validate = False
                Exit Function
            ElseIf Not existDocument(i) Then
                MsgBox gLng.GS(3950009), vbInformation, strMessageTitle
                .Row = i
                .Col = ocTransDocNumber
                validate = False
                Exit Function
            ElseIf isSameDocument(.ValueMatrix(i, ocTransDocNumber)) Then
                MsgBox gLng.GS(3950010), vbInformation, strMessageTitle
                .Row = i
                .Col = ocTransDocNumber
                validate = False
                Exit Function
            ElseIf HasDocumentShareOtherCost(.ValueMatrix(i, ocTransDocNumber)) Then
                MsgBox gLng.GS(3950013), vbInformation, strMessageTitle
                .Row = i
                .Col = ocTransDocNumber
                validate = False
                Exit Function
            ElseIf Not validateDetails(.TextMatrix(i, ocTransSerial), .TextMatrix(i, ocTransDocument), .ValueMatrix(i, ocTransDocNumber), .ValueMatrix(i, ocTotalNetAmount)) Then
                MsgBox "O valor a repartir do documento não coincide com o total do valor a repartir das linhas.", vbInformation, strMessageTitle '*lng*
                .Row = i
                .Col = ocTotalNetAmount
                validate = False
                Exit Function
            End If
        Next
        If .Rows = .FixedRows Then
            blnReturn = True
        Else
            Set objDocumentList = GetDocuments(True)
            If objDocumentList.Count < .Rows - 1 Then
                MsgBox gLng.GS(3950012), vbInformation, strMessageTitle
                .Row = 1
                .Col = ocTransDocNumber
                validate = False
                Set objDocumentList = Nothing
                Exit Function
            End If
    
            blnReturn = (MyRound(dblTotal, intRound) = MyRound(dTotalOtherCost, intRound))
        End If
    End With
    
    If Not blnReturn Then
        MsgBox gLng.GS(3950007), vbInformation, strMessageTitle
    End If

    validate = blnReturn
    Set objDocumentList = Nothing
End Function

Public Function GetTotalAmount() As Double
    Dim dblTotal As Double
    Dim i As Integer
    
    With vsfDocumentList
        For i = 1 To .Rows - 1
            dblTotal = dblTotal + MyVal(.TextMatrix(i, 4), SystemSettings.StartUpInfo.OSDecimalSeparator)
        Next
    End With
    
    GetTotalAmount = dblTotal
    
End Function

Public Function GetTotalRate() As Double
    Dim dblTotal As Double
    Dim i As Integer
    
    With vsfDocumentList
        For i = 1 To .Rows - 1
            dblTotal = dblTotal + MyVal(.TextMatrix(i, ocTotalPercentage), SystemSettings.StartUpInfo.OSDecimalSeparator)
        Next
    End With
    
    GetTotalRate = dblTotal
    
End Function

Private Function GetDocuments(ByVal bLoadCurrentRow As Boolean) As SimpleDocumentList
    Dim objDocument As SimpleDocument
    Dim objDocumentList As SimpleDocumentList
    Dim objDocumentDetails As SimpleDocument
    Dim objDocumentoListDetails As SimpleItemDetail
    Dim abort As Boolean
    Dim i As Integer
    Dim sDetailKey      As String
    Set objDocumentList = New SimpleDocumentList

    With vsfDocumentList
        For i = 1 To .Rows - 1
            abort = False
            Set objDocument = New SimpleDocument
            objDocument.TransSerial = .TextMatrix(i, ocTransSerial)
            objDocument.TransDocument = .TextMatrix(i, ocTransDocument)
            objDocument.TransDocNumber = .ValueMatrix(i, ocTransDocNumber)
            objDocument.TotalTransactionAmount = MyVal(.ValueMatrix(i, ocTotalNetAmount), SystemSettings.StartUpInfo.OSDecimalSeparator)
            objDocument.CurrencyId = .TextMatrix(i, ocCurrencyID)
            objDocument.CurrencyExchange = MyVal(.ValueMatrix(i, ocCurrencyExchange), SystemSettings.StartUpInfo.OSDecimalSeparator)
            objDocument.CurrencyFactor = MyVal(.ValueMatrix(i, ocCurrencyFactor), SystemSettings.StartUpInfo.OSDecimalSeparator)
            
            Set objDocumentDetails = New SimpleDocument
            Set objDocumentoListDetails = New SimpleItemDetail
                     
            For Each objDocumentDetails In oDocumentList
                For Each objDocumentoListDetails In objDocumentDetails.Details
                    If objDocumentoListDetails.DestinationTransSerial = objDocument.TransSerial And objDocumentoListDetails.DestinationTransDocument = objDocument.TransDocument And objDocumentoListDetails.DestinationTransDocNumber = objDocument.TransDocNumber Then
                        objDocument.Details.Add objDocumentoListDetails
                    End If
                Next
            Next

            If objDocument.TransDocNumber > 0 Then
                If .Row = i Then
                    If bLoadCurrentRow Then objDocumentList.Add objDocument
                Else
                    objDocumentList.Add objDocument
                End If
            End If
        Next
    End With
    
    Set GetDocuments = objDocumentList
    
    Set objDocument = Nothing
    Set objDocumentList = Nothing
    Set objDocumentDetails = Nothing
    Set objDocumentoListDetails = Nothing
End Function

Private Sub vsfDocumentList_GotFocus()
    If vsfDocumentList.Rows = 1 Then
        btnAdd.SetFocus
    End If
End Sub

Private Sub vsfDocumentList_KeyPress(KeyAscii As Integer)
    If vsfDocumentList.Col = ocTransDocNumber Then
        vsfDocumentList.ColComboList(ocTransDocNumber) = vbNullString
    End If
End Sub

Private Sub vsfDocumentList_KeyPressEdit(ByVal Row As Long, ByVal Col As Long, KeyAscii As Integer)
    Select Case KeyAscii
        Case 44
            If Col = 2 Then
                KeyAscii = 0
            End If
        Case 46 'Virgula
            If SystemSettings.StartUpInfo.OSDecimalSeparator = "," Then
                If Col = 2 Then
                    KeyAscii = 0
                Else
                    KeyAscii = 44
                End If
            End If
        Case 49, 50, 51, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57 'Numeros
        Case 45, 43, 44 '- + ,
        Case vbKeyReturn, vbKeyTab, vbKeyEscape, vbKeyBack
        Case Else
            KeyAscii = 0
    End Select
End Sub

Public Sub ShowModal()
    Dim objDocument As SimpleDocument
    Dim dblRate As Double
    Dim ShareTypeCost As String
    CenterForm Me
    
    With numTotalAmount
        .Format = oBSOITemTransaction.Transaction.BaseCurrency.CreateMoneyMask(True)
        .DisplayFormat = .Format
        .maxValue = GetTypeMaxValue(vtTDBNumber)
        .minValue = -1 * GetTypeMaxValue(vtTDBNumber)
        .Value = dTotalOtherCost
    End With
    
    vsfDocumentList.Rows = 1
    
    If Not oDocumentList Is Nothing Then
        For Each objDocument In oDocumentList
            With objDocument
                
                If objDocument.Details.Count > 0 Then
                    ShareTypeCost = "M"
                Else
                    ShareTypeCost = "A"
                End If
                
                dblRate = (.TotalTransactionAmount / dTotalOtherCost) * 100
                If .TransDocNumber > 0 Then
                    vsfDocumentList.AddItem .TransSerial & vbTab & .TransDocument & vbTab & .TransDocNumber & vbTab & dblRate & vbTab & .TotalTransactionAmount & vbTab & .CurrencyId & vbTab & .CurrencyExchange & vbTab & .CurrencyFactor & vbTab & ShareTypeCost
                    vsfDocumentList.RowData(vsfDocumentList.Rows - 1) = oBSOITemTransaction
                End If
            End With
        Next
    End If
    Me.Show vbModal
End Sub

Private Sub FormatVsf()
    Dim rsData As ADODB.Recordset
    Dim oDocument As Document
    
    With vsfDocumentList
        .Cols = 9
        .Rows = 1
        .ColWidth(ocTransSerial) = 750
        .ColWidth(ocTransDocument) = 2200
        .ColWidth(ocTransDocNumber) = 720
        .ColWidth(ocTotalPercentage) = 720
        .ColWidth(ocTotalNetAmount) = 600
        .ColWidth(ocCurrencyID) = 300
        .ColWidth(ocCurrencyExchange) = 300
        .ColWidth(ocCurrencyFactor) = 300
        .ColWidth(ocShareCostType) = 450
                
        .ColAlignment(ocShareCostType) = flexAlignCenterCenter
        
        .ColHidden(ocCurrencyID) = True
        .ColHidden(ocCurrencyExchange) = True
        .ColHidden(ocCurrencyFactor) = True

        .ExtendLastCol = False
        .AllowUserResizing = flexResizeColumns
        
        Set rsData = objDSODocumentSeries.GetDocumentSeriesRS()
        
        .ColComboList(ocTransSerial) = .BuildComboList(rsData, ",*Series", "Series")

        closeRecordSet rsData
        
        Set rsData = objDSODocument.getDocumentsRS(dcTypePurchase, SystemSettings.SystemInfo.DefaultLanguageID)
        
        .ColComboList(ocTransDocument) = .BuildComboList(rsData, "TransDocumentID,*Description", "TransDocumentID")
        
        closeRecordSet rsData

        .ColComboList(ocTransDocNumber) = "..."
        .ColComboList(ocShareCostType) = "..."
    End With
    
    SystemSettings.Application.UI.FlexGrid.SetColors vsfDocumentList
    
    Set rsData = Nothing
    Set oDocument = Nothing
End Sub

Public Function GetOtherCostDocumentList() As SimpleDocumentList
    Set GetOtherCostDocumentList = oDocumentList
End Function

Private Function existDocument(ByVal dblRow As Double) As Boolean
    Dim objDSOItemTransaction As DSOItemTransaction
    Set objDSOItemTransaction = New DSOItemTransaction
    
    With vsfDocumentList
        existDocument = objDSOItemTransaction.ItemTransactionExists(.TextMatrix(dblRow, ocTransSerial), .TextMatrix(dblRow, ocTransDocument), .ValueMatrix(dblRow, ocTransDocNumber))
    End With
    
    Set objDSOItemTransaction = Nothing

End Function

Private Function isSameDocument(ByVal dDocNumber As Double) As Boolean
    On Error GoTo errHandler
    
    isSameDocument = False
    If (vsfDocumentList.TextMatrix(vsfDocumentList.Row, ocTransSerial) = oBSOITemTransaction.Transaction.TransSerial _
       And vsfDocumentList.TextMatrix(vsfDocumentList.Row, ocTransDocument) = oBSOITemTransaction.Transaction.TransDocument _
       And dDocNumber = oBSOITemTransaction.Transaction.TransDocNumber) Then

        isSameDocument = True
    End If
    
    Exit Function

errHandler:
    isSameDocument = False
    MsgBox Err.Description, vbInformation, strMessageTitle
    
End Function

Private Function HasDocumentShareOtherCost(ByVal dDocNumber As Double) As Boolean
    HasDocumentShareOtherCost = objDSOItemTransaction.DocumentHasShareOtherCost(oBSOITemTransaction.Transaction.TransSerial, _
        oBSOITemTransaction.Transaction.TransDocument, _
        oBSOITemTransaction.Transaction.TransDocNumber, _
        vsfDocumentList.TextMatrix(vsfDocumentList.Row, ocTransSerial), _
        vsfDocumentList.TextMatrix(vsfDocumentList.Row, ocTransDocument), _
        dDocNumber)
End Function

Private Sub CreateMenuBar()
    Dim oMenuItem As MenuBarItem
    
    Set oMenuItem = abMenu.Items.Add("miSave", gLng.GS(2160002), btnActionPrimary)
    oMenuItem.ToolKeyCode = SystemSettings.Application.ShortcutKeys("SAVE").KeyCode
    oMenuItem.ToolKeyShift = SystemSettings.Application.ShortcutKeys("SAVE").KeyShift
    
    Set oMenuItem = abMenu.Items.Add("miExit", gLng.GS(2160003), btnActionTertiary)
    oMenuItem.ToolKeyCode = vbKeyEscape
    
    Set oMenuItem = Nothing
End Sub
