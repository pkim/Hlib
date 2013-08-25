using System;
using Handler.Settings.Property;

namespace Handler.Settings.Language
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
