
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Inv.WebUI.Models
{
    public class SqlEntities
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {
                return "Data source = " + Server + " ; Initial catalog = " + Database + " ; User id = " + User + "; Password = " + Password + ";";
            }
        }
        private DataTable GetData(string sqlStatment)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    try
                    {
                        connection.Open();
                        command.CommandText = sqlStatment;

                        tbl.Load(command.ExecuteReader());
                        connection.Close();
                        command.Dispose();
                        connection.Dispose();
                        return tbl;
                    }
                    catch (Exception ex)
                    {
                        return tbl;
                        throw new Exception(ex.Message);
                    }

                }


            }
        }

        private SqlDataReader GetDataNew(string sqlStatment)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    connection.Open();
                    command.CommandText = sqlStatment;

                    SqlDataReader reader = command.ExecuteReader();

                     
                    //connection.Close();
                    command.Dispose();
                    connection.Dispose();
                    return reader;

                }


            }
        }


        private List<SqlTables> _sqlTables;
        public List<SqlTables> SqlTables
        {
            get
            {
                if (_sqlTables == null)
                {
                    _sqlTables = (List<SqlTables>)GetData("(Select sys.tables.object_id , sys.tables.name From sys.tables union select sys.views.object_id,sys.views.name from sys.views )order by name").ToList<SqlTables>();
                }

                return _sqlTables;
            }
        }

        private List<SqlColumns> _sqlColumns;
        public List<SqlColumns> SqlColumns
        {
            get
            {
                if (_sqlColumns == null)
                {
                    _sqlColumns = (List<SqlColumns>)GetData("select * from sys.columns").ToList<SqlColumns>();
                }

                return _sqlColumns;
            }
        }

        public SqlDataReader GetDataTable(string NameTable)
        {
             

            SqlDataReader dt = GetDataNew("select * from " + NameTable + "");


            return dt;

        }



        private List<SqlTypes> _sqlTypes;
        public List<SqlTypes> SqlTypes
        {
            get
            {
                if (_sqlTypes == null)
                {
                    _sqlTypes = (List<SqlTypes>)GetData("select * from sys.types").ToList<SqlTypes>();
                }

                return _sqlTypes;
            }
        }
    }
}
