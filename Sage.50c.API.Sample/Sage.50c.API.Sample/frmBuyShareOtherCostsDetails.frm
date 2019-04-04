VERSION 5.00
Object = "{49CBFCC0-1337-11D2-9BBF-00A024695830}#1.0#0"; "tinumb8.ocx"
Object = "{BEEECC20-4D5F-4F8B-BFDC-5D9B6FBDE09D}#1.0#0"; "vsFlex8.ocx"
Object = "{0BA686C6-F7D3-101A-993E-0000C0EF6F5E}#1.0#0"; "threed32.ocx"
Object = "{945E8FCC-830E-45CC-AF00-A012D5AE7451}#17.3#0"; "Codejock.DockingPane.v17.3.0.ocx"
Object = "{7D81BB89-0980-41C2-A648-FC7C9A005C9A}#1.2#0"; "Sage.50c.Menu.18.ocx"
Begin VB.Form frmBuyShareOtherCostsDetails 
   Caption         =   "Form1"
   ClientHeight    =   8295
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   11940
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   8.25
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   ScaleHeight     =   8295
   ScaleWidth      =   11940
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox picData 
      Appearance      =   0  'Flat
      AutoSize        =   -1  'True
      BackColor       =   &H00FFFFFF&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   8205
      Left            =   0
      ScaleHeight     =   8205
      ScaleWidth      =   9855
      TabIndex        =   1
      TabStop         =   0   'False
      Top             =   60
      Width           =   9855
      Begin Threed.SSPanel pnlInstallements 
         Height          =   5955
         Left            =   0
         TabIndex        =   2
         Top             =   1800
         Width           =   9765
         _Version        =   65536
         _ExtentX        =   17224
         _ExtentY        =   10504
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
            Height          =   5775
            Left            =   0
            TabIndex        =   3
            Top             =   0
            Width           =   9645
            _cx             =   17013
            _cy             =   10186
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
            Cols            =   10
            FixedRows       =   1
            FixedCols       =   0
            RowHeightMin    =   0
            RowHeightMax    =   0
            ColWidthMin     =   0
            ColWidthMax     =   0
            ExtendLastCol   =   -1  'True
            FormatString    =   $"frmBuyShareOtherCostsDetails.frx":0000
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
         Left            =   0
         TabIndex        =   4
         Top             =   180
         Width           =   9855
         _Version        =   65536
         _ExtentX        =   17383
         _ExtentY        =   1535
         _StockProps     =   15
         BackColor       =   12040889
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "Segoe UI"
            Size            =   9.01
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
            TabIndex        =   5
            TabStop         =   0   'False
            Top             =   30
            Width           =   9795
            _Version        =   65536
            _ExtentX        =   17277
            _ExtentY        =   1429
            Calculator      =   "frmBuyShareOtherCostsDetails.frx":0128
            Caption         =   "frmBuyShareOtherCostsDetails.frx":0148
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "Segoe UI"
               Size            =   27.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            DropDown        =   "frmBuyShareOtherCostsDetails.frx":01B4
            Keys            =   "frmBuyShareOtherCostsDetails.frx":01D2
            Spin            =   "frmBuyShareOtherCostsDetails.frx":021C
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
            ValueVT         =   1270218757
            Value           =   0
            MaxValueVT      =   977469445
            MinValueVT      =   1465647109
         End
      End
      Begin Threed.SSPanel pnlTotal_Current 
         Height          =   570
         Left            =   0
         TabIndex        =   6
         Top             =   1080
         Width           =   9855
         _Version        =   65536
         _ExtentX        =   17383
         _ExtentY        =   1005
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
         Begin TDBNumber6Ctl.TDBNumber numTotalAmount_Current 
            Height          =   510
            Left            =   30
            TabIndex        =   7
            TabStop         =   0   'False
            Top             =   30
            Width           =   9795
            _Version        =   65536
            _ExtentX        =   17277
            _ExtentY        =   900
            Calculator      =   "frmBuyShareOtherCostsDetails.frx":0244
            Caption         =   "frmBuyShareOtherCostsDetails.frx":0264
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "Segoe UI"
               Size            =   18
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            DropDown        =   "frmBuyShareOtherCostsDetails.frx":02D0
            Keys            =   "frmBuyShareOtherCostsDetails.frx":02EE
            Spin            =   "frmBuyShareOtherCostsDetails.frx":0338
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
            ValueVT         =   1270218757
            Value           =   0
            MaxValueVT      =   977469445
            MinValueVT      =   1465647109
         End
      End
   End
   Begin S50cMenu18.MenuBar abMenu 
      Height          =   2385
      Left            =   9960
      TabIndex        =   0
      Top             =   30
      Width           =   1905
      _ExtentX        =   3360
      _ExtentY        =   4207
   End
   Begin XtremeDockingPane.DockingPane DockingPane 
      Left            =   11040
      Top             =   2880
      _Version        =   1114115
      _ExtentX        =   635
      _ExtentY        =   635
      _StockProps     =   0
   End
