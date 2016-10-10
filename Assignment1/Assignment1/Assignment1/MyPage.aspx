<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyPage.aspx.cs" Inherits="Assignment1.MyPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="scripts/jquery-1.12.3.js"></script>
    <script src="scripts/jquery.dataTables.min.js"></script>
</head>
<body>
    <script>
        $(document).ready(function () {
            var ajs = $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: 'http://localhost:59490/MyWebService.asmx/EmployeeInfo',
                success: ajaxSucceeded,
                error: ajaxFailed
            });
        });

        function ajaxSucceeded(result) {
            intializeTable(JSON.parse(result.d));
        }

        function ajaxFailed(err) {
            console.log(err);
        }

        function intializeTable(data) {
            $('#tblEmployee').DataTable({
                "filter": false,
                "paginate": false,
                "lengthChange": false,
                "info": false,
                "data": data,
                "columns": [
                    { "data": "employeeId" },
                    { "data": "firstName" },
                    { "data": "lastName" },
                    { "data": "state" },
                    { "data": "city" },
                ]
            });
        }

    </script>
    <div style="margin: 20px 10px 10px 10px;">
        <label style="font-weight: bold">Employee Data:</label>
    </div>
    <div style="border: 2px double; align-content: center; margin: 10px;">
        <div style="margin: 5px 5px 15px 5px;">
            <table id="tblEmployee" class="display table" cellspacing="0">
                <thead>
                    <tr>
                        <th style="text-align: left">Employee Id</th>
                        <th style="text-align: left">First Name</th>
                        <th style="text-align: left">Last Name</th>
                        <th style="text-align: left">State</th>
                        <th style="text-align: left">City</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <form runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"></rsweb:ReportViewer>
        </div>

        <div>
            <asp:Button runat="server" Text="DownloadReport" OnClick="btnDownload_Click" ID="btnDownload" />
        </div>

    </form>

</body>
</html>
