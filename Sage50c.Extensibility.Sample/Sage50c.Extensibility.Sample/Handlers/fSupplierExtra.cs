using S50cBL22;
using stdole;
using System.Drawing;
using System.Windows.Forms;

namespace Sage50c.ExtenderSample22.Handlers {
    public partial class fSupplierExtra : Form, IChildPanel {

        public int Handler { get { return Handle.ToInt32(); } }

        public StdPicture Picture { get { return (StdPicture)ImageConverter.GetIPictureDispFromImage(Icon.ToBitmap()); } }

        public string Title { get; }

        public string Description { get; }

        public fSupplierExtra(string title = "My title", string description = "My Description") {
            InitializeComponent();

            Title = title;
            Description = description;
        }

        public void SetSize(float Width, float Height) {
            try {
                var width = (int)Microsoft.VisualBasic.Compatibility.VB6.Support.TwipsToPixelsX(Width);
                var height = (int)Microsoft.VisualBasic.Compatibility.VB6.Support.TwipsToPixelsY(Height);

                SetBounds(0, 0, width, height);
                Visible = true;
            }
            catch { }
        }

        public void SetFocus() {
            try {
                SetFocus();
            }
            catch { }
        }

        public bool OnMenuItem(string MenuItemID) {
            return true;
        }

        public bool CheckIDValue(string value) {
            return true;
        }

        public void SetBackcolor(int value) {
            BackColor = Color.FromArgb(value);
        }

        public bool BeforeOk() {
            return true;
        }

        public bool BeforeCancel() {
            return true;
        }
    }
}