End
Attribute VB_Name = "frmBuyShareOtherCostsDetails"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private oDocumentList           As SimpleDocumentList
Private objDSOItemTransaction   As DSOItemTransaction
Private dTotalOtherCost         As Double
Private mHasManualShare         As Boolean
Private strMsgBoxTitle          As String
Private CurrencyDef             As CurrencyDefinition
Private DecimaPlaces            As Integer

Private Enum OCColPosition
    ocLine = 0
    ocItemID = 1
    ocItemDescr = 2
    ocLotID = 3
    ocColor = 4
    ocSize = 5
    ocUnitPriceShare = 6
    ocDetailKey = 7
    ocUnitPrice = 8
    ocQuantity = 9
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

Private Sub CreateMenuBar()
    Dim oMenuItem As MenuBarItem
    
    Set oMenuItem = abMenu.Items.Add("miSave", gLng.GS(2160002), btnActionPrimary)
    oMenuItem.ToolKeyCode = SystemSettings.Application.ShortcutKeys("SAVE").KeyCode
    oMenuItem.ToolKeyShift = SystemSettings.Application.ShortcutKeys("SAVE").KeyShift
    
    Set oMenuItem = abMenu.Items.Add("miExit", gLng.GS(2160003), btnActionTertiary)
    oMenuItem.ToolKeyCode = vbKeyEscape
    
    Set oMenuItem = Nothing
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
      
    DecimaPlaces = 2
    Set objDSOItemTransaction = New DSOItemTransaction
    
    If SystemSettings.StartUpInfo.UseXPStyle Then
        vsfDocumentList.Appearance = flexXPThemes
    End If
    
    Set objUIItem = New UIHandler
    SystemSettings.Application.UI.UpdateStyles Me
    Set objUIItem = Nothing
    
    CreateMenuBar
    
    SystemSettings.Application.UI.DockingPane.Format DockingPane, picData, abMenu
    
    Resize
    
    strMsgBoxTitle = gLng.GS(0, SystemSettings.Application.LongName)
    
    mHasManualShare = False
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
    If UnloadMode = vbFormControlMenu Then
        Cancel = True
        Finish False
    End If
End Sub

Private Sub Form_Resize()
    
    Resize
    
End Sub

Private Sub Resize()

    pnlInstallements.Width = picData.Width
    pnlInstallements.Width = picData.Width
    
    pnlTotal.Width = picData.Width
    pnlTotal_Current.Width = picData.Width
    
    numTotalAmount.Width = pnlTotal.Width - 60
    numTotalAmount_Current.Width = pnlTotal.Width - 60
    
    vsfDocumentList.Width = pnlInstallements.Width
    vsfDocumentList.Height = pnlInstallements.Height
    VsfAutoResize vsfDocumentList, True
    
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Set oDocumentList = Nothing
    Set objDSOItemTransaction = Nothing
End Sub

Private Function Finish(ByVal Status As Boolean)
    mHasManualShare = Status
    If Status Then
        Set oDocumentList = GetDocuments(True)
    End If
    
    Me.Hide
End Function

