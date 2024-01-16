using S50cBO22;
using S50cSAFTX22;
using System;
using System.Windows.Forms;

namespace Sage50c.API.Sample.Helpers {
    public static class SAFTHelper {
        public static void ExportSAFT(DateTime startDate, DateTime endDate, string filePath, bool bIsSimplified) {

            SAFTExportFactory factory = new SAFTExportFactory {
                SaftSimplified = bIsSimplified,
                SAFTSelfBilling = false,
                SaftType = SaftTypeEnum.SaftTypeInvoice,
                TransmissionStatus = (short)TransmissionStatusEnum.TransmissionStatusExportedForTesting,
                Version = "1.04",

                AuditFileName = filePath,
                InitialDate = startDate,
                FinalDate = endDate
            };

            var exporter = factory.GetSAFTExporter();
            if (exporter.ValidateDates()) {

                var bExported = exporter.Export(filePath);
                if (bExported) {
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"Exportado com sucesso. Ficheiro disponível em:{Environment.NewLine}{Environment.NewLine}{filePath}", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
                else {
                    APIEngine.CoreGlobals.MsgBoxFrontOffice("Não foi possível exportar o ficheiro.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
            }
            else {
                APIEngine.CoreGlobals.MsgBoxFrontOffice($"As datas indicadas não são válidas.{Environment.NewLine}Data de início: {startDate.ToShortDateString()}{Environment.NewLine}Data de fim: {endDate.ToShortDateString()}", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
        }
    }
}
