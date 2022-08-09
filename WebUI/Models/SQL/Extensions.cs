using Inv.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Data
{
    public static class Extensions
    {
        public static List<T> ToList<T>(this DataTable source)
        {
            var genericType = typeof(T);
            List<T> finalResult = new List<T>();
            foreach (DataRow row in source.Rows)
            {
                var result = Activator.CreateInstance(genericType);
                foreach (DataColumn column in source.Columns)
                {
                    var property = genericType.GetProperty(column.ColumnName);
                    if (property == null)
                        continue;
                    var value = row[column].ToString();
                    List<object> parameters = new List<object>();
                    parameters.Add(value.ToPropertyType(property));
                    property.GetSetMethod().Invoke(result, parameters.ToArray());
                }
                finalResult.Add((T)result);
            }

            return finalResult;
        }


        public static object ToPropertyType(this object source,PropertyInfo property)
        {

            string name = property.PropertyType.Name.ToLower();
            object result = null;
            switch (name)
            {
                case "string":
                    result = source.ToString();
                    break;
                case "int32":
                    result = int.Parse(source.ToString());
                    break;
                case "bool":
                    result = (bool)result;
                    break;
                default:
                    break;
            }

            return result;
        }
         


        public static string ColumnType(this SqlColumns column, SqlEntities db)
        {
            var type = db.SqlTypes.Where(f => f.system_type_id == column.system_type_id && f.name != "").FirstOrDefault();
            string result = "string";
            switch (type.name)
            {
                case "nvarchar":
                    result = "string";
                    break;

                case "int":
                case "bitint":
                case "smallint":
                case "tinyint":
                case "numeric":
                case "decimal":
                case "float":
                    result = "number";
                    break;

                case "bit":
                    result = "boolean";
                    break;

                case "datetime":
                case "smalldatetime":
                case "date":
                case "time":
                    result = "string";
                    break;
                default:
                    result = "any";
                    break;
            }

            return result;
        }

        public static string ColumnType(this DataColumn column)
        {
            string typeName = column.DataType.Name.ToLower();
            string result = "string";
            switch (typeName)
            {
                case "string":
                    result = "string";
                    break;

                case "int32":
                case "int64":
                case "short":
                case "int16":
                case "decimal":
                case "double":
                case "float":
                    result = "number";
                    break;

                case "boolean":
                    result = "boolean";
                    break;

                case "datetime":
                case "smalldatetime":
                case "date":
                case "time":
                    result = "string";
                    break;
                default:
                    result = "any";
                    break;
            }
            return result;
        }
    }
}
