/*  
 *  Author: Patrik Kimmeswenger
 *  
 *  Created:    23.October 2011
 * 
 *  Description: 
 *  
 *      This class has methods and properties which gives you options to read and write to an datatbase
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Expression;
using System.Collections;


namespace DatabaseHandler.Hibernate
{

    /// <summary>
    /// This class has methods and properties which gives you options to read and write to an datatbase
    /// </summary>
    public class HibernateManager
    {

        #region Objects

        /// <summary>
        /// contains the session of the hibernateConnector
        /// </summary>
        private ISession session;

        /// <summary>
        /// contains the configuration of the hibernateConnector
        /// </summary>
        private Configuration configuration;

        #endregion


        #region Properties
        
        /// <summary>
        /// The session which which contains the connection to the database
        /// </summary>
        public ISession Session
        {
            get
            {
                if (this.session == null || !this.session.IsConnected)
                    this.connect();

                return this.session;
            }
        }

        /// <summary>
        /// returns true if the sesssion is connected
        /// </summary>
        public Boolean IsConnected
        {
            get { return this.session.IsConnected; }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// HibernateHandler default constructor
        /// </summary>
        public HibernateManager()
        {

        }

        #endregion


        #region Methods

        /// <summary>
        /// Connect to an given database
        /// </summary>
        /// <returns>true if the connecting was successfull or false if not</returns>
        public Boolean connect()
        {
            
            // create configuration
            this.configuration = new Configuration();

            // add classes (with mapping attributes)
            this.configuration.AddInputStream(HbmSerializer.Default.Serialize(Assembly.GetExecutingAssembly()));

            //create session
            ISessionFactory sessionFactory = this.configuration.BuildSessionFactory();
            this.session = sessionFactory.OpenSession();

            if (this.session.IsConnected)
                return true;

            else
                return false;
        }

        /// <summary>
        /// Disconnets the this object from the database
        /// </summary>
        public void disconnect()
        {
            if (this.session == null || !this.session.IsConnected)
                return;

            this.session.Disconnect();
        }

        /// <summary>
        /// Create the DataBase, based on the configurations in the app.conf
        /// </summary>
        public void createSchema()
        {
            if (this.session == null || !this.session.IsConnected)
                this.connect();

            SchemaExport schemaExport = new SchemaExport(this.configuration);
            schemaExport.Execute(true, true, false, true);
        }

    
        /// <summary>
        /// inserts a entity from generic class EntityType into the database
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_entity">the entity which should be insert into the database</param>
        public void insert<EntityType>(Object _entity)
        {
            ITransaction transaction = this.session.BeginTransaction();

            this.session.Save((EntityType)_entity);
            this.session.Flush(); 
            
            transaction.Commit();
        }


        /// <summary>
        /// update the entity
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_entity">the entity which should be insert into the database</param>
        public void update<EntityType>(Object _entity)
        {
            ITransaction transaction = this.session.BeginTransaction();

            this.session.Update((EntityType)_entity);
            this.session.Flush();

            transaction.Commit();
        }


        /// <summary>
        /// update the entity with the ID _id
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_entity">the entity which should be insert into the database</param>
        /// <param name="_id">the ID of the entity to update</param>
        public void update<EntityType>(Object _entity, Int32 _id)
        {
            ITransaction transaction = this.session.BeginTransaction();

            this.session.Update((EntityType)_entity, _id);
            this.session.Flush();

            transaction.Commit();
        }


        /// <summary>
        /// Deletes a Entity in the database
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_entity">the entity which should be insert into the database</param>
        public void delete<EntityType>(Object _entity)
        {
            ITransaction transaction = this.session.BeginTransaction();

            this.session.Delete((EntityType)_entity);

            transaction.Commit();
        }

        /// <summary>
        /// Deletes a Entity in the database
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_id">deletes the entity with the ID _id</param>
        public void delete<EntityType>(Int32 _id)
        {
            ITransaction transaction = null;

            try
            {
                transaction = this.session.BeginTransaction();

                EntityType entity = this.get_Entity_ByID<EntityType>(_id);

                this.session.Delete(entity);
            }
            catch (HibernateException)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }

            transaction.Commit();
        }


        /// <summary>
        /// Returns the entity with the ID _id
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_id">The ID of the entity which this method should return</param>
        /// <returns>The entity with the ID _id</returns>
        public EntityType get_Entity_ByID<EntityType>(Int32 _id)
        {
            ITransaction transaction = null;

            EntityType entity = default(EntityType);

            try
            {
                transaction = this.session.BeginTransaction();
                entity = (EntityType)this.session.Get(typeof(EntityType), _id);
            }

            catch (HibernateException)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }

            return entity;
        }


        /// <summary>
        /// Returns the entities with the name _name
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_name">the name of the entities which this method should return</param>
        /// <returns>Returns a List with entities which Name propertie is _name</returns>
        public IList<EntityType> get_Entity_ByProperty<EntityType>(String _property, object _value)
        {
            ITransaction transaction = null;

            IList<EntityType> entities = null;

            try
            {
                transaction = this.session.BeginTransaction();

                entities = this.session.CreateCriteria(typeof(EntityType))
                                    .Add(Expression.Like(_property, _value))
                                    .List<EntityType>();
   
            }

            catch (HibernateException)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }

            return entities;
        }


        /// <summary>
        /// Select all Entities in the database from type EntityType and return them in form of a List
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <returns>Returns all Entities in the database from type EntityType</returns>
        public IList<EntityType> get_Entities<EntityType>()
        {

            ITransaction transaction  = null;

            IList<EntityType> entities = null;


            try
            {
                transaction = this.session.BeginTransaction();
                entities = this.session.CreateCriteria(typeof(EntityType)).List<EntityType>();
            }

            catch (HibernateException)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }

            return entities;
        }


        /// <summary>
        /// Check if an entity with the ID _id exists
        /// </summary>
        /// <typeparam name="EntityType">the type of the entity</typeparam>
        /// <param name="_id">the ID of the entity</param>
        /// <returns>True if that entity exists in the database or false if not</returns>
        public Boolean entity_Exits<EntityType>(Int32 _id)
        {
            if (this.get_Entity_ByID<EntityType>(_id) != null)
                return true;

            return false;

        }

        #endregion

    }
}
