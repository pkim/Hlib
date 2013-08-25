using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handler.Handler.Settings.Language;
using Handler.Settings.Property;
using Handler.Handler.Item.Serialization;

namespace Handler.Settings.Language
{
    public class PropertyLanguageCode : Property<PropertyLanguageCode>
    {
        public SerializableDictionary<LanguageID, String> Dictionary { get; set; }

        private PropertyLanguageCode()
        {
            this.Dictionary = new SerializableDictionary<LanguageID, String>();

            this.Dictionary.Add(LanguageID.UNKNOWN, String.Empty);
            this.Dictionary.Add(LanguageID.GERMAN , "DE");
            this.Dictionary.Add(LanguageID.ENGLISH, "EN");
        }
    }
}
