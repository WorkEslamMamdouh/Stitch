﻿
using Inv.WebUI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Text;
using System.Data;
using System.Dynamic;
using System.Data.SqlClient;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Inv.WebUI.Controllers
{//eslam 1 dec 2020
    public class GeneralSQLController : Controller
    {

        public class ModelSql
        {
            public SqlTables sqlTables = new SqlTables();
            public SqlEnt sqlEnt = new SqlEnt();
            public List<object> Model = new List<object>();
        } 
        public class ClassName
        {
            public Dictionary<string, object> Dictionary { get; set; }
        }

        SqlEntities db = new SqlEntities();
        dynamic newobj = new ExpandoObject();
        public JsonResult CounactData(SqlEnt rp)
        {


            db.Server = rp.Server;
            db.Database = rp.Database;
            db.User = rp.User;
            db.Password = rp.Password;

            List<SqlTables> Tab = new List<SqlTables>();

            Tab = db.SqlTables;

            return Json(Tab, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GenerateModelsTest(string RepP)
        {
            ModelSql rp = JsonConvert.DeserializeObject<ModelSql>(RepP);

            db.Server = rp.sqlEnt.Server;
            db.Database = rp.sqlEnt.Database;
            db.User = rp.sqlEnt.User;
            db.Password = rp.sqlEnt.Password;

            List<SqlTables> Tab = new List<SqlTables>();

            Tab = db.SqlTables;


            StringBuilder models = new StringBuilder();
            var table = rp.sqlTables;

            // as List<SqlTables>;// db.SqlTables;
            //foreach (SqlTables table in tables)
            //{


            models.AppendLine("{");


            models.Append(GenerateConstructorCast(table));

            models.AppendLine("}");

            //var columns = db.SqlColumns.Where(f => f.object_id == table.object_id).ToList();
            //foreach (SqlColumns column in columns)
            //{
            //    models.Append(column.name).Append(" = ").Append(column.ColumnType(this.db)).AppendLine();
            //}


            //}

            //txtModels.Text = models.ToString();

            return Json(models.ToString(), JsonRequestBehavior.AllowGet);


        }
        
        public JsonResult ShowData(string RepP)
        {
            ModelSql rp = JsonConvert.DeserializeObject<ModelSql>(RepP);

            db.Server = rp.sqlEnt.Server;
            db.Database = rp.sqlEnt.Database;
            db.User = rp.sqlEnt.User;
            db.Password = rp.sqlEnt.Password;
           string NameTable = rp.sqlTables.name;
             
            StringBuilder models = new StringBuilder();


            using (SqlConnection connection = new SqlConnection("Data source = " + db.Server + " ; Initial catalog = " + db.Database + " ; User id = " + db.User + "; Password = " + db.Password + ";"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    connection.Open();
                    command.CommandText = "select * from " + NameTable + "";

                    SqlDataReader reader = command.ExecuteReader();
                     
                    var columns = db.SqlColumns.Where(f => f.object_id == rp.sqlTables.object_id).ToList();
                     
                    int falgfrist = 0;
                    int falgfristM = 0;
                     
                     
                    models.AppendLine("  [ "); 
                    while (reader.Read())
                    {

                        if (falgfristM == 0)
                        {
                            models.AppendLine("{");

                        }
                        else
                        {
                            models.AppendLine(",{");
                        }

                        falgfrist = 0;
                        foreach (SqlColumns column in columns)
                        {

                            if (falgfrist == 0)
                            {
                                models.AppendLine("\"" + column.name + "\"" +" : "+ "\"" + reader["" + column.name + ""] + "\"");
                            }
                            else
                            {
                                models.AppendLine(",\"" + column.name + "\"" + " : " + "\"" + reader["" + column.name + ""] + "\"");
                            }

                            falgfrist = 1; 

                        }
                         
                        models.AppendLine("}"); 
                        falgfristM = 1; 
                    }
                     
                    models.AppendLine(" ] ");
                     
                    connection.Close();
                    command.Dispose();
                    connection.Dispose(); 

                }


            }





            return Json(models.ToString(), JsonRequestBehavior.AllowGet);


        }



        public void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            var exDict = expando as IDictionary<string, object>;
            if (exDict.ContainsKey(propertyName))
                exDict[propertyName] = propertyValue;
            else
                exDict.Add(propertyName, propertyValue);
        }









        public JsonResult GenerateModels(string RepP)
        {
            ModelSql rp = JsonConvert.DeserializeObject<ModelSql>(RepP);

            db.Server = rp.sqlEnt.Server;
            db.Database = rp.sqlEnt.Database;
            db.User = rp.sqlEnt.User;
            db.Password = rp.sqlEnt.Password;

            List<SqlTables> Tab = new List<SqlTables>();

            Tab = db.SqlTables;


            StringBuilder models = new StringBuilder();
            var table = rp.sqlTables;

            // as List<SqlTables>;// db.SqlTables;
            //foreach (SqlTables table in tables)
            //{

            models.Append("class ").Append(table.name).AppendLine("{");


            models.Append(GenerateConstructor(table));

            var columns = db.SqlColumns.Where(f => f.object_id == table.object_id).ToList();
            foreach (SqlColumns column in columns)
            {
                models.Append("public ").Append(column.name).Append(":").Append(column.ColumnType(this.db)).Append(";").AppendLine();
            }

            //models.AppendLine("}").AppendLine();
            //}

            //txtModels.Text = models.ToString();

            return Json(models.ToString(), JsonRequestBehavior.AllowGet);


        }

        public JsonResult InsetData(string RepP)
        {
            ModelSql rp = JsonConvert.DeserializeObject<ModelSql>(RepP);

            db.Server = rp.sqlEnt.Server;
            db.Database = rp.sqlEnt.Database;
            db.User = rp.sqlEnt.User;
            db.Password = rp.sqlEnt.Password;
            string NameTable = rp.sqlTables.name;
            var table = rp.sqlTables; 


            StringBuilder models = new StringBuilder();

             

            using (SqlConnection connection = new SqlConnection("Data source = " + db.Server + " ; Initial catalog = " + db.Database + " ; User id = " + db.User + "; Password = " + db.Password + ";"))
            {
                 
                using (SqlCommand command = new SqlCommand())
                {

                    //--------------------------------------------------InsetData---------------------------------------




                    AssignAndSave(rp.Model, table);


                    //------------------------------------------------ShowData--------------------------------------------

                    command.Connection = connection;

                    connection.Open();
                    command.CommandText = "select * from " + NameTable + "";

                    SqlDataReader reader = command.ExecuteReader();

                    var columns = db.SqlColumns.Where(f => f.object_id == rp.sqlTables.object_id).ToList();

                    int falgfrist = 0;
                    int falgfristM = 0;


                    models.AppendLine("  [ ");
                    while (reader.Read())
                    {

                        if (falgfristM == 0)
                        {
                            models.AppendLine("{");

                        }
                        else
                        {
                            models.AppendLine(",{");
                        }

                        falgfrist = 0;
                        foreach (SqlColumns column in columns)
                        {

                            if (falgfrist == 0)
                            {
                                models.AppendLine("\"" + column.name + "\"" + " : " + "\"" + reader["" + column.name + ""] + "\"");
                            }
                            else
                            {
                                models.AppendLine(",\"" + column.name + "\"" + " : " + "\"" + reader["" + column.name + ""] + "\"");
                            }

                            falgfrist = 1;

                        }

                        models.AppendLine("}");
                        falgfristM = 1;
                    }

                    models.AppendLine(" ] ");

                    connection.Close();
                    command.Dispose();
                    connection.Dispose();

                }


            }





            return Json(models.ToString(), JsonRequestBehavior.AllowGet);


        }


        private void AssignAndSave(List<object> Model, SqlTables table)
        {

            foreach (Object obj in Model)
            {

                returnQueryInsert(obj, table); 
                 
            }
  
        }

 
        private string returnQueryInsert( object obj , SqlTables table)
        {
            StringBuilder models = new StringBuilder();
            //models.AppendLine("constructor()")
            //    .AppendLine("{");
            int falgfrist = 0;

            var columns = db.SqlColumns.Where(f => f.object_id == table.object_id).ToList();
            foreach (SqlColumns column in columns)
            {

                //var value = obj.GetType().GetProperty(propertyName).GetValue(obj, null);
                string propertyName = column.name;
                var type = obj.GetType();
                string value;

                //obj = GetPropertyValue(obj, column.name); 
                //var makeValue = (string)obj.GetPropertyValue("Make"); 
                var json = JObject.Parse(obj.ToString());
               var transactionsWithAccountBalance = json[""].AsJEnumerable()
                                                        .Where(t => t[column.name] != null) 
                                                        .ToList();

                foreach (var item in json["json"])
                {
                    if (item[column.name] != null)
                    {
                        value = item[column.name].Value<string>();
                    }
                }


                Type myType = obj.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(obj, null);

                    // Do something with propValue
                }

                //var y = obj.ToPropertyType()
                var x = obj.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
                
                value = (string)type.GetProperty(propertyName).GetValue(obj, null);

                models.AppendLine(value.ToString());

            }
            //models.AppendLine("}");

            return models.ToString();

        }
        public string GetPropertyValue(string propertyName)
        {
            try
            {
                return this.GetType().GetProperty(propertyName).GetValue(this, null) as string;
            }
            catch { return null; }
        }
        private string GenerateConstructorCast(SqlTables table)
        {
            StringBuilder models = new StringBuilder();
            //models.AppendLine("constructor()")
            //    .AppendLine("{");
            int falgfrist = 0;

            var columns = db.SqlColumns.Where(f => f.object_id == table.object_id).ToList();
            foreach (SqlColumns column in columns)
            {
                string typeName = column.ColumnType(this.db);
                string value = string.Empty;
                switch (typeName)
                {
                    case "string":
                        value = " : \"\" ";
                        break;
                    case "number":
                        value = " : 0";
                        break;
                    case "boolean":
                        value = " : false";
                        break;
                    default:
                        break;
                }

                if (falgfrist == 0)
                {
                    models.AppendLine("\"" + column.name + "\"" + value); 
                }
                else
                {
                    models.AppendLine(",\"" + column.name + "\"" + value);
                }

                falgfrist = 1;
            }
            //models.AppendLine("}");

            return models.ToString();

        }

 


        private string GenerateConstructor(SqlTables table)
        {
            StringBuilder models = new StringBuilder();
            models.AppendLine("constructor()")
                .AppendLine("{");
            var columns = db.SqlColumns.Where(f => f.object_id == table.object_id).ToList();
            foreach (SqlColumns column in columns)
            {
                string typeName = column.ColumnType(this.db);
                string value = string.Empty;
                switch (typeName)
                {
                    case "string":
                        value = " = \"\";";
                        break;
                    case "number":
                        value = " = 0;";
                        break;
                    case "boolean":
                        value = " = false;";
                        break;
                    default:
                        break;
                }

                models.AppendLine("this." + column.name + value);
            }
            models.AppendLine("}");

            return models.ToString();

        }


    }
}