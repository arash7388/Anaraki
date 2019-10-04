using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.FriendlyUrls;
using System.Web.Http;
using Common;

namespace MehranPack
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapPageRoute("Login", "Login", "~/Login.aspx");
            RouteTable.Routes.MapPageRoute("Home", "Home", "~/Home.aspx");
            RouteTable.Routes.MapPageRoute("Confirmation", "Confirmation", "~/Confirmation.aspx");
            RouteTable.Routes.MapPageRoute("CustomerList", "CustomerList", "~/CustomerList.aspx");
            RouteTable.Routes.MapPageRoute("InputOutputList", "InputOutputList/{Type}", "~/InputOutputList.aspx");
            RouteTable.Routes.MapPageRoute("InputOutputListActionResult", "InputOutputList/{Type}/{ActionResult}", "~/InputOutputList.aspx");
            RouteTable.Routes.MapPageRoute("OrderList", "OrderList", "~/OrderList.aspx");
            RouteTable.Routes.MapPageRoute("FactorList", "FactorList", "~/FactorList.aspx");
            RouteTable.Routes.MapPageRoute("PaymentList", "PaymentList", "~/PaymentList.aspx");
            RouteTable.Routes.MapPageRoute("FactorListActionResult", "FactorList/{ActionResult}", "~/FactorList.aspx");
            RouteTable.Routes.MapPageRoute("PaymentListActionResult", "PaymentList/{ActionResult}", "~/PaymentList.aspx");
            RouteTable.Routes.MapPageRoute("Customer", "Customer/{Id}", "~/Customer.aspx");
            RouteTable.Routes.MapPageRoute("InputOutput", "InputOutput/{Type}/{Id}", "~/InputOutput.aspx");
            RouteTable.Routes.MapPageRoute("IOReport", "IOReport/{Type}", "~/IOReport.aspx");
            RouteTable.Routes.MapPageRoute("Cardex", "Cardex", "~/Cardex.aspx");
            RouteTable.Routes.MapPageRoute("GoodsSupply", "GoodsSupply", "~/GoodsSupply.aspx");
            RouteTable.Routes.MapPageRoute("GoodsGroupSupply", "GoodsGroupSupply", "~/GoodsGroupSupply.aspx");
            RouteTable.Routes.MapPageRoute("Factor", "Factor/{Id}", "~/Factor.aspx");
            RouteTable.Routes.MapPageRoute("FactorsReport", "FactorsReport", "~/FactorsReport.aspx");
            RouteTable.Routes.MapPageRoute("FactorPrint", "FactorPrint/{Id}", "~/FactorPrint.aspx");
            RouteTable.Routes.MapPageRoute("Order", "Order/{Id}", "~/Order.aspx");
            RouteTable.Routes.MapPageRoute("OrdersReport", "OrdersReport", "~/OrdersReport.aspx");
            RouteTable.Routes.MapPageRoute("OrderPrint", "OrderPrint/{Id}", "~/OrderPrint.aspx");
            RouteTable.Routes.MapPageRoute("Payment", "Payment/{Id}", "~/Payment.aspx");
            RouteTable.Routes.MapPageRoute("PaymentsReport", "PaymentsReport", "~/PaymentsReport.aspx");
            RouteTable.Routes.MapPageRoute("ProductTypeList", "ProductTypeList", "~/ProductTypeList.aspx");
            RouteTable.Routes.MapPageRoute("ProductType", "ProductType/{Id}", "~/ProductTypeList.aspx");
            RouteTable.Routes.MapPageRoute("User", "User/{Id}", "~/Users.aspx");
            RouteTable.Routes.MapPageRoute("UsersList", "UsersList", "~/UsersList.aspx");

            RouteTable.Routes.MapPageRoute("Process", "Process/{Id}", "~/Process.aspx");
            RouteTable.Routes.MapPageRoute("ProcessList", "ProcessList", "~/ProcessList.aspx");

            RouteTable.Routes.MapPageRoute("ProcessCategory", "ProcessCategory/{Id}", "~/ProcessCategory.aspx");
            RouteTable.Routes.MapPageRoute("ProcessCategoryList", "ProcessCategoryList", "~/ProcessCategoryList.aspx");

            RouteTable.Routes.Ignore("*.html|js|css|gif|jpg|jpeg|png|swf");
            RouteTable.Routes.EnableFriendlyUrls();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            Debuging.Error(exc.Message);

           // For other kinds of errors give the user some information
            // but stay on the default page
            Response.Write("<h2>Global Page Error</h2>\n");
            Response.Write(
                "<p>" + exc.Message + "</p>\n");
            Response.Write("Return to the <a href='Default.aspx'>" +
                "Default Page</a>\n");

           // Clear the error from the server
            Server.ClearError();
     
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}