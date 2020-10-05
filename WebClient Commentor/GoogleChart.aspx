<%@ Page Title="" Language="C#" MasterPageFile="~/Sites.Master" AutoEventWireup="true" CodeBehind="GoogleChart.aspx.cs" Inherits="WebClient_Commentor.GoogleChart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- Here we need to write some JS code to load google chart with data. --%>
    <script src="Scripts/jquery-3.4.1.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script>
        var chartData; //Global variable to hold chart data.
        google.load("visualization", "1", { packages: ["corechart"] });

        //Here we'll fill chartData

        $(document).ready(function () {
            $.ajax({
                url: "GoogleChart.aspx/GetChartData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    chartData = data.d;
                },
                error: function () {
                    alert("Error loading data. Please try again.");
                }
            }).done(function () {
                // After loading is complete
                drawChart();
            });
        });

        function drawChart() {
            var data = google.visualization.arrayToDataTable(chartData);

            var options = {
                title: "Temporary Car Count Name",
                pointSize: 5
            };

            var columnChart = new google.visualization.columnChart(document.getElementById('chart_div'));
            columnChart.draw(data, options);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="chart_div" style="width:500px;height:400px">
        <%-- Here the chart will load. --%>
    </div>
</asp:Content>
