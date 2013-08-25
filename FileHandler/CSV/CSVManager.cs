using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using HLib.Item.Reflection;

namespace HLib.File.CSV
{
    public class CSVManager
    {
        #region Methods

        /// <summary>
        /// Reads and splits the given csv file
        /// </summary>
        /// <param name="_csvData">The data to split</param>
        /// <param name="_seperator">The char which is the seperator between the values</param>
        /// <returns>A List which contains the rows as String Array which contains the values</returns>
        public static List<String[]> Get(String _csvData, Char _seperator)
        {
            List<String[]> rows = null;

            using (StreamReader streamReader = new StreamReader(_csvData))
            {
                try
                {
                    String[] row;

                    while (streamReader.Peek() != -1)
                    {
                        row = streamReader.ReadLine().Split(_seperator);

                        rows.Add(row);
                    }
                }

                catch (IOException ioexeption)
                { System.Console.WriteLine(ioexeption.Message); }
            }

            return rows;

        }


        /// <summary>
        /// Reads and splits the given csv file
        /// </summary>
        /// <param name="_csvData">The data to split</param>
        /// <param name="_seperator">The char which is the seperator between the values</param>
        /// <returns>A List which contains the rows as T Object</returns>
        public static List<T> Get_FromFile<T>(String _filePath, Char _seperator)
            where T : class
        {
            List<T> rows = new List<T>();

            List<PropertyInfo> properties = ReflectionHandler.GetProperties<T>();

            using (StreamReader streamReader = new StreamReader(_filePath))
            {
                try
                {
                    String[] row;
                    Int32 valuesCount = new Int32();

                    while (streamReader.Peek() != -1)
                    {
                        row = streamReader.ReadLine().Split(_seperator);
                        valuesCount = row.Count<String>();

                        T tupel = ReflectionHandler.GetInstance<T>();
                        Type tupelType  = tupel.GetType();

                        for (Int32 i = new Int32(); i < valuesCount; i++)
                        {
                            PropertyInfo propertyInfo = tupelType.GetProperty(properties[i].Name);

                            Object value = CSVManager.getObject(row[i], propertyInfo.PropertyType);

                            propertyInfo.SetValue(tupel, value, null);
                        }

                        rows.Add(tupel);
                    }
                }

                catch (IOException ioexeption)
                { System.Console.WriteLine(ioexeption.Message); }
            }

            return rows;
        }


        public static List<T> Get_FromData<T>(String _data, Char _seperator)
           where T : class
        {
            List<T> rows = new List<T>();

            List<PropertyInfo> properties = ReflectionHandler.GetProperties<T>();

            try
            {
                String[] row;
                Int32 valuesCount = new Int32();

                Int32 endIndex    = new Int32();

                while ((endIndex = _data.IndexOf((Char)10)) != -1)
                {
                    
                    row = _data.Substring(0, endIndex).Split(_seperator);
                    valuesCount = row.Count<String>();

                    _data = _data.Remove(0, endIndex + 1);

                    T tupel = ReflectionHandler.GetInstance<T>();
                    Type tupelType = tupel.GetType();

                    
                    for (Int32 i = new Int32(); i < valuesCount; i++)
                    {
                        PropertyInfo propertyInfo = tupelType.GetProperty(properties[i].Name);

                        Object value = CSVManager.getObject(row[i], propertyInfo.PropertyType);

                        propertyInfo.SetValue(tupel, value, null);
                    }                  

                    rows.Add(tupel);
                }
            }
            catch (IOException ioexeption)
            { System.Console.WriteLine(ioexeption.Message); }

            return rows;
        }


        private static Object getObject(String _value, Type _type)
        {

            switch (_type.Name)
            {

                case "DateTime": 
                    
                    DateTime result_DateTime;
                    if (DateTime.TryParse(_value, out result_DateTime))
                        return result_DateTime;

                    break;
                        

                case "Int32":

                    Int32 result_Int32;
                    if (Int32.TryParse(_value, out result_Int32))
                        return result_Int32;
                    
                    break;


                case "Double":

                    Double result_Double;
                    if (Double.TryParse(_value, out result_Double))
                        return result_Double;
                    
                    break;


                case "String":   return _value;


                default: return null;
            }

            return null;
        }

        #endregion // Methods
    }
}
