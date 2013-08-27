using System;
using HLib.Item.Singleton;
using HLib.File.XML;
using System.Xml.Serialization;
using System.Threading;

namespace HLib.Settings.Property
{
    /// <summary>
    /// This class is the super class of all Properties.
    /// It initailzes a instance of the Propertytype as a singelton
    /// and de/serializes that instance autmatically at the point of initialisation.
    /// </summary>
    /// <typeparam name="TPropertyType">the Properytype</typeparam>
    //[SerializableAttribute] 
    //public abstract class Property<PropertyType> where PropertyType : Property<PropertyType>
    public abstract class Property<TPropertyType> where TPropertyType : Property<TPropertyType>
    {
        #region Objects

        private Boolean serializeable  = true;
        private Boolean deserilizealbe = true;

        private String outputDirectory = String.Format("{0}{1}\\", AppDomain.CurrentDomain.BaseDirectory, "Properties");
        private String fileName        = String.Empty;
        private String filePath        = String.Empty;

        #region Properties

        /// <summary>
        /// Sets if the Property should get Serialized
        /// Default value is true
        /// </summary>
        [XmlIgnore]
        protected Boolean Serializeable
        {
            get { return this.serializeable; }
            set { this.serializeable = value; }
        }

        /// <summary>
        /// Sets if the Property should get Deserialized
        /// Default value is true
        /// </summary>
        [XmlIgnore]
        protected Boolean Deserializeable
        {
            get { return this.deserilizealbe; }
            set { this.deserilizealbe = value; }
        }

        /// <summary>
        /// the property instance
        /// </summary>
        [XmlIgnore]
        private static volatile TPropertyType instance;

        /// <summary>
        /// The Directory where the Property is situated. Default is AssemblyDirectory
        /// </summary>
        [XmlIgnore]
        protected String OutputDirectory
        {
            get { return this.outputDirectory; }
            set { this.outputDirectory = value; }
        }

        /// <summary>
        /// the absolut filepath + ItemName + filetype
        /// </summary>
        [XmlIgnore]
        protected String FilePath
        { 
            get 
            { 
                if(String.IsNullOrEmpty(this.filePath))
                {
                    this.filePath = String.Format("{0}{1}.xml", this.outputDirectory, this.Name); 
                }
                return this.filePath;
            }

            set
            {
                this.filePath = value;
            }
        }

        protected String FileName
        { 
            get 
            { 
                if(String.IsNullOrEmpty(this.fileName))
                {
                    this.fileName = String.Format("{0}.xml",this.Name);
                }
                return this.fileName;
            } 
            
            set
            {
                this.fileName = value;
            }
        }

        /// <summary>
        /// the ItemName of the Property
        /// </summary>
        [XmlIgnore]
        protected String Name
        { get { return this.GetType().Name; } }

        #endregion

        #endregion // Objectes

        #region Deconstructor
        ~Property()
        {
            if (this.Serializeable)
            {
                this.Serialize(true);
            }
        }
        #endregion Deconstructor

        #region Methods

        /// <summary>
        /// Initializes a new property instance as singelton
        /// and de/serializes ist.
        /// </summary>
        /// <returns>the de/serialized property instance</returns>
        private static TPropertyType GetInstance(out Boolean _new)
        {
            // get the instance of the property as singelton
            Property<TPropertyType>.instance = SingletonProvider.GetInstance<TPropertyType>(out _new);

            // if the instance is not already serialized 
            // serialize it
            if (!System.IO.File.Exists(instance.FilePath) && Property<TPropertyType>.instance.Serializeable)
                Property<TPropertyType>.instance.Serialize();

            // If the instance is new initialized
            // deserialize it
            else if (_new && Property<TPropertyType>.instance.Deserializeable)
                Property<TPropertyType>.instance.Deserialize();

            // return the de/serialized instance
            return Property<TPropertyType>.instance;
        }

        /// <summary>
        /// Initializes a new property instance as singelton
        /// and de/serializes ist.
        /// </summary>
        /// <param name="_new">include the information if the instance is new or not</param>
        /// <param name="_filePath">The path where the file of the instance is situated.</param>
        /// <returns>the de/serialized property instance</returns>
        private static TPropertyType GetInstance(out Boolean _new, String _filePath)
        {

            // get the instance of the property as singelton
            Property<TPropertyType>.instance = SingletonProvider.GetInstance<TPropertyType>(out _new);
            Property<TPropertyType>.instance.FilePath = _filePath;

            // if the instance is not already serialized 
            // serialize it
            if (!System.IO.File.Exists(instance.FilePath) && Property<TPropertyType>.instance.Serializeable)
                Property<TPropertyType>.instance.Serialize();

            // If the instance is new initialized
            // deserialize it
            else if (_new && Property<TPropertyType>.instance.Deserializeable)
                Property<TPropertyType>.instance.Deserialize();


            // return the de/serialized instance
            return Property<TPropertyType>.instance;
        }


        /// <summary>
        /// Initializes a new property instance as singelton
        /// and de/serializes ti.
        /// </summary>
        /// <returns>the de/serialized property instance</returns>
        public static TPropertyType GetInstance()
        {
            Boolean trash;

            return GetInstance(out trash);
        }

        /// <summary>
        /// Initializes a new property instance as singelton
        /// and de/serializes ist.
        /// </summary>
        /// <returns>the de/serialized property instance</returns>
        public static TPropertyType GetInstance(String _filePath)
        {
            Boolean trash;

            return GetInstance(out trash, _filePath);
        }

        /// <summary>
        /// serializes the property
        /// </summary>
        public void Serialize()
        {
            if (!System.IO.Directory.Exists(this.outputDirectory))
            {
                System.IO.Directory.CreateDirectory(this.outputDirectory);
            }

            try
            {
                XMLHandler.Serialize(this, this.FilePath);
            }

            catch
            {
                Console.WriteLine("Unable to create xml file of property {0}. Using the default values", this.Name);
            }
        }

        public void Serialize(Boolean _threaded)
        {
            if (_threaded)
            {
                Thread serializeThread = new Thread(this.Serialize);

                serializeThread.Start();
            }

            else
            {
                this.Serialize();
            }
        }

        /// <summary>
        /// deserilaize the property
        /// </summary>
        public void Deserialize()
        {
            TPropertyType propertyType = instance;
            if (XMLHandler.TryDeserialize(ref propertyType, this.FilePath))
                SingletonProvider.SetInstance(instance);
        }

        public void Deserialize(Boolean _threaded)
        {
            if (_threaded)
            {
                Thread deserializeThread = new Thread(this.Deserialize)
                {
                    Name = String.Format("DeserializeThread_{0}", this.Name)
                };

                deserializeThread.Start();
            }

            else
            {
                this.Deserialize();
            }
        }

        #endregion
    }
}
