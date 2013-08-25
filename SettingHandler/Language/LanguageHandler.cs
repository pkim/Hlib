using System;
using System.Collections.Generic;
using HLib.Settings.Language;
using HLib.Settings.Property;
using System.IO;


namespace Handler.HLib.Settings.Language
{
    public static class LanguageHandler<T>
        where T: Property<T>
    {
        private static PropertyConfig       PropertyConfig       { get { return PropertyConfig      .GetInstance(); } }
        private static PropertyLanguageCode PropertyLanguageCode { get { return PropertyLanguageCode.GetInstance(); } }

        private static Dictionary<Guid, T> languageDictionary = new Dictionary<Guid, T>();
        
        public static T getInstance(LanguageID _language)
        {
            Type type     = typeof(T);
            T    instance = null;

            if(!Directory.Exists(String.Format("{0}{1}",
                                            LanguageHandler<T>.PropertyConfig.LanguageFileLocation,
                                            LanguageHandler<T>.PropertyLanguageCode.Dictionary[_language]
                                            )))
            {
                Directory.CreateDirectory(String.Format("{0}{1}",
                                            LanguageHandler<T>.PropertyConfig.LanguageFileLocation,
                                            LanguageHandler<T>.PropertyLanguageCode.Dictionary[_language]
                                            ));
            }

            String langaugeFilePath = String.Format("{0}{1}\\{2}.xml",
                                LanguageHandler<T>.PropertyConfig.LanguageFileLocation,
                                LanguageHandler<T>.PropertyLanguageCode.Dictionary[_language],
                                type.Name
                                );

            if (LanguageHandler<T>.languageDictionary.ContainsKey(type.GUID))
            {
                instance = LanguageHandler<T>.languageDictionary[type.GUID]; 
            }

            else
            {
                instance = Property<T>.GetInstance(langaugeFilePath);

                LanguageHandler<T>.languageDictionary.Add(type.GUID, instance);
            }

            return instance;
        }
    }
}
