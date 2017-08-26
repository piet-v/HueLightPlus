using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightPlus
{
    struct Led
    {
        public int channel;
        public int ledIndex;

        public Led(int channel, int ledIndex)
        {
            this.channel = channel;
            this.ledIndex = ledIndex;
        }
    }

    struct ScreenRegion
    {
        public Led[] leds;
        public Collection<Point> coordinates;

        public ScreenRegion(Led[] leds)
        {
            this.leds = leds;
            this.coordinates = new Collection<Point>();
        }
    }

    struct ScreenSide
    {
        public ScreenRegion[] screenRegions;

        public ScreenSide(ScreenRegion[] screenRegions)
        {
            this.screenRegions = screenRegions;
        }
    }

    struct AmbiLight
    {
        public ScreenSide right;
        public ScreenSide left;
        public ScreenSide top;
        public ScreenSide bottom;

        public AmbiLight(ScreenSide right, ScreenSide top, ScreenSide left, ScreenSide bottom)
        {
            this.right = right;
            this.top = top;
            this.left = left;
            this.bottom = bottom;
        }
    }
}
