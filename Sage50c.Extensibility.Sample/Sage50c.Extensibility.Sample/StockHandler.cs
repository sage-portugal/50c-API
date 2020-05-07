using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S50cBL18;
using S50cSys18;
using S50cBO18;

namespace Sage50c.ExtenderSample
{
    public class StockHandler : IDisposable
    {
        private ExtenderEvents headerStockEvents = null;
        private ExtenderEvents detailsStockEvents = null;

        private BSOStockTransaction bsoStockTrans = null;
        private PropertyChangeNotifier propaChangeNotifier = null;

        public void SetDetailEventsHandler(ExtenderEvents e)
        {
            detailsStockEvents = e;

            detailsStockEvents.OnValidating += DetailsStockEvents_OnValidating;
        }

        private void DetailsStockEvents_OnValidating(object Sender, ExtenderEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SetHeaderEventsHandler(ExtenderEvents e)
        {
            headerStockEvents = e;

            headerStockEvents.OnInitialize += HeaderStockEvents_OnInitialize;
            headerStockEvents.OnMenuItem += HeaderStockEvents_OnMenuItem;
            headerStockEvents.OnValidating += HeaderStockEvents_OnValidating;
        }

        private void HeaderStockEvents_OnValidating(object Sender, ExtenderEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void HeaderStockEvents_OnMenuItem(object Sender, ExtenderEventArgs e)
        {
            var menuId = (string)e.get_data();

            switch (menuId)
            {
                case "mniXTrans11":
                    System.Windows.Forms.MessageBox.Show("YAY");
                    break;
            }
        }

        private void HeaderStockEvents_OnInitialize(object Sender, ExtenderEventArgs e)
        {
            var propList = (ExtendedPropertyList)e.get_data();
            propaChangeNotifier = (PropertyChangeNotifier)propList.get_Value("PropertyChangeNotifier");
            propaChangeNotifier.PropertyChanged += OnaPropertyChanged;

            bsoStockTrans = (BSOStockTransaction)propList.get_Value("TransactionManager");

            e.result.ResultMessage = "HeaderEvents_OnInitialize";

            // Colocar o caminho para o icone. 
            // Não usar os nomes de ficheiro da Sage em:
            //      TARGETDIR\Icons50c
            //      TARGETDIR\Images
            var myTargetDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            myTargetDir = System.IO.Path.Combine(myTargetDir, "Icons");

            var newMenus1 = new ExtenderMenuItems();
            //
            //Criar o grupo: Tab
            var mnuGroup1 = newMenus1.Add("mniXCustomTools1", "Custom Tools");
            //criar item1
            var mnuItem1 = mnuGroup1.ChildItems.Add("mniXTrans11", "Custom Item 1");
            mnuItem1.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;
            mnuItem1.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-01.png");

            //criar item2
            mnuItem1 = mnuGroup1.ChildItems.Add("mniXTrans21", "Custom Item 2");
            mnuItem1.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;
            mnuItem1.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-02.png");

            object returnMenu = newMenus1;
            e.result.set_data(returnMenu);
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        void OnaPropertyChanged(string PropertyID, ref object value, ref bool Cancel)
        {
            // HANDLE BSOItemTransaction PROPERTY CHANGES HERE

            Console.WriteLine("OnPropertyChanged {0}={1}; Cancel={2}", PropertyID, value, Cancel);
            //Cancel = false;
        }
    }
}
