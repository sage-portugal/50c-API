using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using S50cBO18;
using S50cSys18;

namespace Sage.S50c.API.Sample {
    internal static class UIUtils {
        internal static void FillCountryCombo( ComboBox combo ){
            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DisplayMember = "CountryName";
            combo.ValueMember = "CountryId";

            var rsCountries = S50cAPIEngine.DSOCache.CountryProvider.GetCountriesRS();
            while (!rsCountries.EOF) {
                var country = new CountryCode();
                country.CountryID = (string)rsCountries.Fields["CountryId"].Value;
                country.CountryName = (string)rsCountries.Fields["CountryName"].Value;
                combo.Items.Add(country);
                rsCountries.MoveNext();
            }
            rsCountries.Close();
            rsCountries = null;
        }

        internal static void FillCurrencyCombo(ComboBox combo) {
            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DisplayMember = "Description";
            combo.ValueMember = "CurrencyId";

            var rs = S50cAPIEngine.DSOCache.CurrencyProvider.GetCurrencyActiveRS();
            while (!rs.EOF) {
                CurrencyDefinition currency = new CurrencyDefinition(){
                    CurrencyID = (string)rs.Fields["CurrencyId"].Value,
                    Description = (string)rs.Fields["Description"].Value 
                };
                combo.Items.Add(currency);
                rs.MoveNext();
            }
            rs.Close();
            rs = null;
        }

        
        internal static void FillEntityFiscalStatusCombo(ComboBox combo) {
            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DisplayMember = "Description";
            combo.ValueMember = "EntityFSId";

            var rs = S50cAPIEngine.DSOCache.TaxesProvider.GetEntityFiscalStatusRS(ZoneTypeEnum.ztNational);
            while (!rs.EOF) {
                var entityfs = new EntityFiscalStatus();
                entityfs.EntityFiscalStatusID = (short)((int)rs.Fields["EntityFiscalStatusId"].Value);
                entityfs.Description = (string)rs.Fields["Description"].Value;

                combo.Items.Add(entityfs);
                rs.MoveNext();
            }
            rs.Close();
            rs = null;
        }
    }
}
