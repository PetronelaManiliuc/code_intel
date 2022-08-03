using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Infrastructure
{
    public static class DataHelper
    {
        #region Static Data Helper Methods

        public static DateTime GetDateTime(object value)
        {
            DateTime dateValue = DateTime.MinValue;
            if ((value != null) && (value != DBNull.Value))
            {
                if ((DateTime)value > DateTime.Parse(Constants.MinSqlDateValue))
                {
                    dateValue = (DateTime)value;
                }
            }
            return dateValue;
        }

        public static int GetInteger(object value)
        {
            int integerValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                int.TryParse(value.ToString(), out integerValue);
            }
            return integerValue;
        }

        public static decimal GetDecimal(object value)
        {
            decimal decimalValue = 0.0m;
            if (value != null && !Convert.IsDBNull(value))
            {
                decimal.TryParse(value.ToString(), out decimalValue);
            }
            return decimalValue;
        }

        public static double GetDouble(object value)
        {
            double doubleValue = 0;
            if (value != null && !Convert.IsDBNull(value))
            {
                double.TryParse(value.ToString(), out doubleValue);
            }
            return doubleValue;
        }

        public static Guid GetGuid(object value)
        {
            Guid guidValue = Guid.Empty;
            if (value != null && !Convert.IsDBNull(value))
            {
                try
                {
                    guidValue = new Guid(value.ToString());
                }
                catch
                {
                    // really do nothing, because we want to return a value for the guid = Guid.Empty;
                }
            }
            return guidValue;
        }

        public static string GetString(object value)
        {
            string stringValue = string.Empty;
            if (value != null && !Convert.IsDBNull(value))
            {
                stringValue = value.ToString().Trim();
            }
            return stringValue;
        }

        public static bool GetBoolean(object value)
        {
            bool bReturn = false;
            if (value != null && value != DBNull.Value)
            {
                bReturn = Convert.ToBoolean(value);
            }
            return bReturn;
        }

        public static T GetEnumValue<T>(object databaseValue) where T : struct
        {
            T enumValue = default(T);

            if (databaseValue != null && databaseValue.ToString().Length > 0)
            {
                object parsedValue = Enum.Parse(typeof(T), databaseValue.ToString());
                if (parsedValue != null)
                {
                    enumValue = (T)parsedValue;
                }
            }

            return enumValue;
        }

        public static string EntityListToDelimited<T>(IList<T> entities) where T : IEntity
        {
            StringBuilder builder = new StringBuilder(20);
            if (entities != null)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(",");
                    }
                    builder.Append(entities[i].Key.ToString());
                }
            }
            return builder.ToString();
        }

        public static bool ReaderContainsColumnName(DataTable schemaTable, string columnName)
        {
            bool containsColumnName = false;
            foreach (DataRow row in schemaTable.Rows)
            {
                if (row["ColumnName"].ToString() == columnName)
                {
                    containsColumnName = true;
                    break;
                }
            }
            return containsColumnName;
        }
               
        public static object GetSqlValue<T>(T value)
        {
            if (value != null)
            {
                string typeValue = value.GetType().Name;
                switch (typeValue)
                {
                    case "String":
                    case "Enum":
                        return string.Format("N'{0}'", value.ToString().Replace("'", "''"));
                    case "Guid":
                        return string.Format("'{0}'", value.ToString());
                    case "Boolean":
                        bool isBoolean;
                        return bool.TryParse(value.ToString(), out isBoolean) ? "1" : "0";
                    case "DateTime":
                        if (DateTime.Parse(value.ToString()) == DateTime.MinValue)
                        {
                            return string.Format("'{0}'",
                                Constants.MinSqlDateValue);
                        }
                        return string.Format("'{0}'", value.ToString());

                    default:
                        return value;
                }
            }
            else
            {
                return "NULL";
            }
        }
        #endregion
    }
}
