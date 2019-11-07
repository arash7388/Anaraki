using Repository.Entity.Domain;
using System;
using System.Collections.Generic;
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
                         join pro in MTOContext.Processes on wl.ProcessId equals pro.Id
                         join u in MTOContext.Users on wl.OperatorId equals u.Id
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
                             ProductName = p.Name,
                             PersianInsertDateTime="1398/01/01"
                         };

            var res = result.ToList();

            foreach (WorkLineHelper item in res)
            {
                item.PersianInsertDateTime = Common.Utility.CastToFaDateTime(item.InsertDateTime);
            }

            return res;
        }
    }

    public class WorkLineHelper : WorkLine
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProcessName { get; set; }
        public string OperatorName { get; set; }

        public string PersianInsertDateTime { get; set; }
    }
}
