using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MehranPack
{
    /// <summary>
    /// Summary description for DataHandler
    /// </summary>
    public class DataHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var serializer = new JavaScriptSerializer();

            var details = new WorksheetDetailRepository().GetAllDetails(id).ToList();

            var json = serializer.Serialize(details);
            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}