Private Function GetDocuments(ByVal bLoadCurrentRow As Boolean) As SimpleDocumentList
    Dim objDocument             As SimpleDocument
    Dim objDocumentList         As SimpleDocumentList
    Dim oSimpleItemDetail       As SimpleItemDetail
    Dim oNewSimpleItemDetail    As SimpleItemDetail
    Dim i                       As Integer
    Dim sDetailKey              As String
    Dim abort                   As Boolean
     
    With vsfDocumentList
        For i = 1 To .Rows - 1
            Set objDocument = New SimpleDocument
            Set objDocumentList = New SimpleDocumentList
            abort = False
            For Each objDocument In oDocumentList
                For Each oSimpleItemDetail In objDocument.Details ' objSimpleDocument.Details
                    With oSimpleItemDetail
                        sDetailKey = .DestinationTransSerial & "|" & .DestinationTransDocument & "|" & Str$(.DestinationTransDocNumber) & "|" & Str$(.DestinationLineItemID) & "|" & .ItemID & "|" & Str$(.Color.ColorID) & "|" & Str$(.Size.SizeID)
                    End With
                
                    If Not objDocument.Details.IsInCollection(sDetailKey) Then
                        Set oNewSimpleItemDetail = New SimpleItemDetail
                        With oNewSimpleItemDetail
                            .DestinationTransSerial = oSimpleItemDetail.DestinationTransSerial
                            .DestinationTransDocument = oSimpleItemDetail.DestinationTransDocument
                            .DestinationTransDocNumber = oSimpleItemDetail.DestinationTransDocNumber
                            .DestinationLineItemID = oSimpleItemDetail.DestinationLineItemID
                            .ItemID = oSimpleItemDetail.ItemID
                            .LotID = oSimpleItemDetail.LotID
                            .Color.ColorID = oSimpleItemDetail.Color.ColorID
                            .Size.SizeID = oSimpleItemDetail.Size.SizeID
                            .UnitPrice = oSimpleItemDetail.UnitPrice
                            .Quantity = oSimpleItemDetail.Quantity
                            .ItemSearchKey = .DestinationTransSerial & "|" & .DestinationTransDocument & "|" & Str$(.DestinationTransDocNumber) & "|" & Str$(.DestinationLineItemID) & "|" & .ItemID & "|" & Str$(.Color.ColorID) & "|" & Str$(.Size.SizeID)
                        End With
    
                        objDocument.Details.Add oNewSimpleItemDetail.Clone
                    End If
              
                    If .TextMatrix(i, ocDetailKey) = sDetailKey Then
                        oSimpleItemDetail.UnitPrice = MyRound(.TextMatrix(i, ocUnitPriceShare), DecimaPlaces)
                        abort = True
                    End If
                    If abort = True Then
                        Exit For
                    End If
                Next
                
                If abort = True Then
                    Exit For
                End If
            
            Next
        Next
    End With
    
    Set GetDocuments = oDocumentList
    
    Set objDocument = Nothing
    Set objDocumentList = Nothing
    Set oSimpleItemDetail = Nothing
    Set oNewSimpleItemDetail = Nothing

End Function

