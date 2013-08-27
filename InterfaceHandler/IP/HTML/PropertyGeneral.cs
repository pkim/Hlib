using System;
using HLib.Settings.Property;

namespace HLib.Network.IP.HTML
{
    public class PropertyGeneral : Property<PropertyGeneral>
    {
        public Int64 AmountOfDownloads { get; set; } 

        private PropertyGeneral()
        {
            this.AmountOfDownloads = new Int64();
        }

        ~PropertyGeneral()
        {
            this.Deserialize(true);
        }
    }
}
