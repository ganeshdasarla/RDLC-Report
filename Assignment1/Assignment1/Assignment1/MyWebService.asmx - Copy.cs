using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data.Common;

namespace Assignment1
{
    [ScriptService]
    public class MyWebService : System.Web.Services.WebService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionName"].ToString();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string EmployeeInfo()
        {
            string file = Server.MapPath("~/App_Data/Employee.json");
            return File.ReadAllText(file);
        }


        [WebMethod]
        public void EmployeeData()
        {

            SqlDatabase sqldb = new SqlDatabase(connectionString);

            //string sql = "Select * from employee";
            //var result = sqldb.ExecuteSqlStringAccessor<Employee>(sql);

            IParameterMapper paramMapper = new ExampleParameterMapper();
            IRowMapper<Employee> rowMapper = MapBuilder<Employee>.MapAllProperties()
                                            .MapByName(x => x.EmployeedId)
                                            .DoNotMap(x => x.Age)
                                            .Build();

            var resultsp = sqldb.ExecuteSprocAccessor<Employee>("GetEmployeeInfo", paramMapper, rowMapper, 123, 34);
        }

        [WebMethod]
        public string EmployeedInfoUsingStringAccessor()
        {
            try
            {

                //DataTable dt = new DataTable();
                //string connString = connectionString;
                //string query = "select * from employee";

                //SqlConnection conn = new SqlConnection(connString);
                //SqlCommand cmd = new SqlCommand(query, conn);
                //conn.Open();

                //// create data adapter
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //// this will query your database and return the result to your datatable
                //da.Fill(dt);
                //conn.Close();
                //da.Dispose();

                SqlDatabase db = new SqlDatabase(connectionString );

                //var rr = dbase.ExecuteSqlStringAccessor<Employee>(query);

                //// Create and execute a SQL string accessor that uses a custom output mapper 
                //IRowMapper<Employee> rowMapper = MapBuilder<Employee>.MapAllProperties()
                //                                .MapByName(x => x.FName)
                //                                .DoNotMap(x => x.LName)
                //                                .Build();
                //var results = dbase.ExecuteSqlStringAccessor<Employee>(query, rowMapper);


                //var tt = dbase.ExecuteSprocAccessor<Employee>("GetEmployeeInfo");



                // Create and execute a sproc accessor that uses default parameter and output mappings
                var results = db.ExecuteSprocAccessor<Employee>("GetEmployeeInfo", 1);

                //DbCommand cmd = d
                //// Use a custom parameter mapper and the default output mappings
                //IParameterMapper paramMapper = new ExampleParameterMapper();
                //var resultstt = db.ExecuteSprocAccessor<Employee>("GetEmployeeInfo", paramMapper, yourCustomParamsArray);

                // Use the default parameter mappings and a custom output mapper
                //IRowMapper<Employee> rowMapper = MapBuilder<Employee>.MapAllProperties()
                //                                .MapByName(x => x.FName)
                //                                .DoNotMap(x => x.LName)
                //                                .Build();
                //var results11 = db.ExecuteSprocAccessor<Employee>("[GetEmployeeInfo]", rowMapper, 1);

                // Use a custom parameter mapper and a custom output mapper
                //var results1 = db.ExecuteSprocAccessor<Customer>("Customer List", paramMapper, rowMapper, yourCustomParamsArray);

            }
            catch (Exception ex) { }

            return null;
        }
    }
    public class ExampleParameterMapper : IParameterMapper    
    {
        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@empId";
            parameter.Value = parameterValues[0];
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "@age";
            parameter.Value = parameterValues[1];
            command.Parameters.Add(parameter);
        }

        //void IParameterMapper.AssignParameters(DbCommand command, object[] parameterValues)
        //{
        //    DbParameter parameter = command.CreateParameter();
        //    parameter.ParameterName = "@p1";
        //    parameter.Value = parameterValues[0];
        //    command.Parameters.Add(parameter);
        //}
    }

    internal class Employee
    {
        public object EmployeedId { get; internal set; }
        public object Name { get; internal set; }

        public int Age { get; set; }

        public string City { get; set; }

    }
}
