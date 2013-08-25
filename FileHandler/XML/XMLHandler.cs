/*
 * Name: Handler.XMLHandler
 * Date: 11 April 2011
 * Author: Patrik Kimmeswenger
 * Description: serves some methods to handle xml communication
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using HLib.Item.Reflection;
using System.Runtime.Serialization;

namespace HLib.File.XML
{
    /// <summary>
    /// Includes methods to handle XML
    /// </summary>
    public static class XMLHandler
    {


        /// <summary>
        /// Serializeas a list of objects
        /// </summary>
        /// <param name="_object">The object to serialize</param>
        /// <param name="_filePath">path where the xml file should be located</param>
        /// <returns></returns>
        public static void Serialize(Object _object, String _filePath)
        {
            // XML objects
            XmlWriter     xmlWriter     = null;
            XmlSerializer xmlSerializer = null;
            Type[]        propertyTypes = null;

            try
            {
                //Create XMLWriter with the specific path and ItemName
                xmlWriter = XmlWriter.Create(_filePath);
                //xmlWriter.Settings.Encoding = Encoding.UTF8;

                propertyTypes = ReflectionHandler.GetPropertyTypes(_object).ToArray();

                // Serialize the object
                xmlSerializer = new XmlSerializer(_object.GetType(), propertyTypes);
                xmlSerializer.Serialize(xmlWriter, _object);
            }

            catch (Exception exception)
            {
                throw exception;
            }

            finally
            {
                if (xmlWriter != null)
                {
                    // Close the XMLWriter
                    xmlWriter.Close();
                }
            }

        }

        /// <summary>
        /// Serializeas a list of objects
        /// </summary>
        /// <param name="_object">The object to serialize</param>
        /// <param name="_types">The types of the properties from the ListEntity</param>
        /// <param name="_filePath">path where the xml file should be located</param>
        /// <returns></returns>
        public static Boolean TrySerialize(Object _object, Type[] _types, String _filePath)
        {
            // XML objects
            XmlWriter xmlWriter;
            XmlSerializer xmlSerializer;

            //Create XMLWriter with the specific path and ItemName
            xmlWriter = XmlWriter.Create(_filePath);

            try
            {
                // Serialize the object
                xmlSerializer = new XmlSerializer(_object.GetType(), _types);
                xmlSerializer.Serialize(xmlWriter, _object);
            }

            catch
            {
                return false;
            }

            finally
            {
                // Close the XMLWriter
                xmlWriter.Close();
            }

            return true;

        }

        /// <summary>
        /// Serializeas a list of objects
        /// </summary>
        /// <param name="_object">The object to serialize</param>
        /// <param name="_filePath">path where the xml file should be located</param>
        /// <returns></returns>
        public static Boolean TrySerialize(Object _object, String _filePath)
        {
            // XML objects
            XmlWriter xmlWriter;
            XmlSerializer xmlSerializer;
            Type[] propertyTypes;

            //Create XMLWriter with the specific path and ItemName
            xmlWriter = XmlWriter.Create(_filePath);

            try
            {
                propertyTypes = ReflectionHandler.GetPropertyTypes(_object).ToArray();

                // Serialize the object
                xmlSerializer = new XmlSerializer(_object.GetType(), propertyTypes);
                xmlSerializer.Serialize(xmlWriter, _object);
            }

            catch
            {
                return false;
            }

            finally
            {
                // Close the XMLWriter
                xmlWriter.Close();
            }

            return true;

        }


        /// <summary>
        /// deserializes an object readed from an xml file
        /// </summary>
        /// 
        /// <param name="_typeName">
        /// the type of the object
        /// </param>
        /// 
        /// <param name="_filePath">
        /// path where the xml file should be located
        /// </param>
        /// 
        /// <returns>
        /// returns the object readed from the xml file
        /// </returns>
        public static Object Deserialize(String _typeName, String _filePath)
        {

            //objects
            XmlSerializer xmlSerializer;
            StreamReader streamReader;
            Object resultObject;

            // initialization
            xmlSerializer = new XmlSerializer(Type.GetType(_typeName));
            streamReader = new StreamReader(_filePath);
            resultObject = new Object();

            try
            {
                // return the object readed from the file
                resultObject = (Object)xmlSerializer.Deserialize(streamReader);

                // close the streamReader connection
                streamReader.Close();

                // return the deserialized object
                return resultObject;
            }

            catch (Exception exception)
            {
                // close the streamReader connection
                streamReader.Close();

                throw exception;
            }

        }


        /// <summary>
        /// Deserializes an object readed from an xml file
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="_filePath">The path + name of the xml file</param>
        /// <returns>The deserialized object from type T</returns>
        public static T Deserialize<T>(String _filePath)
            where T : class
        {
            //objects
            XmlSerializer xmlSerializer;
            StreamReader streamReader;
            T resultObject;

            // initialization
            if (System.IO.File.Exists(_filePath))
            {
                xmlSerializer = new XmlSerializer(typeof(T));
                streamReader = new StreamReader(_filePath);
                resultObject = ReflectionHandler.GetInstance<T>();

                try
                {
                    // return the object readed from the file
                    resultObject = (T)xmlSerializer.Deserialize(streamReader);

                    // close the streamReader connection
                    streamReader.Close();

                    // return the deserialized object
                    return resultObject;
                }

                catch (Exception exception)
                {
                    // close the streamReader connection
                    streamReader.Close();

                    throw exception;
                }
            }

            else return null;

        }

        /// <summary>
        /// Deserializes an object readed from an xml file
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="_instance">the instance which contains the deserialized object</param>
        /// <param name="_filePath">The path + name of the xml file</param>
        /// <returns>true if deserializaion was successfull otherwise false</returns>
        public static Boolean TryDeserialize<T>(ref T _instance, String _filePath)
            where T : class
        {
            //objects
            XmlSerializer xmlSerializer;
            StreamReader  streamReader;

            // initialization
            if (System.IO.File.Exists(_filePath))
            {
                xmlSerializer = new XmlSerializer(typeof(T));
                streamReader = new StreamReader(_filePath);

                try
                {
                    // return the object readed from the file
                    _instance = xmlSerializer.Deserialize(streamReader) as T;

                    // close the streamReader connection
                    streamReader.Close();
                }

                catch
                {
                    return false;
                }

                finally
                {
                    // close the streamReader connection
                    streamReader.Close();
                }

            }

            return true;
        }
    }
}
