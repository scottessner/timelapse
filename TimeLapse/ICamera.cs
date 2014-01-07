using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeLapse
{
    public interface ICamera
    {
        int UniqueID { get; }
        Frame CaptureFrame();
    }
}