Public Function ShowModal(ByVal ShareValue As Double, ByVal ItemIndex As Long) As Boolean
    Dim objDocument                     As SimpleDocument
    Dim objTempItemTransaction          As ItemTransaction
    Dim objTempItemTransactionDetail    As ItemTransactionDetail
    Dim oNewSimpleItemDetail            As SimpleItemDetail
    Dim ManualUnitPrice                 As Double
    Dim sDetailKey                      As String
    Dim CurPos                          As Integer
    Dim bHasManualShare                 As Boolean
    Dim i As Integer
    Dim dTotalAmountShare As Double
    Dim dTotaQuantityShare As Double
    Dim dTotaAmoutQTY As Double
    Dim dRemainValue As Double
        
    ShowModal = False
    CurPos = 1
     
    dTotalOtherCost = ShareValue
    CenterForm Me
    
    vsfDocumentList.Rows = 1
    Set CurrencyDef = New CurrencyDefinition
    If Not oDocumentList Is Nothing Then
        For Each objDocument In oDocumentList
                      
            If CurPos = ItemIndex Then
                
                Me.Caption = "Repartir Custos (" & objDocument.TransID.ToString & ")"
                     
                Set objTempItemTransaction = objDSOItemTransaction.GetItemTransaction(dcTypePurchase, objDocument.TransID.TransSerial, objDocument.TransID.TransDocument, objDocument.TransID.TransDocNumber)
            
                For Each objTempItemTransactionDetail In objTempItemTransaction.Details
                    ManualUnitPrice = 0
                    
                     Set CurrencyDef = objTempItemTransactionDetail.BaseCurrency
                    
                    With objTempItemTransactionDetail
                        sDetailKey = objDocument.TransID.TransSerial & "|" & objDocument.TransID.TransDocument & "|" & Str$(objDocument.TransID.TransDocNumber) & "|" & Str$(.LineItemID) & "|" & .ItemID & "|" & Str$(.Color.ColorID) & "|" & Str$(.Size.SizeID)
                    End With
                    
                    If Not objDocument.Details.IsInCollection(sDetailKey) Then
                        Set oNewSimpleItemDetail = New SimpleItemDetail
                        With oNewSimpleItemDetail
                            .DestinationTransSerial = objDocument.TransID.TransSerial
                            .DestinationTransDocument = objDocument.TransID.TransDocument
                            .DestinationTransDocNumber = objDocument.TransID.TransDocNumber
                            .DestinationLineItemID = objTempItemTransactionDetail.LineItemID
                            .ItemID = objTempItemTransactionDetail.ItemID
                            .description = objTempItemTransactionDetail.description
                            .LotID = objTempItemTransactionDetail.LOT.LotID
                            .Color.ColorID = objTempItemTransactionDetail.Color.ColorID
                            .Size.SizeID = objTempItemTransactionDetail.Size.SizeID
                            .UnitPrice = 0
                            .Quantity = objTempItemTransactionDetail.Quantity
                            .ItemSearchKey = .DestinationTransSerial & "|" & .DestinationTransDocument & "|" & Str$(.DestinationTransDocNumber) & "|" & Str$(.DestinationLineItemID) & "|" & .ItemID & "|" & Str$(.Color.ColorID) & "|" & Str$(.Size.SizeID)
                        End With
    
                        objDocument.Details.Add oNewSimpleItemDetail.Clone
                    End If
                          
                    ManualUnitPrice = MyRound(objDocument.Details.item(sDetailKey).UnitPrice, DecimaPlaces) 'objDocument.Details.item(sDetailKey).UnitPrice
                    'in case ManualUnitPrice > 0 mark as HasManualShare
                    If ManualUnitPrice > 0 Then
                        bHasManualShare = True
                    End If
                    
                    If objTempItemTransactionDetail.LOT.LotID <> "" Then
                        vsfDocumentList.ColHidden(ocLotID) = False
                    End If
                    
                    If objTempItemTransactionDetail.Color.ColorID > 0 Then
                        vsfDocumentList.ColHidden(ocColor) = False
                    End If
                    
                    If objTempItemTransactionDetail.Size.SizeID > 0 Then
                        vsfDocumentList.ColHidden(ocSize) = False
                    End If
                     
                    vsfDocumentList.AddItem objTempItemTransactionDetail.LineItemID & vbTab & objTempItemTransactionDetail.ItemID & vbTab & objTempItemTransactionDetail.description & vbTab & objTempItemTransactionDetail.LOT.LotID & vbTab & objTempItemTransactionDetail.Color.description & vbTab & objTempItemTransactionDetail.Size.description & vbTab & ManualUnitPrice & vbTab & sDetailKey & vbTab & objTempItemTransactionDetail.UnitPrice & vbTab & objTempItemTransactionDetail.Quantity
                    
                Next
            
                Exit For
            End If
            CurPos = CurPos + 1
        Next
    End If
        
    With numTotalAmount
        .Format = CurrencyDef.CreateMoneyMask(True)
        .DisplayFormat = .Format
        .maxValue = GetTypeMaxValue(vtTDBNumber)
        .minValue = -1 * GetTypeMaxValue(vtTDBNumber)
        .Value = dTotalOtherCost
    End With

    With numTotalAmount_Current
        .Format = CurrencyDef.CreateMoneyMask(True)
        .DisplayFormat = .Format
        .maxValue = GetTypeMaxValue(vtTDBNumber)
        .minValue = -1 * GetTypeMaxValue(vtTDBNumber)
        .Value = dTotalOtherCost
    End With
    
    FormatVsf

    DecimaPlaces = CurrencyDef.DecimalPlaces
    'case manual share false calculate de automatic share
    If bHasManualShare = False Then
        
        dRemainValue = dTotalOtherCost
        
        'Calculate Total Amount to share
        With vsfDocumentList
            For i = 1 To .Rows - .FixedRows
                 dTotalAmountShare = dTotalAmountShare + MyRound(.TextMatrix(i, ocUnitPrice), DecimaPlaces)
                 dTotaQuantityShare = dTotaQuantityShare + MyRound(.TextMatrix(i, ocQuantity), DecimaPlaces)
                 dTotaAmoutQTY = dTotaAmoutQTY + (MyRound(.TextMatrix(i, ocUnitPrice), DecimaPlaces) * MyRound(.TextMatrix(i, ocQuantity), DecimaPlaces))
            Next
            
            For i = 1 To .Rows - .FixedRows
                
                If CDbl(dTotaAmoutQTY) > 0 And dRemainValue > 0 Then
                    .TextMatrix(i, ocUnitPriceShare) = MyRound(((CDbl(.TextMatrix(i, ocUnitPrice)) * CDbl(.TextMatrix(i, ocQuantity))) * CDbl(dTotalOtherCost)) / dTotaAmoutQTY, DecimaPlaces)
                    dRemainValue = dRemainValue - CDbl(.TextMatrix(i, ocUnitPriceShare))
                Else
                    .TextMatrix(i, ocUnitPriceShare) = 0
                End If
            
            Next
            
            'Add Remain value to last Line
            If dRemainValue <> 0 Then
                .TextMatrix(.Rows - .FixedRows, ocUnitPriceShare) = CDbl(.TextMatrix(.Rows - .FixedRows, ocUnitPriceShare)) + dRemainValue
            End If
            
        End With
    End If
    
    Calc_Current_TotalAmount
    
    Me.Show vbModal
    ShowModal = mHasManualShare
    
    Set objDocument = Nothing
    Set objTempItemTransaction = Nothing
    Set objTempItemTransactionDetail = Nothing
    Set oNewSimpleItemDetail = Nothing
 
