using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bussiness
{
    public class GoogleOffset
    {
        public static int getMars(ref double lng, ref double lat)
        {
            double x = 0, y = 0;
            getOffset(ref x, ref y);
            lng += x;
            lat += y;
            return 0;
        }

        private static void getOffset(ref double x, ref double y)
        {
            x = -0.003876;
            y = 0.002664;
        }
    }
}
