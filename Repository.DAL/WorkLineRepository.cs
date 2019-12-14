﻿using Common;
using Repository.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL
{
    public class WorkLineRepository : BaseRepository<WorkLine>
    {
        public List<WorkLineHelper> GetTodayWorkLine()
        {
            var previousDay = DateTime.Now.Date.AddDays(-1);
            var prevDay = new DateTime(previousDay.Year, previousDay.Month, previousDay.Day, 23, 59, 59);

            var endOfDay = DateTime.Now;
            var endofDayTillMidnight = new DateTime(endOfDay.Year, endOfDay.Month, endOfDay.Day, 23, 59, 59);

            var result = from wl in MTOContext.WorkLines
                         where wl.InsertDateTime > prevDay && wl.InsertDateTime < endofDayTillMidnight
                         //join p in MTOContext.Products on wl.ProductId equals p.Id
                         //join c in MTOContext.Categories on p.ProductCategoryId equals c.Id
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
                             //ProductCode = p.Code,
                             //ProductId = wl.ProductId,
                             //ProductName = c.Name + " " + p.Name,
                             WorksheetId = wl.WorksheetId
                         };

            var res = result.ToList();

            foreach (WorkLineHelper item in res)
            {
                var dt = Common.Utility.CastToFaDateTime(item.InsertDateTime);
                dt = dt.Substring(11, dt.Length - 11);
                item.PersianDateTime = dt;
            }

            return res;
        }

        public List<WorkLineHelper> GetAllForReport(System.Linq.Expressions.Expression<Func<WorkLineHelper, bool>> whereClause)
        {
            var result = from wl in MTOContext.WorkLines
                             //join p in MTOContext.Products on wl.ProductId equals p.Id
                             //join c in MTOContext.Categories on p.ProductCategoryId equals c.Id
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
                             //ProductCode = p.Code,
                             //ProductId = wl.ProductId,
                             //ProductName = c.Name + " " + p.Name,
                             WorksheetId = wl.WorksheetId
                         };

            var finalRes = new List<WorkLineHelper>();

            if (whereClause != null)
            {
                finalRes = result.Where(whereClause).Distinct().ToList();
            }
            else
                finalRes = result.Distinct().ToList();

            foreach (WorkLineHelper item in finalRes)
            {
                var dt = Common.Utility.CastToFaDateTime(item.InsertDateTime);
                item.PersianDateTime = dt;
            }

            return finalRes;
        }

        public List<WorkLineSummary> GetAllForSummaryReport(int reportType, System.Linq.Expressions.Expression<Func<WorkLineHelper, bool>> whereClause)
        {

            var workLinesSelect = from wl in MTOContext.WorkLines
                                  join u in MTOContext.Users on wl.OperatorId equals u.Id

                                  select new WorkLineHelper
                                  {
                                      Id = wl.Id,
                                      InsertDateTime = wl.InsertDateTime,
                                      OperatorId = wl.OperatorId,
                                      OperatorName = u.FriendlyName,
                                      ProcessId = wl.ProcessId,
                                      WorksheetId = wl.WorksheetId,
                                  };

            var workLinesSelectList = new List<WorkLineHelper>();

            if (whereClause != null)
            {
                workLinesSelectList = workLinesSelect.Where(whereClause).ToList();
            }
            else
                workLinesSelectList = workLinesSelect.ToList();

            foreach (WorkLineHelper item in workLinesSelectList)
            {
                var faDate = Common.Utility.CastToFaDate(item.InsertDateTime);
                item.PersianDate = faDate;
                item.PersianDateTime = Common.Utility.CastToFaDateTime(item.InsertDateTime); ;
                item.Year = faDate.Substring(0, 4).ToSafeInt();
                item.Month = faDate.Substring(5, 2).ToSafeInt();
                item.Day = faDate.Substring(8, 2).ToSafeInt();
                item.Hour = item.InsertDateTime.Value.Hour;
                item.Min = item.InsertDateTime.Value.Minute;
                item.Sec = item.InsertDateTime.Value.Second;
            }

            IEnumerable<WorkLineSummary> result = null;

            if (reportType == 1)
            {
                result = from x in workLinesSelectList
                         group x by new { x.OperatorId, x.OperatorName, x.PersianDate, x.Year, x.Month, x.Day } into g
                         orderby g.Key.OperatorId, g.Key.Year, g.Key.Month, g.Key.Day

                         select new WorkLineSummary
                         {
                             OperatorId = g.Key.OperatorId,
                             FriendlyName = g.Key.OperatorName,
                             PersianDate = g.Key.PersianDate,
                             Year = g.Key.Year,
                             Month = g.Key.Month,
                             Day = g.Key.Day,
                             Count = g.Count()
                         };
            }
            else if (reportType == 2)
            {
                var worksheetsDetails = from ws in MTOContext.Worksheets
                                        join wd in MTOContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in MTOContext.Products on wd.ProductId equals p.Id
                                        join cat in MTOContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in MTOContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in MTOContext.Processes on pcat.ProcessId equals pro.Id
                                        select new WorkLineHelper
                                        {
                                            WorksheetId = ws.Id,
                                            OperatorId = (int)ws.OperatorId,
                                            ProductId = wd.ProductId,
                                            ProcessId = pro.Id,
                                            ProductCode = p.Code,
                                            ProductName = cat.Name + " " + p.Name,
                                            ProcessName = pro.Name
                                        };

                var worksheetsDetailsList = new List<WorkLineHelper>();

                //if (whereClause != null)
                //{
                //    worksheetsDetailsList = worksheetsDetails.Where(whereClause).ToList();
                //}
                //else
                worksheetsDetailsList = worksheetsDetails.ToList();

                result = (from x in worksheetsDetailsList
                          join wl in workLinesSelectList
                          on new { x.WorksheetId, x.ProcessId } equals new { wl.WorksheetId, wl.ProcessId }
                          select new WorkLineSummary
                          {
                              InsertDateTime = (DateTime)wl.InsertDateTime,
                              OperatorId = x.OperatorId,
                              FriendlyName = wl.OperatorName,
                              PersianDate = wl.PersianDateTime,
                              ProductCode = x.ProductCode,
                              ProductName = x.ProductName,
                              ProcessName = x.ProcessName,
                          }).ToList();

                foreach (WorkLineSummary item in result)
                {
                    var faDate = Utility.CastToFaDate(item.InsertDateTime);
                    item.PersianDate = Utility.CastToFaDateTime(item.InsertDateTime); //faDate;
                    //item.PersianDateTime = Utility.CastToFaDateTime(item.InsertDateTime); 
                    item.Year = faDate.Substring(0, 4).ToSafeInt();
                    item.Month = faDate.Substring(5, 2).ToSafeInt();
                    item.Day = faDate.Substring(8, 2).ToSafeInt();
                    //item.Hour = item.InsertDateTime.Value.Hour;
                    //item.Min = item.InsertDateTime.Value.Minute;
                    //item.Sec = item.InsertDateTime.Value.Second;
                }
            }
            else if (reportType == 3)
            {
                var worksheetsDetails = from ws in MTOContext.Worksheets
                                        join wd in MTOContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in MTOContext.Products on wd.ProductId equals p.Id
                                        join cat in MTOContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in MTOContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in MTOContext.Processes on pcat.ProcessId equals pro.Id
                                        select new WorkLineHelper
                                        {
                                            WorksheetId = ws.Id,
                                            OperatorId = (int)ws.OperatorId,
                                            ProductId = wd.ProductId,
                                            ProcessId = pro.Id,
                                            ProductCode = p.Code,
                                            ProductName = cat.Name + " " + p.Name,
                                            ProcessName = pro.Name,
                                            ProcessTime = pcat.ProcessTime
                                        };

                var worksheetsDetailsList = new List<WorkLineHelper>();

                worksheetsDetailsList = worksheetsDetails.ToList();

                result = (from x in worksheetsDetailsList
                          join wl in workLinesSelectList
                          on new { x.WorksheetId, x.ProcessId } equals new { wl.WorksheetId, wl.ProcessId }
                          select new WorkLineSummary
                          {
                              InsertDateTime = (DateTime)wl.InsertDateTime,
                              OperatorId = x.OperatorId,
                              FriendlyName = wl.OperatorName,
                              PersianDate = wl.PersianDateTime,
                              ProductCode = x.ProductCode,
                              ProductName = x.ProductName,
                              ProcessName = x.ProcessName,
                              ProcessTime = x.ProcessTime
                          }).ToList();

                //calcing allowed time
                var allowedTimeResult = from r in result
                         group r by new { r.OperatorId, r.FriendlyName } into g
                         select new WorkLineSummary
                         {
                             OperatorId = g.Key.OperatorId,
                             FriendlyName = g.Key.FriendlyName,
                             ProcessTime = g.Sum(a => a.ProcessTime)
                         };


                //calcing spent time
                var groupedByWIDProcess = from x in result
                                          group x by new { x.ProcessId, x.ProcessName, x.OperatorId, x.FriendlyName, x.InsertDateTime } into g
                                          select new WorkLineSummary
                                          {
                                              OperatorId = g.Key.OperatorId,
                                              FriendlyName = g.Key.FriendlyName,
                                              ProcessId = g.Key.ProcessId,
                                              ProcessName = g.Key.ProcessName,
                                              InsertDateTime = g.Key.InsertDateTime
                                          };

                var groupedByWIDProcessList = groupedByWIDProcess.ToList();
                groupedByWIDProcessList = groupedByWIDProcessList.OrderBy(a=>a.OperatorId).ThenBy(a=>a.InsertDateTime).ToList();

                WorkLineSummary item, prevItem, nextItem;
                for (int i = 0; i < groupedByWIDProcessList.Count; i++)
                {
                    item = groupedByWIDProcessList[i];

                    if (i != 0)
                        prevItem = groupedByWIDProcessList[i - 1];
                    else
                        prevItem = null;

                    if (i != groupedByWIDProcessList.Count - 1)
                        nextItem = groupedByWIDProcessList[i + 1];
                    else
                        nextItem = null;
                                       
                    var faDate = Utility.CastToFaDate(item.InsertDateTime);
                    item.PersianDate = Utility.CastToFaDateTime(item.InsertDateTime);
                    item.Year = faDate.Substring(0, 4).ToSafeInt();
                    item.Month = faDate.Substring(5, 2).ToSafeInt();
                    item.Day = faDate.Substring(8, 2).ToSafeInt();

                    if (prevItem?.OperatorId == item.OperatorId && item.Year == prevItem.Year && item.Month==prevItem.Month && item.Day==prevItem.Day)
                        prevItem.ProcessDuration = Math.Truncate((item.InsertDateTime - prevItem.InsertDateTime).TotalSeconds).ToSafeInt();

                }
               

                result = groupedByWIDProcessList;

            }

            return result.ToList();
        }
    }

    public class WorkLineHelper : WorkLine
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProcessName { get; set; }
        public string OperatorName { get; set; }
        public string PersianDate { get; set; }
        public string PersianDateTime { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public int Sec { get; set; }
        public int ProcessTime { get; set; }
    }

    public class WorkLineSummary
    {
        public int WorksheetId { get; set; }
        public int OperatorId { get; set; }
        public string FriendlyName { get; set; }
        public int Count { get; set; }
        public string PersianDate { get; set; }
        public string PersianDateTime { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProcessName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int ProductId { get; set; }
        public int ProcessId { get; set; }
        public int ProcessTime { get; set; }
        public int ProcessDuration { get; set; }
    }
}
