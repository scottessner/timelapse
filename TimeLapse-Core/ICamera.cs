using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeLapse_Core
{
    public interface ICamera
    {
        int UniqueID { get; }
        Frame CaptureFrame();
    }
}
