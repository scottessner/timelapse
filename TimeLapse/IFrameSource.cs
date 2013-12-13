using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeLapse
{
    public interface IFrameSource
    {
        string Manufacturer { get;}
        string ModelNumber { get;}
        int WidthNative { get;}
        int HeightNative { get;}
        Frame CaptureFrame();
    }
}
