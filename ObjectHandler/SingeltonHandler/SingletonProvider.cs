using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;

namespace HLib.Item.Singleton
{

    /// <summary>
    /// This class handles singeltones. It provides that only one object from 
    /// a class can exit at the same time if that object has got initialized by GetInstance()
    /// </summary>
    public static class SingletonProvider
    {

        #region Objects

        /// <summary>
        /// InstanceKeyTable contains the GUID of each class which instance is a singelton
        /// </summary>
        private static Hashtable InstanceKeyTable = new Hashtable();

        /// <summary>
        /// This object is needed to provide that GetInstance() is threadsecure
        /// </summary>
        private static Object Lock = new Object();

        #endregion


        #region Methods

        /// <summary>
        /// Initializes a new Object from type T as singelton
        /// </summary>
        /// <typeparam name="T">dynamic Type of the singelton instance</typeparam>
        /// <param name="New">Contains the boolean value if the instance is new</param>
        /// <returns>The singelton object</returns>
        public static T GetInstance<T>(out Boolean New)
          where T : class // Jede Klasse ist erlaubt
        {
            // If the class T has a public constructor
            ConstructorInfo checkCtor = (typeof(T)).GetConstructor(Type.EmptyTypes);

            /*
             * If there exists a public constructor in these class
             * throw a Exception
             */
            if (checkCtor != null)
                throw new InvalidOperationException("Singleton means that you don't have any public constructors");

            // Threadsynchronisation
            lock (Lock)
            {

                // if the InstanceKeyTable contains the GUID key
                if (InstanceKeyTable.ContainsKey(typeof(T).GUID))
                {
                    // set New false, because the instance already exists
                    New = false;

                    // return that instance from type T
                    return InstanceKeyTable[typeof(T).GUID] as T;
                }

                // Create new instance with reflection
                ConstructorInfo ctorInfo;

                // Read protected construtor
                ctorInfo = typeof(T).GetConstructor(
                            BindingFlags.NonPublic |
                            BindingFlags.Instance,
                            null,
                            Type.EmptyTypes,
                            null
                          );

                // run construtor without any parameters
                T _Instanz = (T)ctorInfo.Invoke(new object[] { });
                
                // add instance to hashtable InstanceKeyTable
                InstanceKeyTable.Add(typeof(T).GUID, _Instanz);

                // set New true, because the instance is new
                New = true;

                // return that instance
                return (T)InstanceKeyTable[typeof(T).GUID];
            }
        }

        /// <summary>
        /// Initializes a new Object from type T as singelton 
        /// </summary>
        /// <typeparam name="T">The type of the singelton instance</typeparam>
        /// <returns>The singelton object</returns>
        public static T GetInstance<T>()
          where T : class
        {
            Boolean _Trash;
            return GetInstance<T>(out _Trash);
        }


        /// <summary>
        /// Set the instance
        /// </summary>
        /// <typeparam name="T">The tpye of the singelton instance</typeparam>
        /// <param name="_instance">the instance which resets the instance</param>
        public static void SetInstance<T>(T _instance)
            where T : class
        {

            // Threadsyncronisation
            lock (Lock)
            {
                // if the instance from type T already exists 
                if (SingletonProvider.InstanceKeyTable.ContainsKey(typeof(T).GUID))
                {
                    // set instance
                    InstanceKeyTable[typeof(T).GUID] = _instance as T;
                    
                }
            }
        }

        #endregion

    }

}
