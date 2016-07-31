using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SliceAndDiceWeb
{
    public class DBug
    {
        public static void Echo (string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}