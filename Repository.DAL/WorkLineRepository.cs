using Common;
using Repository.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL
{
    public class WorkLineRepository:BaseRepository<WorkLine>
    {
        public List<WorkLineHelper> GetTodayWorkLine()
        {
            var previousDay = DateTime.Now.Date.AddDays(-1);
            var prevDay = new DateTime(previousDay.Year, previousDay.Month, previousDay.Day,23, 59, 59);

            var endOfDay = DateTime.Now;
            var endofDayTillMidnight = new DateTime(endOfDay.Year, endOfDay.Month, endOfDay.Day, 23, 59, 59);

            var result = from wl in MTOContext.WorkLines
                         where wl.InsertDateTime> prevDay && wl.InsertDateTime< endofDayTillMidnight
                         join p in MTOContext.Products on wl.ProductId equals p.Id
                         join c in MTOContext.Categories on p.ProductCategoryId equals c.Id
                         join pro in MTOContext.Processes on wl.ProcessId equals pro.Id
                         join u in MTOContext.Users on wl.OperatorId equals u.Id
                         orderby wl.InsertDateTime descending
                         select new WorkLineHelper
                         {
                             Id = wl.Id,
                             InsertDateTime = wl.InsertDateTime,
                             OperatorId = wl.OperatorId,
                             OperatorName = u.FriendlyName,
                             ProcessId = wl.ProcessId,
                             ProcessName = pro.Name,
                             ProductCode = p.Code,
                             ProductId = wl.ProductId,
                             ProductName = c.Name + " " + p.Name,
                             WorksheetId=wl.WorksheetId
                         };

            var res = result.ToList();

            foreach (WorkLineHelper item in res)
            {
                var dt = Common.Utility.CastToFaDateTime(item.InsertDateTime);
                dt = dt.Substring(11, dt.Length - 11);
                item.PersianInsertDateTime = dt;
            }

            return res;
        }

        public List<WorkLineHelper> GetAllForReport(System.Linq.Expressions.Expression<Func<WorkLineHelper, bool>> whereClause)
        {
            var result = from wl in MTOContext.WorkLines
                         join p in MTOContext.Products on wl.ProductId equals p.Id
                         join c in MTOContext.Categories on p.ProductCategoryId equals c.Id
                         join pro in MTOContext.Processes on wl.ProcessId equals pro.Id
                         join u in MTOContext.Users on wl.OperatorId equals u.Id
                         orderby wl.InsertDateTime descending
                         
                         select new WorkLineHelper
                         {
                             Id = wl.Id,
                             InsertDateTime = wl.InsertDateTime,
                             OperatorId = wl.OperatorId,
                             OperatorName = u.FriendlyName,
                             ProcessId = wl.ProcessId,
                             ProcessName = pro.Name,
                             ProductCode = p.Code,
                             ProductId = wl.ProductId,
                             ProductName = c.Name + " " + p.Name,
                             WorksheetId = wl.WorksheetId
                         };

            var finalRes = new List<WorkLineHelper>();

            if (whereClause != null)
            {
                finalRes = result.Where(whereClause).ToList();
            }

            foreach (WorkLineHelper item in finalRes)
            {
                var dt = Common.Utility.CastToFaDateTime(item.InsertDateTime);
                item.PersianInsertDateTime = dt;
            }

            return finalRes;
        }

        public List<WorkLineSummary> GetAllForSummaryReport(System.Linq.Expressions.Expression<Func<WorkLineHelper, bool>> whereClause)
        {
            var select = from wl in MTOContext.WorkLines
                             join u in MTOContext.Users on wl.OperatorId equals u.Id

                             select new WorkLineHelper
                             {
                                 Id = wl.Id,
                                 InsertDateTime = wl.InsertDateTime,
                                 OperatorId = wl.OperatorId,
                                 OperatorName = u.FriendlyName,
                                 ProcessId = wl.ProcessId,
                                 ProductId = wl.ProductId,
                                 WorksheetId = wl.WorksheetId,

                             };

            var selectList = new List<WorkLineHelper>();

            if (whereClause != null)
            {
                selectList = select.Where(whereClause).ToList();
            }

            foreach (WorkLineHelper item in selectList)
            {
                var faDate = Common.Utility.CastToFaDate(item.InsertDateTime);
                item.PersianInsertDateTime = faDate;
                item.Year = faDate.Substring(0, 4).ToSafeInt();
                item.Month = faDate.Substring(5, 2).ToSafeInt();
                item.Day = faDate.Substring(8, 2).ToSafeInt();
            }

            var result = from x in selectList
                         group x by new { x.OperatorId,x.OperatorName,x.PersianInsertDateTime,x.Year,x.Month, x.Day } into g
                         orderby g.Key.OperatorId
                         orderby g.Key.Year
                         orderby g.Key.Month
                         orderby g.Key.Day

                         select new WorkLineSummary
                         {
                             OperatorId= g.Key.OperatorId,
                             FriendlyName = g.Key.OperatorName,
                             PersianDate=g.Key.PersianInsertDateTime,
                             Year = g.Key.Year,
                             Month = g.Key.Month,
                             Day = g.Key.Day,
                             Count = g.Count()
                         };

            return result.ToList();
        }

    }

    public class WorkLineHelper : WorkLine
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProcessName { get; set; }
        public string OperatorName { get; set; }
        public string PersianInsertDateTime { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }

    public class WorkLineSummary
    {
        public int OperatorId { get; set; }
        public string FriendlyName { get; set; }
        public int Count { get; set; }

        public string PersianDate { get; set; }

        public DateTime InsertDateTime { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
