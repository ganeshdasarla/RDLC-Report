using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment1
{
    public partial class MyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/Report1.rdlc");
                SampleDBDataSet dsCustomers = GetData("select * from employee");
                ReportDataSource datasource = new ReportDataSource("DataSet1", dsCustomers.Tables[1]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);
            } 
        }

        private SampleDBDataSet GetData(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["SampleDBConnectionString"].ConnectionString;
            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (SampleDBDataSet dsCustomers = new SampleDBDataSet())
                    {
                        sda.Fill(dsCustomers, "DataTableEmp");
                        return dsCustomers;
                    }
                }
            }
        }

        //private DataSetEmp GetDataEmp(string query)
        //{
        //    string conString = ConfigurationManager.ConnectionStrings["ConnectionName"].ConnectionString;
        //    SqlCommand cmd = new SqlCommand(query);
        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            cmd.Connection = con;

        //            sda.SelectCommand = cmd;
        //            using (DataSetEmp dsCustomers = new DataSetEmp())
        //            {
        //                sda.Fill(dsCustomers, "DataTableEmp");
        //                return dsCustomers;
        //            }
        //        }
        //    }
        //}

        private void ReportGenerator()
        {

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;//enter code here`
        string extension = string.Empty;
           // DataSet dsGrpSum, dsActPlan, dsProfitDetails,
            //    dsProfitSum, dsSumHeader, dsDetailsHeader, dsBudCom = null;

            //enter code here

        //This is optional if you have parameter then you can add parameters as much as you want
        //ReportParameter[] param = new ReportParameter[5];
        //    param[0] = new ReportParameter("Report_Parameter_0", "1st Para", true);
        //    param[1] = new ReportParameter("Report_Parameter_1", "2nd Para", true);
        //    param[2] = new ReportParameter("Report_Parameter_2", "3rd Para", true);
        //    param[3] = new ReportParameter("Report_Parameter_3", "4th Para", true);
        //    param[4] = new ReportParameter("Report_Parameter_4", "5th Para");

           // DataSet dsData = "Fill this dataset with your data";
            //ReportDataSource rdsAct = new ReportDataSource("RptActDataSet_usp_GroupAccntDetails", dsActPlan.Tables[0]);

            SampleDBDataSet dsCustomers = GetData("select * from employee");
            ReportDataSource datasource = new ReportDataSource("DataSet1", dsCustomers.Tables[1]);

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.Refresh();
            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/Report1.rdlc");//"Reports/AcctPlan.rdlc"; //This is your rdlc name.
                                                                                    // viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Add(datasource); // Add  datasource here         
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            // byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.          
            // System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename= filename" + "." + extension);
            Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
            Response.Flush(); // send it to the client to download  
            Response.End();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            ReportGenerator();
        }
    }
}