End Function

Private Sub FormatVsf()

    With vsfDocumentList
        .ColWidth(ocLine) = 500
        .ColWidth(ocItemID) = 1000
        .ColWidth(ocItemDescr) = 3000
        .ColWidth(ocLotID) = 850
        .ColWidth(ocColor) = 850
        .ColWidth(ocSize) = 850
        .ColWidth(ocUnitPriceShare) = 850
        .ColWidth(ocUnitPrice) = 850

        .ColHidden(ocLotID) = True
        .ColHidden(ocColor) = True
        .ColHidden(ocSize) = True
        .ColHidden(ocDetailKey) = True
        .ColHidden(ocUnitPrice) = True
        .ColHidden(ocQuantity) = True
        
        
        On Error Resume Next
        .ColFormat(ocUnitPriceShare) = CurrencyDef.CreateMoneyMask(True, True, True)
        
        .ExtendLastCol = True
        .AllowUserResizing = flexResizeColumns
        
    End With

    SystemSettings.Application.UI.FlexGrid.SetColors vsfDocumentList
    
End Sub

Private Sub VsfAutoResize(ByVal VsfFlexgrid As Object, ByVal blnUseScroollBarVertical As Boolean)
    Dim dblWith As Double
    Dim dblUsedWith As Double
    Dim intX As Integer
    Dim dblValue As Double
    
    
    On Error GoTo errHandler
    If blnUseScroollBarVertical Then
        dblWith = VsfFlexgrid.Width - 350
    End If
    If dblWith > 0 Then
        For intX = 0 To VsfFlexgrid.Cols - 1
            If VsfFlexgrid.ColHidden(intX) = False Then
                dblUsedWith = dblUsedWith + VsfFlexgrid.ColWidth(intX)
            End If
        Next intX
        dblValue = 100 / (dblUsedWith * 100 / dblWith)
        dblWith = 0
        For intX = 0 To VsfFlexgrid.Cols - 1
            If VsfFlexgrid.ColWidth(intX) > 0 Then
                dblWith = dblWith + VsfFlexgrid.ColWidth(intX) * dblValue
                VsfFlexgrid.ColWidth(intX) = dblWith
                dblWith = dblWith - VsfFlexgrid.ColWidth(intX)
            End If
        Next intX
    End If

    Exit Sub
