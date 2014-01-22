using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace System.Diagnostics
{
    public partial class DebugExtension
    {
        public static void TimeStampedWriteLine(string message)
        {
            DateTime now = DateTime.Now;
            string outMessage = now.ToShortDateString() + " " + now.ToLongTimeString() + ": " + message;
            Debug.WriteLine(outMessage);
        }
    }
}
