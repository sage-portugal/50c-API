using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sage.S50c.API.Sample {
    public static class Extensions {
        public static double ToDouble(this string value) {
            double result = 0;
            if (!double.TryParse(value, out result)) {
                result = 0;
            }
            return result;
        }

        public static short ToShort(this string value) {
            short result = 0;
            if (!short.TryParse(value, out result)) {
                result = 0;
            }
            return result;
        }

        public static DateTime ToDateTime(this string value) {
            DateTime result = DateTime.Now;
            if (!DateTime.TryParse(value, out result)) {
                result = new DateTime(1899,12,30);
            }
            return result;
        }

        public static DateTime ToDateTime(this string value, DateTime DefaultValue) {
            DateTime result = DefaultValue;
            if (!DateTime.TryParse(value, out result)) {
                result = DefaultValue;
            }
            return result;
        }
    }
}
