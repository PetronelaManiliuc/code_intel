using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Kata_Invoicing.Infrastructure.Helpers;
using log4net;

namespace Kata_Invoicing.Infrastructure.PersistanceManager
{
    public static class PersistanceManager
    {
        #region PrivateFields
        private static ILog _log = LogManager.GetLogger(typeof(PersistanceManager));
        private static readonly string ConnectionString;
        #endregion

        #region Constructor
        static PersistanceManager()
        {
            if (ConfigurationManager.AppSettings["ConnectionString"] != null)
            {
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Receives an SqlCommand and executes it. Returns a list of IItems where IItem is an Interface that is implemented by the given Type. 
        /// This way we can use this method for all Types that implement IItem.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns>List of Items</returns>
        public static IList<T> ExecuteStoredProcedureQuery<T>(SqlCommand command, int commandTimeout = 600) where T : IItem
        {
            IList<T> entities = new List<T>();

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    using (command.Connection = connection)
                    {
                        connection.Open();
                        command.CommandTimeout = commandTimeout;
                        //instantiate the type
                        var type = Activator.CreateInstance(typeof(T)) as IItem;
                        if (type != null)
                        {
                            using (IDataReader dataReader = command.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    entities.Add((T)type.CreateFromReader(dataReader));
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _log.Error(ex + " Procedure: " + command.CommandText);
                //entities = null;
                throw;
            }

            return entities;
        }

        /// <summary>
        /// Executes SqlCommand Non Queries and return bool IsSuccess
        /// </summary>
        /// <param name="command"></param>
        /// <returns>IsSuccess</returns>
        public static bool ExecuteStoredProcedureNonQuery(SqlCommand command, int commandTimeout = 600)
        {
            bool IsSuccess;

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    using (command.Connection = connection)
                    {
                        connection.Open();
                        command.CommandTimeout = commandTimeout;
                        command.ExecuteNonQuery();

                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex + " Procedure: " + command.CommandText);
                IsSuccess = false;
            }

            return IsSuccess;
        }

        /// <summary>
        /// Executes SqlCommand Non Queries and return int ID (out Parameter)
        /// </summary>
        /// <param name="command"></param>
        /// <returns>ID</returns>
        public static int ExecuteStoredProcedureReturnID(SqlCommand command)
        {
            int ID;

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    using (command.Connection = connection)
                    {
                        connection.Open();

                        command.ExecuteNonQuery();

                        ID = (int)command.Parameters["@ID"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex + " Procedure: " + command.CommandText);
                ID = -1;
            }

            return ID;
        }
        public static List<string> GetSabadellImportedFiles(SqlCommand command)
        {
            List<string> fileNames = new List<string>();

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    using (command.Connection = connection)
                    {
                        connection.Open();

                        using (IDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                fileNames.Add(dataReader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex + " Procedure: " + command.CommandText);
                fileNames = null;
            }

            return fileNames;
        }

        /// <summary>
        /// Executes SqlBulkCopy for a given List of objects that implement IBulkInsert.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemsList"></param>
        /// <param name="destinationTableName"></param>
        /// <param name="batchSize">optional. default is 10.000</param>
        /// <returns></returns>
        public static bool ExecuteSqlBulkCopy<T>(List<T> itemsList, string destinationTableName, int batchSize = 10000) where T : IBulkInsert
        {
            bool isSuccess = false;

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = batchSize;
                        bulkCopy.DestinationTableName = destinationTableName;

                        var type = Activator.CreateInstance(typeof(T)) as IBulkInsert;
                        if (type != null)
                        {
                            foreach (var item in type.GetTableColumnMappings())
                            {
                                bulkCopy.ColumnMappings.Add(item.Key, item.Value);
                            }

                            try
                            {
                                bulkCopy.WriteToServer(itemsList.AsDataTable());
                                transaction.Commit();
                                isSuccess = true;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                _log.Error(ex + " ExecuteSqlBulkCopy--> " + " DestinationTableName: " + destinationTableName);
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex + " ExecuteSqlBulkCopy--> " + " DestinationTableName: " + destinationTableName);
                throw;

            }

            return isSuccess;
        }

        #endregion
    }
}
