using System;
using System.Windows.Forms;

namespace Sage50c.API.Sample
{
    public partial class JSONForm : Form
    {
        public string JSON
        {
            get
            {
                if (string.IsNullOrEmpty(TbJSON.Text))
                {
                    return string.Empty;
                }
                else
                {
                    return TbJSON.Text;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    TbJSON.Text = string.Empty;
                }
                else
                {
                    TbJSON.Text = value;
                }
            }
        }

        public JSONForm()
        {
            InitializeComponent();
        }

        private void BOk_Click(object sender, EventArgs e)
        {
            JSON = TbJSON.Text;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
