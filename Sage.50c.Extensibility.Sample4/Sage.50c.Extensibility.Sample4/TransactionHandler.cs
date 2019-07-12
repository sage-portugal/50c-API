using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S50cBL18;
using S50cSys18;
using S50cBO18;
using System.Windows.Forms;

namespace Sage50c.ExtenderSample {
    class TransactionHandler : IDisposable {
        private ExtenderEvents headerEvents = null;
        private ExtenderEvents detailEvents = null;

        private BSOItemTransaction bsoItemTrans = null;
        private PropertyChangeNotifier propChangeNotifier = null;
        private ItemTransaction _transaction = null;


        /// <summary>
        /// Eventos disparados pelo Retail:
        /// OnInitialize:   Uma vez no arranque da aplicação
        /// OnNew:          Sempre que se inicializa uma nova linha
        /// OnValidating:   Ao validar uima linha. Pode ser cancelada a introdução da linha
        /// 
        /// Restantes eventos não são disparados.
        /// </summary>
        /// <param name="e"></param>
        public void SetDetailEventsHandler(ExtenderEvents e) {
            detailEvents = e;

            detailEvents.OnInitialize += DetailEvents_OnInitialize;
            detailEvents.OnValidating += DetailEvents_OnValidating;
            detailEvents.OnNew += DetailEvents_OnNew;
        }

