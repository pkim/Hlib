using System;
using HLib.Settings.Property;

namespace HLib.Settings.Language
{
    public class PropertyConfig : Property<PropertyConfig>
    {
        public String LanguageFileLocation { get; set; }

        private PropertyConfig()
        {
            this.LanguageFileLocation = this.OutputDirectory;
        }
    }
}
