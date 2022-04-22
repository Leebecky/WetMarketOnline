using EWM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
namespace EWM.HelperClass
{
    /// <summary>
    /// This class is used to manage connections with MSSQL Database . Connect, Construct and Execute SQL Commands with the Database.
    /// </summary>
    public class DatabaseManager
    {
        #region Properties
        //? Connection String is automatically retrieved from the application's configuration file
        public static string DbConnectionString { get => ConfigurationManager.AppSettings["ConnString"]; set => DbConnectionString = value; }

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region General Methods
        //? Set the Connection String
        public static void SetConnectionString(string connString)
        {
            DbConnectionString = connString;
        }


        //? Convert the given string to snake_case
        public static string ToSnakeCase(string str)
        {
            // REF:https://stackoverflow.com/questions/18781027/regex-camel-case-to-underscore-ignore-first-occurrence#:~:text=44-,Non%2DRegex%20solution,-string%20result%20%3D%20string
            // Select(currentChar, intValueInEnumeration) => if i > 0 and currentChar is UpperCase, return _currentChar. For each return, concat 
            string snakeCase = string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
            return snakeCase.ToLower();
        }

        //? Convert the given string to PascalCase
        public static string ToPascalCase(string str)
        {
            // Select(currentChar, intValueInEnumeration) => if (i == 0) OR (i > 0 and char after currentChar is _), return currentChar in uppercase. For each return, concat 
            string pascalCase = string.Concat(str.Select((x, i) => (i == 0) || (i > 0 && str[i - 1].Equals('_')) ? x.ToString().ToUpper() : x.ToString()));
            pascalCase = pascalCase.Replace("_", "");
            return pascalCase;
        }
        #endregion

