using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sage50c.API.Sample {
    internal static class Extensions {
        internal static int ToInt(this string Value){
            int result = 0;
            int.TryParse(Value, out result);
            return result;
        }
    }
}
