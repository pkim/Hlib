/* *****************************************
 * Author:  Patrik Kimmeswenger
 * 
 * Date:    25.01.2012
 * 
 * Description:
 * 
 *  This class serves differnt methods to get values, types or instances from a type
 * 
 * *****************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Handler.Item.Reflection
{
    public static class ReflectionHandler
    {

        public static void Clone<T>(T _source, T _destination)
        {
            Type typeOfSource      = _source.GetType();
            Type typeOfDestinaiton = _destination.GetType();

            PropertyInfo[] propertyInfoSource = typeOfSource.GetProperties();

            foreach (PropertyInfo sourceProperty in propertyInfoSource)
            {
                PropertyInfo propertyDestination = typeOfDestinaiton.GetProperty(sourceProperty.Name);
                propertyDestination.SetValue(_destination, sourceProperty.GetValue(_source, null), null);
            }
        }

        public static List<PropertyEntity> GetValues(Object _object)
        {
            List<PropertyEntity> values = new List<PropertyEntity>();

            PropertyInfo[] properties = _object.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                PropertyEntity propertyEntity = new PropertyEntity();

                // set Value
                propertyEntity.Value = property.GetValue(_object, null);

                // set Name
                propertyEntity.Name = property.Name;

                // set Type
                propertyEntity.Type = property.PropertyType;

                values.Add(propertyEntity);
            }

            return values;
        }

        /// <summary>
        /// Specifies the Properties of a Class
        /// </summary>
        /// <typeparam name="T">The type of the class</typeparam>
        /// <returns>The Properties as List</returns>
        public static List<PropertyInfo> GetProperties<T>()
        {
            // get all properties of generic Class T
            return typeof(T).GetProperties().ToList<PropertyInfo>();
        }


        public static List<PropertyInfo> GetProperties<T>(Boolean _sortByName)
            where T : class
        {
            PropertyInfo[] propertyInfos;

            // get all properties of generic Class T
            propertyInfos = typeof(T).GetProperties();


            // if the user wants to sort the properties by name
            if (_sortByName)
            {
                // sort properties by name
                Array.Sort(propertyInfos,
                    delegate(PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                    { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });
            }

            return propertyInfos.ToList<PropertyInfo>();
        }

        public static List<String> GetPropertyNames<T>()
        {
            List<String> propertyNames = new List<String>();
            PropertyInfo[] propertyInfos;

            // get all properties of generic Class T
            propertyInfos = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyNames.Add(propertyInfo.Name);
            }

            return propertyNames;
        }

        public static List<Type> GetPropertyTypes<T>()
        {
            List<Type> propertyTypes = new List<Type>();
            PropertyInfo[] propertyInfos;

            // get all properties of generic Class T
            propertyInfos = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyTypes.Add(propertyInfo.PropertyType);
            }

            return propertyTypes;
        }

        public static List<Type> GetPropertyTypes(Object _object)
        {
            List<Type> propertyTypes = new List<Type>();
            PropertyInfo[] propertyInfos;

            // get all properties of _object
            propertyInfos = _object.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyTypes.Add(propertyInfo.PropertyType);
            }

            return propertyTypes;
        }

        public static Object GetPropertyInstance<T>(String _propertyName)
        {
            PropertyInfo propertyInfo = GetProperties<T>().First(f => f.Name == _propertyName);

            return Activator.CreateInstance(propertyInfo.PropertyType);
        }

        public static T GetInstance<T>()
            where T : class
        {
            // Create new instance with reflection
            ConstructorInfo ctorInfo;
            T instanz;

            // Read protected construtor
            ctorInfo = typeof(T).GetConstructor(
                        BindingFlags.Public |
                        BindingFlags.Instance,
                        null,
                        Type.EmptyTypes,
                        null
                      );

            // run construtor without any parameters
            instanz = (T)ctorInfo.Invoke(new object[] { });

            return instanz;
        }

        public static T GetCustomAttribute<T>(this Object _objectItem)
            where T : Attribute
        {
            return GetCustomAttribute<T>(_objectItem, false);
        }

        public static T GetCustomAttribute<T>(this Object _objectItem, bool _inherit)
            where T : Attribute
        {

            T attribute = null;

            Object[] attributes = _objectItem.GetType().GetCustomAttributes(typeof(T), _inherit);

            if (attributes.Length == 1)
            {
                attribute = (T)attributes[0];
            }

            return attribute;
        }

        public static T GetCustomAttribute<T>(PropertyInfo _property, bool _inherit)
            where T : Attribute
        {

            T attribute = null;

            Object[] attributes = _property.GetCustomAttributes(typeof(T), _inherit);

            if (attributes.Length == 1)
            {
                attribute = (T)attributes[0];
            }

            return attribute;
        }
    }
}
