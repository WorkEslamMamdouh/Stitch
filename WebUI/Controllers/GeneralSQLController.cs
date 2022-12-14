
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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NPOI.SS.Formula.Functions;
using System.Web.Script.Serialization;

namespace Inv.WebUI.Controllers
{//eslam 1 dec 2020
    public class GeneralSQLController : Controller
    {

        public class ModelSql
        {
            public SqlTables sqlTables = new SqlTables();
            public SqlEnt sqlEnt = new SqlEnt();
            public object Model = new object();
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

            string jsonString = rp.Model.ToString();

            var json = JObject.Parse(jsonString);
            List<object> NewModel = json["Model"].AsJEnumerable()
                                                                    .Select(t => t.ToObject<object>())
                                                                    .ToList();

            StringBuilder models = new StringBuilder();



            using (SqlConnection connection = new SqlConnection("Data source = " + db.Server + " ; Initial catalog = " + db.Database + " ; User id = " + db.User + "; Password = " + db.Password + ";"))
            {

                using (SqlCommand command = new SqlCommand())
                {

                    //--------------------------------------------------InsetData---------------------------------------




                    AssignAndSave(NewModel, table);


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
            //List<object> Modell = Model;


            foreach (Object obj in Model)
            {
                using (SqlConnection connection = new SqlConnection("Data source = " + db.Server + " ; Initial catalog = " + db.Database + " ; User id = " + db.User + "; Password = " + db.Password + ";"))
                {

                    using (SqlCommand command = new SqlCommand())
                    {

                        command.Connection = connection;

                        connection.Open();

                        string ID = "";
                        command.CommandText = @"select C.COLUMN_NAME FROM  
                                                INFORMATION_SCHEMA.TABLE_CONSTRAINTS T  
                                                JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE C  
                                                ON C.CONSTRAINT_NAME=T.CONSTRAINT_NAME  
                                                WHERE  
                                                C.TABLE_NAME='" + table.name + "'  and T.CONSTRAINT_TYPE='PRIMARY KEY'  ";

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            ID = reader["COLUMN_NAME"].ToString();
                        }
                        connection.Close();

                        string qury = returnQueryInsert(obj, table, ID);



                        connection.Open();
                        command.CommandText = qury;
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                        connection.Dispose();

                    }
                }
            }


        }


        private string returnQueryInsert(object obj, SqlTables table, string ID)
        {
            int flagfrist = 0;
            StringBuilder models = new StringBuilder();
            StringBuilder where = new StringBuilder();
            var json_serializer = new JavaScriptSerializer();
            var routes_list = (IDictionary<string, object>)json_serializer.DeserializeObject(obj.ToString());

            var columns = db.SqlColumns.Where(f => f.object_id == table.object_id).ToList();

            string StatusFlag = routes_list["StatusFlag"].ToString();
            string TableID = routes_list[ID].ToString();

            if (StatusFlag == "i") //insert
            {
                models.AppendLine("insert into " + table.name + " values( ");
                foreach (SqlColumns column in columns)
                {

                    string propertyName = column.name;
                    var type = obj.GetType();
                    string value = "";


                    value = routes_list[column.name].ToString();


                    if (flagfrist == 0)
                    {
                        models.Append("'" + value.ToString() + "'");

                    }
                    else
                    {
                        models.Append(",'" + value.ToString() + "'");
                    }


                    flagfrist = 1;
                }
                models.Append(" ) ");

            }
            if (StatusFlag == "u")//update
            {

                models.AppendLine("update " + table.name + " set ");
                //where.AppendLine("where " + TableID + " = ");
                foreach (SqlColumns column in columns)
                {

                    string propertyName = column.name;
                    var type = obj.GetType();
                    string value = "";


                    value = routes_list[column.name].ToString();


                    if (column.name != ID)
                    {

                        if (flagfrist == 0)
                        {
                            models.Append("" + column.name + " = '" + value.ToString() + "'");

                        }
                        else
                        {
                            models.Append("," + column.name + " = '" + value.ToString() + "'");
                        }
                        flagfrist = 1;
                    }
                    else
                    {
                        where.Append(" where " + column.name + " = '" + value.ToString() + "'");
                    }


                }
                models.Append(where);

            }
            if (StatusFlag == "d")
            {
                models.AppendLine("delete " + table.name + "  ");
                //where.AppendLine("where " + TableID + " = ");
                foreach (SqlColumns column in columns)
                {

                    string propertyName = column.name;
                    var type = obj.GetType();
                    string value = "";


                    value = routes_list[column.name].ToString();


                    if (column.name == ID)
                    {
                        where.Append(" where " + column.name + " = '" + value.ToString() + "'");

                    }
                     


                }
                models.Append(where);

            }






            return models.ToString();

        }



        public object ToClassString(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
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