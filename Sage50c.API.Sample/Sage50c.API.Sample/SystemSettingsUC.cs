using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Sage50c.API.Sample
{
    public partial class SystemSettingsUC : UserControl
    {
        public SystemSettingsUC()
        {
            InitializeComponent();
        }

        private void BGetSystemSettings_Click(object sender, EventArgs e)
        {
            var systemSettings = APIEngine.SystemSettings;

            TbSystemSettingsList.Text = JToken.FromObject(systemSettings, APIEngine.Serializer).ToString();
        }
    }
}