        #region getStoredProcedureParameters
        //? Call this function to retrieve the list of parameters required by a stored procedure
        public ArrayList getSpParameters(string strSpName)
        {
            SqlDataReader dr = null;
            ArrayList paramList = new ArrayList();
            try
            {
                string sqlQuery = "Select parameters.name from sys.parameters inner join sys.procedures on parameters.object_id = procedures.object_id where procedures.name = @strSpName";
                SqlCommand sqlCmd = new SqlCommand(sqlQuery);
                sqlCmd.Parameters.AddWithValue("@strSpName", strSpName);

                //dr = DatabaseManager.
                while (dr.Read())
                {
                    paramList.Add(dr.GetValue(0).ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQLManager.getStoredProcedureParameters : Exception : " + ex.Message);
            }
            finally
            {
                dr.Close();
            }
            return paramList;
        }

        public static string TrimEndWord(string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }
        #endregion

        #region SQLCommand Manager
        //? Constructs an SQL Query
        public static SqlCommand ConstructSqlCommand(string objectName, object obj, string queryType = "Select", string filterType = "Column")
        {
            SqlCommand cmd = new SqlCommand();

            var objectType = Type.GetType(objectName);
            dynamic dbObject = Activator.CreateInstance(objectType);
            dbObject = obj;

            string sqlQuery = "";
            string sqlQueryValues = "";
            string sqlQueryColumns = "";

            //int propCount = dbObject.GetType().GetProperties().Length;
            //for (int i = 0; i < propCount; i++) 

            string tblName = ToSnakeCase(objectType.Name);

            if (obj != null)
            {
                // Loop through object properties
                foreach (PropertyInfo property in dbObject.GetType().GetProperties())
                {
                    var dataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    //bool isNullable = (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
                    bool checkNotNull = false;

                    //if (!isNullable)
                    //{
                        if (dataType == typeof(String))
                        {
                            checkNotNull = (property.GetValue(dbObject) != null && property.GetValue(dbObject) != "");
                        }
                        else if (dataType == typeof(DateTime))
                        {
                            checkNotNull = (property.GetValue(dbObject) != null && property.GetValue(dbObject)!= default(DateTime));
                        }
                        else
                        {
                            checkNotNull = (property.GetValue(dbObject) != null);
                        }
                    //}

                    if (checkNotNull)
                    {
                        string colName = ToSnakeCase(property.Name);
                        if (queryType.ToLower() == "insert")
                        {
                            sqlQueryValues = string.Concat(sqlQueryValues, "@", colName, ",");
                        }
                        else if (queryType.ToLower() == "update")
                        {
                            sqlQueryValues = string.Concat(sqlQueryValues, colName, " = @", colName, ",");
                        }
                        else
                        {
                            sqlQueryValues = string.Concat(sqlQueryValues, colName, " = @", colName, " and ");
                        }

                        cmd.Parameters.AddWithValue(string.Concat("@", colName), property.GetValue(dbObject));

                        if (queryType.ToLower() != "update")
                        {
                            sqlQueryColumns = string.Concat(sqlQueryColumns, colName, ",");
                        }
                        else
                        {
                            PropertyInfo[] propPrivateInfo = dbObject.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
                            PropertyInfo oriData = (from p in propPrivateInfo where p.Name == string.Concat("Ori", property.Name) select p).FirstOrDefault();

                            bool isOriNotNull = false;
                            if (dataType == typeof(String))
                            {
                                isOriNotNull = (oriData.GetValue(dbObject) != null && oriData.GetValue(dbObject) != "");
                            }
                            else if (dataType == typeof(DateTime))
                            {
                                isOriNotNull = (oriData.GetValue(dbObject) != default(DateTime));
                            }
                            else
                            {
                                isOriNotNull = (oriData.GetValue(dbObject) != null);
                            }

                            if (!isOriNotNull) { continue; }

                            sqlQueryColumns = string.Concat(sqlQueryColumns, colName, " = @ori_", colName, " and ");

                            cmd.Parameters.AddWithValue(string.Concat("@ori_", colName), oriData.GetValue(dbObject));
                        }
                    }
                }
            }

            // Remove trailing words
            if (queryType.ToLower() == "insert" || queryType.ToLower() == "update")
            {
                sqlQueryValues = DatabaseManager.TrimEndWord(sqlQueryValues, ",");
            }
            else
            {
                sqlQueryValues = DatabaseManager.TrimEndWord(sqlQueryValues, "and ");
            }

            if (queryType.ToLower() != "update")
            {
                sqlQueryColumns = sqlQueryColumns.TrimEnd(',');
            }
            else
            {
                sqlQueryColumns = DatabaseManager.TrimEndWord(sqlQueryColumns, "and ");
            }

            switch (queryType.ToLower())
            {
                case "insert":
                    sqlQuery = string.Concat("Insert into ", tblName, "(", sqlQueryColumns, ") values(", sqlQueryValues, ")");
                    break;
                case "update":
                    sqlQuery = string.Concat("Update ", tblName, " set ", sqlQueryValues, " Where ", sqlQueryColumns);
                    break;
                case "delete":
                    sqlQuery = string.Concat("Delete from ", tblName, " where ", sqlQueryValues);
                    break;
                case "select": //-- Select ... where
                    if (filterType.ToLower() == "column")
                    {
                        sqlQuery = string.Concat("Select ", sqlQueryColumns, " from ", tblName, " where ", sqlQueryValues);
                    }
                    else
                    {
                        sqlQuery = string.Concat("Select * from ", tblName, " where ", sqlQueryValues);
                    }
                    break;
                default: // -- Select without filter
                    sqlQuery = string.Concat("Select * from ", tblName);
                    break;
            }
            cmd.CommandText = sqlQuery;
            return cmd;
        }

        //? Executes the given SQL Command and returns the datatable
        public static DataTable ExecuteQueryCommand_Datatable(SqlCommand sqlCmd)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(DbConnectionString);
            SqlDataReader dr = null;
            try
            {
                sqlCmd.Connection = conn;
                conn.Open();
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                log.Error("ExecuteCommand_Datatable: " + ex);
                throw;
            }
            finally
            {
                conn.Close();
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return dt;
        }

        //? Executes the given SQL Command and returns the object
        public static Object ExecuteQueryCommand_Object(SqlCommand sqlCmd, string objectName, string listName)
        {
            var objectType = Type.GetType(objectName);
            dynamic dbList = Activator.CreateInstance(Type.GetType(listName));

            SqlConnection conn = new SqlConnection(DbConnectionString);
            SqlDataReader dr = null;
            try
            {
                sqlCmd.Connection = conn;
                conn.Open();

                dr = sqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    dynamic dbObject = Activator.CreateInstance(objectType);
                    foreach (PropertyInfo property in dbObject.GetType().GetProperties())
                    {
                        string colName = ToSnakeCase(property.Name);
                        var dataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        dynamic propValue = null;

                        var exists = Enumerable.Range(0, dr.FieldCount).Any(i => string.Equals(dr.GetName(i), colName, StringComparison.OrdinalIgnoreCase));
                        bool isNullable = (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>));


                        if (dataType == typeof(String))
                        {
                            if (exists)
                            {
                                propValue = (dr.IsDBNull(dr.GetOrdinal(colName))) ? default(string) : dr[colName];
                            }
                            else
                            {
                                propValue = default(string);
                            }
                        }
                        else if (dataType == typeof(DateTime))
                        {
                            if (exists)
                            {
                                propValue = (!dr.IsDBNull(dr.GetOrdinal(colName))) ? dr[colName] : (isNullable) ? (DateTime?)null : default(DateTime); ;
                            }
                            else
                            {
                                propValue = (isNullable) ? (DateTime?)null : default(DateTime);
                            }
                        }
                        else if (dataType == typeof(int))
                        {
                            if (exists)
                            {
                                propValue = (!dr.IsDBNull(dr.GetOrdinal(colName))) ? dr[colName] : isNullable ? (int?)null : default(int);
                            }
                            else
                            {
                                propValue = isNullable ? (int?)null : default(int);
                            }
                        }
                          else if (dataType == typeof(Decimal))
                        {
                            if (exists)
                            {
                                propValue = (!dr.IsDBNull(dr.GetOrdinal(colName))) ?  dr[colName] : isNullable ? (decimal?)null : default(decimal);
                            }
                            else
                            {
                                propValue = isNullable ? (decimal?)null : default(decimal);
                            }
                        }
                        else if (dataType == typeof(bool))
                        {
                            if (exists)
                            {
                                propValue = (dr.IsDBNull(dr.GetOrdinal(colName))) ? default(bool) : dr[colName];
                            }
                            else
                            {
                                propValue =  default(bool);
                            }
                        }

                        property.SetValue(dbObject, propValue);

                        PropertyInfo[] propPrivateInfo = dbObject.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
                        PropertyInfo oriData = (from p in propPrivateInfo where p.Name == string.Concat("Ori", property.Name) select p).FirstOrDefault();
                        oriData.SetValue(dbObject, propValue);
                    }

                    dbList.Add(dbObject);
                }

            }
            catch (Exception ex)
            {
                log.Error("ExecuteCommand_Datatable: " + ex);
                throw;
            }
            finally
            {
                conn.Close();
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return dbList;
        }

        //? Executes the given SQL Command and returns the rows affected 
        public static int ExecuteQueryCommand_RowsAffected(SqlCommand sqlCmd, bool isSelect = false)
        {
            SqlConnection conn = new SqlConnection(DbConnectionString);

            int rowsAffected = -1;
            try
            {
                sqlCmd.Connection = conn;
                conn.Open();

                if (isSelect)
                {
                    rowsAffected = (sqlCmd.ExecuteReader().HasRows) ? 1 : 0;
                }
                else
                {
                    rowsAffected = sqlCmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                log.Error("ExecuteCommand_Datatable: " + ex);
                throw;
            }
            finally
            {
                conn.Close();
            }
            return rowsAffected;
        }
        #endregion
        //end class
    }
}