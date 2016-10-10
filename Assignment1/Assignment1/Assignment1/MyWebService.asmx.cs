using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;
using System.IO;
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
    }

    internal class Employee
    {
        public object EmployeedId { get; internal set; }
        public object Name { get; internal set; }
        public int Age { get; set; }
        public string City { get; set; }
    }
}
