using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;
using System.IO;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data.Common;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        public string EmployeeInfo_Old()
        {
            string file = Server.MapPath("~/App_Data/Employee.json");
            return File.ReadAllText(file);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat=ResponseFormat.Json, UseHttpGet = true)]
        public string EmployeeInfo()
        {
            List<Employee1> emplist = GetEmployeeList();
            string response = JsonConvert.SerializeObject(emplist, Formatting.None);
            return response;
        }


        private List<Employee1> GetEmployeeList()
        {

            List<Employee1> empList = new List<Employee1>();

            empList.Add(new Employee1
            {
                employeeId = 6021,
                firstName = "John",
                lastName = "Doe",
                state = "Arizona",
                city = "Phoenix"
            });

            empList.Add(new Employee1
            {
                employeeId = 6022,
                firstName = "Anna",
                lastName = "Smith",
                state = "California",
                city = "Sacramento"
            });

            empList.Add(new Employee1
            {
                employeeId = 6023,
                firstName = "Peter",
                lastName = "Jones",
                state = "Colorado",
                city = "Denver"
            });

            return empList;

        }

        private class Employee1
        {
            public int employeeId { get; set; }

            public string firstName { get; set; }

            public string lastName { get; set; }

            public string state { get; set; }

            public string city { get; set; }

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
