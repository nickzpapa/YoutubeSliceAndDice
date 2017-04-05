using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SliceAndDiceWeb.Models
{
    public class SliceException : System.Exception
    {
        public SliceException() : base() { }
        public SliceException(string message) : base(message) { }

    }

}