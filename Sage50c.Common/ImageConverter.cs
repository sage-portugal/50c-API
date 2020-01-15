using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sage50c.Common {
    internal class ImageConverter: System.Windows.Forms.AxHost {
        private ImageConverter(): base(string.Empty){}
        public static stdole.IPictureDisp GetIPictureDispFromImage(System.Drawing.Image image) {
            return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
        }

    }
}
