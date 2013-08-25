using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handler.Settings.Property;

namespace Handler.Interface.NetworkHandler.IP.HTML
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
            this.deserialize(true);
        }
    }
}