errHandler:
    MsgBoxFrontOffice Trim$(Str$(Err.Number)) & " - " & Err.description & vbCrLf & "<frmItemLastPrices:VsfAutoResize>", vbInformation, strMsgBoxTitle
End Sub

Public Property Set BuyShareOtherCost(ByVal Value As SimpleDocumentList)
    Set oDocumentList = Value
End Property

Public Function GetBuyShareOtherCostsDocumentList() As SimpleDocumentList
    Set GetBuyShareOtherCostsDocumentList = oDocumentList
End Function

Private Function validate() As Boolean
    Dim objDocumentList As SimpleDocumentList
    Dim dblTotal        As Double
    Dim i               As Integer
    Dim dblRowValue     As Double
    Dim blnReturn       As Boolean
    Dim intRound        As Integer
    
    dblRowValue = 0
    validate = True
    
'    With vsfDocumentList
'        For i = 1 To .Rows - .FixedRows
'
'            If Not IsNumeric(.TextMatrix(i, ocUnitPriceShare)) Then
'                MsgBox "Valor Invalido na linha Nº" & i, vbInformation '*lng
'                validate = False
'                Exit Function
'            End If
'
'            dblRowValue = dblRowValue + CDbl(.TextMatrix(i, ocUnitPriceShare))
'        Next
'    End With

    If CDbl(numTotalAmount_Current.Value) <> CDbl(numTotalAmount.Value) Then
        validate = False
        MsgBox gLng.GS(3950007), vbInformation
    End If

    Set objDocumentList = Nothing
End Function

Private Sub vsfDocumentList_EnterCell()
    
    With vsfDocumentList
        If .Col <> ocUnitPriceShare Then
            .Editable = False
        Else
            .Editable = True
        End If
    End With

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

Private Sub vsfDocumentList_LeaveCell()
    With vsfDocumentList
        If .Col = ocUnitPriceShare Then
            Calc_Current_TotalAmount
        End If
    End With
End Sub

Private Sub Calc_Current_TotalAmount()
    Dim i               As Integer
    Dim dblRowValue     As Double

    With vsfDocumentList
        dblRowValue = 0
        
            For i = .FixedRows To .Rows - .FixedRows
                If IsNumeric(.TextMatrix(i, ocUnitPriceShare)) Then
                
                    dblRowValue = dblRowValue + CDbl(.TextMatrix(i, ocUnitPriceShare))
                Else
                    MsgBox "Valor Invalido na linha Nº" & i, vbInformation '*lng
                    dblRowValue = 0
                    Exit For
                End If
            Next
        
        numTotalAmount_Current.Value = dblRowValue
    End With

End Sub

Private Sub vsfDocumentList_ValidateEdit(ByVal Row As Long, ByVal Col As Long, Cancel As Boolean)
    With vsfDocumentList
        If .Col = ocUnitPriceShare Then
            If IsNumeric(.EditText) Then
                .TextMatrix(Row, ocUnitPriceShare) = MyRound(.EditText, DecimaPlaces)
                Calc_Current_TotalAmount
            Else
                MsgBox "Valor Invalido na linha ", vbInformation  '*lng
                .TextMatrix(Row, ocUnitPriceShare) = 0
            End If
        End If
    End With

End Sub
