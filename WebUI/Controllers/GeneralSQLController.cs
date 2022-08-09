﻿using Inv.DAL.Domain;
using Inv.DAL.Repository;
using Inv.WebUI.Reports.Forms;
using Inv.WebUI.Reports.Models;
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

namespace Inv.WebUI.Controllers
{//eslam 1 dec 2020
    public class GeneralSQLController : ReportsPagePrintController
    {

        public class ModelSql
        {
            public SqlTables sqlTables = new SqlTables();
            public SqlEnt sqlEnt = new SqlEnt();
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