        private void DetailEvents_OnDispose() {
            System.Windows.Forms.MessageBox.Show("DetailEvents_OnDispose", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DetailEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
            //System.Windows.Forms.MessageBox.Show("DetailEvents_OnInitialize");
        }

        private void DetailEvents_OnNew(object Sender, ExtenderEventArgs e) {
            var detail = (ItemTransactionDetail)e.get_data();

            //detail.ItemID = "7up4";

            //e.result.ResultMessage = "Alterei a descrição de um artigo novo";
            //e.result.Success = true;
        }

        public void SetHeaderEventsHandler(ExtenderEvents e) {
            headerEvents = e;

            headerEvents.OnInitialize += HeaderEvents_OnInitialize;
            headerEvents.OnMenuItem += HeaderEvents_OnMenuItem;
            headerEvents.OnValidating += HeaderEvents_OnValidating;
            headerEvents.OnSave += HeaderEvents_OnSave;
            headerEvents.OnDelete += HeaderEvents_OnDelete;
            headerEvents.OnNew += HeaderEvents_OnNew;
            headerEvents.OnLoad += HeaderEvents_OnLoad;
            headerEvents.OnDispose += HeaderEvents_OnDispose;
        }

        private void HeaderEvents_OnDispose() {
            // Dispose your objects
        }

        private void HeaderEvents_OnLoad(object Sender, ExtenderEventArgs e) {
            var trans = (ItemTransaction)e.get_data();

            ///... Code here

            e.result.Success = true;
        }

        private void HeaderEvents_OnNew(object Sender, ExtenderEventArgs e) {
            _transaction = (ItemTransaction)e.get_data();
        }

        private void HeaderEvents_OnDelete(object Sender, ExtenderEventArgs e) {
            System.Windows.Forms.MessageBox.Show("HeaderEvents_OnDelete: Acabei de anular.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HeaderEvents_OnSave(object Sender, ExtenderEventArgs e) {
            System.Windows.Forms.MessageBox.Show("HeaderEvents_OnSave: Gravou.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HeaderEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            var propList = (ExtendedPropertyList)e.get_data();
            var forDeletion = (bool)propList.get_Value("forDeletion");
            var transaction = (ItemTransaction) propList.get_Value("Data");

            if (!forDeletion) {
                foreach (ItemTransactionDetail detail in transaction.Details) {
                    if (detail.FamilyID == 1) {
                        e.result.ResultMessage = string.Format("HeaderEvents_OnValidating: Não pode vender artigos da familia {0}", detail.FamilyName);
                        e.result.Success = false;
                        break;
                    }
                }


                //if (transaction.Details.Count > 3) {
                //    e.result.Success = true;
                //}
                //else {
                //    e.result.Success = false;
                //    e.result.ResultMessage = "Não é possivel gravar documentos com menos de 3 linhas";
                //}
            }
            else {
                e.result.ResultMessage = "HeaderEvents_OnValidating: Não pode anular documentos! Mas vou devolver TRUE para deixar anular.";
                e.result.Success = true;
            }
        }

        void OnPropertyChanged(string PropertyID, ref object value, ref bool Cancel) {
            // HANDLE BSOItemTransaction PROPERTY CHANGES HERE

            Console.WriteLine("OnPropertyChanged {0}={1}; Cancel={2}", PropertyID, value, Cancel);
            //Cancel = false;
        }


        #region HEADER EVENTS


        /// <summary>
        /// Inicialização
        /// Podemos adicionar novas opções de menu aqui
        /// IN:
        ///     e.get_data(): ExtendedPropertyList
        ///     "PropertyChangeNotifier" = Evento que podemos subscrever para controlar quando uma propriedade é alterada
        ///     "TransactionManager" = BSOItemTransaction; Controlador da transação em curso
        /// 
        /// OUT:
        ///     result.Sucess: true para sinalizar sucesso e carregar novos menus; false para cancelar
        ///     result.ResultMessage: Ignorado
        ///     result.set_data( ExtenderMenuItems ): Items de menu a carregar 
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void HeaderEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
            var propList = (ExtendedPropertyList)e.get_data();
            propChangeNotifier = (PropertyChangeNotifier)propList.get_Value("PropertyChangeNotifier");
            propChangeNotifier.PropertyChanged += OnPropertyChanged;

            bsoItemTrans = (BSOItemTransaction)propList.get_Value("TransactionManager");
            bsoItemTrans.WarningItemStock += BsoItemTrans_WarningItemStock;

            var newMenus = new ExtenderMenuItems();
            //
            //Criar o grupo: Tab
            var mnuGroup = newMenus.Add("mniXCustomTools", "Custom Tools");
            //criar item1
            var mnuItem1 = mnuGroup.ChildItems.Add("mniXTrans1", "Custom Item 1");
            mnuItem1.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;
            //mnuItem1.Picture = ImageConverter.GetIPictureDispFromImage(  )

            //criar item2
            mnuItem1 = mnuGroup.ChildItems.Add("mniXTrans2", "Custom Item 2");
            mnuItem1.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;

            object returnMenu = newMenus;
            e.result.set_data(returnMenu);

        }

        void HeaderEvents_OnMenuItem(object Sender, ExtenderEventArgs e) {
            var menuId = (string)e.get_data();
            var rnd = new Random();

            switch (menuId) {
                case "mniXTrans1":
                    //System.Windows.Forms.MessageBox.Show("YAY");
                    double qty = rnd.Next(1, 10) + (double)rnd.Next(0, 99) / 100;
                    double unitPrice = rnd.Next(1, 100) + (double)rnd.Next(0, 99) / 100;

                    var item = MyApp.DSOCache.ItemProvider.GetItem("aaa", MyApp.SystemSettings.BaseCurrency);
                    if (item != null) {
                        var detail = new ItemTransactionDetail() {
                            LineItemID = bsoItemTrans.Transaction.Details.Count + 1,
                            ItemID = item.ItemID,
                            Description = item.Description
                        };
                        if (bsoItemTrans.Transaction.TransactionTaxIncluded) {
                            detail.TaxIncludedPrice = unitPrice;
                        }
                        else {
                            detail.UnitPrice = unitPrice;
                        }
                        detail.SetUnitOfSaleID(item.UnitOfSaleID);
                        detail.SetQuantity(qty);
                        detail.TaxableGroupID = item.TaxableGroupID;

                        //var detail = TransAddDetail(bsoItemTrans.Transaction, item, qty, "UNI", unitPrice, 23, 1, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, 0, 0, 0);
                        // IMPORTANTE: Mandar calcular a linha!
                        bsoItemTrans.BSOItemTransactionDetail.Calculate(detail);
                        // Adicionar à venda
                        bsoItemTrans.AddDetail(detail);
                    }
                    break;
                case "XFunctionA":
                    MessageBox.Show("Your function here...");
                    break;

                case "XPosDisplay":
                    SendMessageToCustomerDisplay();
                    break;

            }
        }

        //switch (e.get_data().ToString().ToLower()) {
        //    case "lepeso":
        //        if (bsoItemTrans.BSOItemTransactionDetail != null) {
        //            bsoItemTrans.BSOItemTransactionDetail.HandleItemDetail("9.99", TransDocFieldIDEnum.fldQuantity);

        //            e.result.Success = true;
        //            e.result.ResultMessage = string.Empty;
        //        }
        //        else {
        //            e.result.Success = false;
        //            e.result.ResultMessage = "Não foi posivel obter o controlador da linha (BSOItemTransactionDetail)";
        //        }
        //        break;
        //}
        //}

        #endregion


        /// <summary>
        /// EXEMPLO DE VALIDAÇÃO NA LINHA   
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void DetailEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            ExtendedPropertyList properties = (ExtendedPropertyList)e.get_data();
            ItemTransactionDetail itemTransactionDetail = (ItemTransactionDetail)properties.get_Value("Data");
            string errorMessage = string.Empty;

            //TODO:
            //Insert line handling code HERE
            //------------------------------
            //if (itemTransactionDetail.UnitOfSaleID.Equals("kg", StringComparison.CurrentCultureIgnoreCase)) {
            //    itemTransactionDetail.Quantity = 9.99;
            //}
            //------------------------------

            //if (itemTransactionDetail.Quantity > 100) {
            //    e.result.Success = false;
            //    e.result.R    esultMessage = string.Format("Atenção! Quantidade {0} superior ao permitido", itemTransactionDetail.Quantity);
            //}
            //else {
            //    e.result.Success = true;
            //    e.result.ResultMessage = string.Empty;
            //}

            //// Uncomment to force a taxable group. In this case, it is necessary to recalculate all the detail
            //itemTransactionDetail.TaxableGroupID = 3;
            //bsoItemTrans.BSOItemTransactionDetail.Calculate(itemTransactionDetail);

            properties = null;
            itemTransactionDetail = null;
        }


        /// <summary>
        /// Adiciona um detalhe (linha) à transação
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        /// <param name="unitOfMeasureId"></param>
        /// <param name="unitPrice"></param>
        /// <param name="taxPercent"></param>
        /// <param name="whareHouseId"></param>
        private ItemTransactionDetail TransAddDetail(ItemTransaction trans, Item item, double qty, string unitOfMeasureId, double unitPrice, double taxPercent, short whareHouseId,
                                     short colorId, short sizeId,
                                     string serialNumberPropId, string serialNumberPropValue,
                                     string lotId, string lotDescription, DateTime lotExpDate, short lotReturnWeek, short lotReturnYear, short lotEditionId) {

            var doc = MyApp.SystemSettings.WorkstationInfo.Document[trans.TransDocument];

            ItemTransactionDetail transDetail = new ItemTransactionDetail();
            transDetail.BaseCurrency = MyApp.SystemSettings.BaseCurrency;
            transDetail.ItemID = item.ItemID;
            transDetail.CreateDate = trans.CreateDate;
            transDetail.CreateTime = trans.CreateTime;
            transDetail.ActualDeliveryDate = trans.CreateDate;
            //Utilizar a descrição do artigo, ou uma descrição personalizada
            transDetail.Description = item.Description;
            // definir a quantidade
            transDetail.Quantity = qty;
            // Preço unitário. NOTA: Ver a diferença se o documento for com impostos incluidos!
            if (trans.TransactionTaxIncluded)
                transDetail.TaxIncludedPrice = unitPrice;
            else
                transDetail.UnitPrice = unitPrice;
            // Definir a lista de unidades
            transDetail.UnitList = item.UnitList;
            // Definir a unidade de venda/compra
            transDetail.SetUnitOfSaleID(unitOfMeasureId);
            //Definir os impostos
            short TaxGroupId = MyApp.DSOCache.TaxesProvider.GetTaxableGroupIDFromTaxRate(taxPercent, MyApp.SystemSettings.SystemInfo.DefaultCountryID, MyApp.SystemSettings.SystemInfo.TaxRegionID);
            transDetail.TaxableGroupID = TaxGroupId;
            //*** Uncomment for discout
            //transDetail.DiscountPercent = 10
            //
            // Se o Armazém não existir, utilizar o default que se encontra no documento.
            if (MyApp.DSOCache.WarehouseProvider.WarehouseExists(whareHouseId))
                transDetail.WarehouseID = whareHouseId;
            else
                transDetail.WarehouseID = doc.Defaults.Warehouse;
            // Identificador da linha
            transDetail.LineItemID = trans.Details.Count + 1;
            //
            //*** Uncomment to provide line totals
            //.TotalGrossAmount =        'Line Gross amount
            //.TotalNetAmount =          'Net Gross amount
            //
            //Definir o último preço de compra
            if (doc.TransDocType == DocumentTypeEnum.dcTypePurchase) {
                transDetail.ItemExtraInfo.ItemLastCostTaxIncludedPrice = item.SalePrice[0].TaxIncludedPrice;
                transDetail.ItemExtraInfo.ItemLastCostUnitPrice = item.SalePrice[0].UnitPrice;
            }

            //// Cores e tamanhos
            //if (MyApp.SystemSettings.SystemInfo.UseColorSizeItems ) {
            //    // Cores
            //    if (item.Colors.Count > 0) {
            //        ItemColor color = null;
            //        if (colorId > 0 && item.Colors.IsInCollection(colorId)) {
            //            color = item.Colors[ref colorId];
            //        }
            //        if (color == null) {
            //            throw new Exception(string.Format("A cor indicada [{0}] não existe.", colorId));
            //        }
            //        transDetail.Color.ColorID = colorId;
            //        transDetail.Color.Description = color.ColorName;
            //        transDetail.Color.ColorKey = color.ColorKey;
            //        transDetail.Color.ColorCode = color.ColorCode;
            //    }
            //    //Tamanhos
            //    if (item.Sizes.Count > 0 ) {
            //        ItemSize size = null;
            //        if (sizeId > 0 && item.Sizes.IsInCollection(sizeId)) {
            //            size = item.Sizes[sizeId];
            //        }
            //        if (size == null) {
            //            throw new Exception(string.Format("O tamanho indicado [{0}] não existe.", sizeId));
            //        }
            //        transDetail.Size.Description = size.SizeName;
            //        transDetail.Size.SizeID = size.SizeID;
            //        transDetail.Size.SizeKey = size.SizeKey;
            //    }
            //}
            //
            //// Lotes - Edições
            //// Verificar se estão ativados no sistema e se foram marcados no documento
            //if (MyApp.SystemSettings.SystemInfo.UseKiosksItems 
            //    && (item.ItemType == ItemTypeEnum.itmLot || item.ItemType == ItemTypeEnum.itmEdition)) {
            //    ItemLot lot = null;
            //    if (item.LotList.Count > 0) {
            //        // Validar se existe a Edição
            //        // NOTA: Numa venda vamos sempre assumir que o lote registado na BD é que contém toda a informação relevante como a Validade, Semana e ano de decolução, etc...
            //        //       Vamos procurar pelo lote + edição
            //        lot = null;
            //        foreach (ItemLot tempLot in item.LotList) {
            //            if (tempLot.LotID == lotId && tempLot.EditionID == lotEditionId) {
            //                lot = tempLot;
            //                break;
            //            }
            //        }
            //    }
            //    // Se for uma compra adicionamos o lote
            //    if (lot == null && doc.TransDocType == DocumentTypeEnum.dcTypePurchase && doc.SignPurchaseReport == "+") {
            //        // Adicionar ume novo...
            //        lot = new ItemLot();
            //        lot.EditionID = lotEditionId;
            //        lot.ItemID = item.ItemID;
            //        lot.LotID = lotId;
            //        lot.ExpirationDate = lotExpDate;
            //        lot.ReturnWeek = lotReturnWeek;
            //        lot.ReturnYear = lotReturnYear;
            //        lot.ItemLotDescription = item.Description;
            //        lot.SupplierItemID = MyApp.DSOCache.ItemProvider.GetItemSupplierID(item.ItemID, item.SupplierID);
            //    }
            //    if (lot == null) {
            //        throw new Exception(string.Format("O lote [{0}], Edição [{1}] não existe.", lotId, lotEditionId));
            //    }
            //    transDetail.Lot.BarCode = lot.BarCode;
            //    transDetail.Lot.EditionID = lot.EditionID;
            //    transDetail.Lot.EffectiveDate = lot.EffectiveDate;
            //    transDetail.Lot.ExpirationDate = lot.ExpirationDate;
            //    transDetail.Lot.ItemID = lot.ItemID;
            //    transDetail.Lot.ItemLotDescription = lot.ItemLotDescription;
            //    transDetail.Lot.LotID = lot.LotID;
            //    transDetail.Lot.ReturnWeek = lot.ReturnWeek;
            //    transDetail.Lot.ReturnYear = lot.ReturnYear;
            //    transDetail.Lot.SalePrice = lot.SalePrice;
            //    transDetail.Lot.SaveSalePrice = lot.SaveSalePrice;
            //    transDetail.Lot.SupplierItemID = lot.SupplierItemID;
            //}
            //
            //// Propriedades (números de série e lotes)
            //// ATENÇÃO: As regras de verificação das propriedades não estão implementadas na API. Deve ser a aplicação a fazer todas as validações necessárias
            ////          Como por exemplo a movimentação duplicada de números de série
            //// Verificar se estão ativadas no sistema e se foram marcadas no documento
            //if (MyApp.SystemSettings.SystemInfo.UsePropertyItems ) {
            //    // O Artigo tem propriedades ?
            //    if (item.PropertyEnabled) {
            //        // NOTA: Para o exemplo atual apenas queremos uma propriedade definida no artigo com o ID1 = "NS".
            //        //       Para outras propriedades e combinações, o código deve ser alterado em conformidade.
            //        if (item.PropertyID1.Equals("NS", StringComparison.CurrentCultureIgnoreCase)) {
            //            transDetail.ItemProperties.ResetValues();
            //            transDetail.ItemProperties.PropertyID1 = item.PropertyID1;
            //            transDetail.ItemProperties.PropertyID2 = item.PropertyID2;
            //            transDetail.ItemProperties.PropertyID3 = item.PropertyID3;
            //            transDetail.ItemProperties.ControlMode = item.PropertyControlMode;
            //            transDetail.ItemProperties.ControlType = item.PropertyControlType;
            //            transDetail.ItemProperties.UseExpirationDate = item.PropertyUseExpirationDate;
            //            transDetail.ItemProperties.UseProductionDate = item.PropertyUseProductionDate;
            //            transDetail.ItemProperties.ExpirationDateControl = item.PropertyExpirationDateControl;
            //            transDetail.ItemProperties.MaximumQuantity = item.PropertyMaximumQuantity;
            //            transDetail.ItemProperties.UsePriceOnProp1 = item.UsePriceOnProp1;
            //            transDetail.ItemProperties.UsePriceOnProp2 = item.UsePriceOnProp2;
            //            transDetail.ItemProperties.UsePriceOnProp3 = item.UsePriceOnProp3;
            //            //
            //            transDetail.ItemProperties.PropertyValue1 = serialNumberPropValue;
            //        }
            //    }
            //}
            item = null;
            //
            return transDetail;
        }

        private void BsoItemTrans_WarningItemStock(TransactionWarningsEnum MsgID, ItemTransactionDetail objItemTransactionDetail) {
            double dblStockQuantity = 0;
            double dblReorderPointQuantity = 0;
            string strMessage = string.Empty;

            switch (MsgID) {
                case TransactionWarningsEnum.tweItemColorSizeStockNotHavePhysical:
                case TransactionWarningsEnum.tweItemStockNotHavePhysical:
                    if (objItemTransactionDetail.PackQuantity == 0) {
                        dblStockQuantity = objItemTransactionDetail.QntyPhysicalBalanceCount;
                    }
                    else {
                        dblStockQuantity = objItemTransactionDetail.QntyPhysicalBalanceCount / objItemTransactionDetail.PackQuantity;
                    }
                    strMessage = MyApp.gLng.GS((int)MsgID, new object[]{
                                                             objItemTransactionDetail.WarehouseID.ToString().Trim(),
                                                             dblStockQuantity,
                                                             objItemTransactionDetail.UnitOfSaleID,
                                                             objItemTransactionDetail.ItemID,
                                                             objItemTransactionDetail.Size.Description,
                                                             objItemTransactionDetail.Color.Description
                                                });

                    break;

                case TransactionWarningsEnum.tweItemReorderPoint:
                case TransactionWarningsEnum.tweItemColorSizeReorderPoint:
                    if (objItemTransactionDetail.PackQuantity == 0) {
                        dblStockQuantity = objItemTransactionDetail.QntyWrPhysicalBalanceCount;
                        dblReorderPointQuantity = objItemTransactionDetail.QntyReorderPoint;
                    }
                    else {
                        dblStockQuantity = objItemTransactionDetail.QntyWrPhysicalBalanceCount / objItemTransactionDetail.PackQuantity;
                        dblReorderPointQuantity = objItemTransactionDetail.QntyReorderPoint / objItemTransactionDetail.PackQuantity;
                    }
                    strMessage = MyApp.gLng.GS((int)MsgID, new object[]{
                                                             objItemTransactionDetail.WarehouseID.ToString(),
                                                             dblStockQuantity.ToString(),
                                                             objItemTransactionDetail.UnitOfSaleID,
                                                             objItemTransactionDetail.ItemID,
                                                             objItemTransactionDetail.Size.Description,
                                                             objItemTransactionDetail.Color.Description,
                                                             dblReorderPointQuantity.ToString()
                                                            });
                    break;

                default:
                    if (objItemTransactionDetail.PackQuantity == 0) {
                        dblStockQuantity = objItemTransactionDetail.QntyAvailableBalanceCount;
                    }
                    else {
                        dblStockQuantity = objItemTransactionDetail.QntyAvailableBalanceCount / objItemTransactionDetail.PackQuantity;
                    }
                    strMessage = MyApp.gLng.GS((int)MsgID, new object[]{
                                                             objItemTransactionDetail.WarehouseID.ToString(),
                                                             dblStockQuantity.ToString(),
                                                             objItemTransactionDetail.UnitOfSaleID,
                                                             objItemTransactionDetail.ItemID,
                                                             objItemTransactionDetail.Size.Description,
                                                             objItemTransactionDetail.Color.Description
                                                            });
                    break;
            }
            if (!string.IsNullOrEmpty(strMessage)) {
                MessageBox.Show(strMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Dispose() {
            headerEvents = null;
            detailEvents = null;
            if (bsoItemTrans != null) {
                bsoItemTrans.WarningItemStock -= BsoItemTrans_WarningItemStock;
                bsoItemTrans = null;
            }
            propChangeNotifier = null;
        }
        private void SendMessageToCustomerDisplay() {
            try {
                bool bUseCustomerDisplay = bsoItemTrans.HaveLineDisplay;
                bool bUseAdsViewer = bsoItemTrans.AdsViewerIsConnected;
                string value = "Total: ";
                            
                var newCustomerDisplay = new POSCustomerDisplays();

                if (bUseCustomerDisplay) {
                    newCustomerDisplay = bsoItemTrans.GetLineDisplay();
                    if (newCustomerDisplay == null) {
                        bUseCustomerDisplay = false;
                    }

                    if (bUseCustomerDisplay || bUseAdsViewer) {
                        using (var fCustomerDisplay = new FormCustomerDisplay()) {
                            fCustomerDisplay.UseCustomerDisplay = bUseCustomerDisplay;
                            fCustomerDisplay.UseAdsViewer = bUseAdsViewer;

                            fCustomerDisplay.DisplayLine1 = "Publicidade linha 1";
                            fCustomerDisplay.DisplayValue = 123.45;

                            string displayLine1 = null;
                            string displayLine2 = null;
                            short displayColumns = 0;
                            if (fCustomerDisplay.ShowDisplayMessage() == System.Windows.Forms.DialogResult.OK) {
                                short displayLines = 2;
                                short displayCharset = 0;
                                if (fCustomerDisplay.UseCustomerDisplay) {
                                    var deviceDescription = newCustomerDisplay.Description;
                                    short.TryParse(newCustomerDisplay.DisplayLines, out displayLines);
                                    short.TryParse(newCustomerDisplay.DisplayColumns, out displayColumns);
                                    displayCharset = newCustomerDisplay.SupportedCharSet;

                                    displayLine1 = fCustomerDisplay.DisplayLine1;

                                    if (displayLine1.Length > displayColumns) {
                                        displayLine1 = displayLine1.Substring(0, displayColumns);
                                    }
                                    displayLine1 = MyApp.StringFunctions.GetOEMString(displayCharset, displayLine1);
                                    //
                                    if (displayLines > 1) {
                                        displayLine2 = value + bsoItemTrans.Transaction.BaseCurrency.Symbol + " " + fCustomerDisplay.DisplayValue.ToString();
                                        displayLine2 = displayLine2.Insert(value.Length, new string(' ', (displayColumns - displayLine2.Length) * 2));
                                    }

                                    bsoItemTrans.LineDisplayMessage(displayLine1, displayLine2);
                                }

                                if (fCustomerDisplay.UseAdsViewer) {
                                    displayColumns = 28;
                                    displayLine1 = fCustomerDisplay.DisplayLine1;
                                    if (displayLine1.Length > displayColumns) {
                                        displayLine1 = displayLine1.Substring(0, displayColumns);
                                    }
                                    displayLine2 = value + bsoItemTrans.Transaction.BaseCurrency.Symbol + " " + fCustomerDisplay.DisplayValue.ToString();
                                    displayLine2 = displayLine2.Insert(value.Length, new string(' ', (displayColumns - displayLine2.Length) * 2));

                                    bsoItemTrans.AdsViewerDisplayMessage(displayLine1, displayLine2);
                                }
                            }

                        }
                    }

                };
                //
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}