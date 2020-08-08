using Common;
using Repository.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var result = from wl in DBContext.WorkLines
                         where wl.InsertDateTime > prevDay && wl.InsertDateTime < endofDayTillMidnight
                         //join p in DBContext.Products on wl.ProductId equals p.Id
                         //join c in DBContext.Categories on p.ProductCategoryId equals c.Id
                         join pro in DBContext.Processes on wl.ProcessId equals pro.Id
                         join u in DBContext.Users on wl.OperatorId equals u.Id
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
            var result = from wl in DBContext.WorkLines
                         join pro in DBContext.Processes on wl.ProcessId equals pro.Id
                         join u in DBContext.Users on wl.OperatorId equals u.Id
                         orderby wl.InsertDateTime descending

                         select new WorkLineHelper
                         {
                             Id = wl.Id,
                             InsertDateTime = wl.InsertDateTime,
                             OperatorId = wl.OperatorId,
                             OperatorName = u.FriendlyName,
                             ProcessId = wl.ProcessId,
                             ProcessName = pro.Name,
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
            var workLinesSelect = from wl in DBContext.WorkLines
                                  join u in DBContext.Users on wl.OperatorId equals u.Id

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
                var worksheetsDetails = from ws in DBContext.Worksheets
                                        join wd in DBContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in DBContext.Products on wd.ProductId equals p.Id
                                        join cat in DBContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in DBContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in DBContext.Processes on pcat.ProcessId equals pro.Id
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
                    item.Year = faDate.Substring(0, 4).ToSafeInt();
                    item.Month = faDate.Substring(5, 2).ToSafeInt();
                    item.Day = faDate.Substring(8, 2).ToSafeInt();
                }
            }
            else if (reportType == 3)
            {
                var worksheetsDetails = from ws in DBContext.Worksheets
                                        join u in DBContext.Users on ws.OperatorId equals u.Id
                                        join wd in DBContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in DBContext.Products on wd.ProductId equals p.Id
                                        join cat in DBContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in DBContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in DBContext.Processes on pcat.ProcessId equals pro.Id
                                        select new WorkLineHelper
                                        {
                                            WorksheetId = ws.Id,
                                            OperatorId = (int)ws.OperatorId,
                                            OperatorName = u.FriendlyName,
                                            ProductId = wd.ProductId,
                                            ProcessId = pro.Id,
                                            ProductCode = p.Code,
                                            ProductName = cat.Name + " " + p.Name,
                                            ProcessName = pro.Name,
                                            ProcessTime = pcat.ProcessTime
                                        };

                var worksheetsDetailsList = worksheetsDetails.ToList();

                //calcing allowed time
                var allowedTimeResult = from r in worksheetsDetailsList
                                        group r by new { r.OperatorId, r.OperatorName } into g
                                        select new WorkLineSummary
                                        {
                                            OperatorId = g.Key.OperatorId,
                                            FriendlyName = g.Key.OperatorName,
                                            ProcessTime = g.Sum(a => a.ProcessTime)
                                        };

                result = (from x in worksheetsDetailsList
                          join wl in workLinesSelectList
                          on new { WID = x.WorksheetId, PID = x.ProcessId, OID = x.OperatorId } equals new { WID = wl.WorksheetId, PID = wl.ProcessId, OID = wl.OperatorId }
                          select new WorkLineSummary
                          {
                              WorksheetId = x.WorksheetId ?? -1,
                              InsertDateTime = (DateTime)wl.InsertDateTime,
                              OperatorId = x.OperatorId,
                              FriendlyName = wl.OperatorName,
                              PersianDate = wl.PersianDateTime,
                              ProductCode = x.ProductCode,
                              ProductName = x.ProductName,
                              ProcessName = x.ProcessName,
                              ProcessTime = x.ProcessTime,
                              ProcessId = x.ProcessId,
                          }).ToList();

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
                groupedByWIDProcessList = groupedByWIDProcessList.OrderBy(a => a.OperatorId).ThenBy(a => a.InsertDateTime).ToList();

                WorkLineSummary item, prevItem;
                for (int i = 0; i < groupedByWIDProcessList.Count; i++)
                {
                    item = groupedByWIDProcessList[i];

                    if (i != 0)
                        prevItem = groupedByWIDProcessList[i - 1];
                    else
                        prevItem = null;

                    var faDate = Utility.CastToFaDate(item.InsertDateTime);
                    item.PersianDate = Utility.CastToFaDateTime(item.InsertDateTime);
                    item.Year = faDate.Substring(0, 4).ToSafeInt();
                    item.Month = faDate.Substring(5, 2).ToSafeInt();
                    item.Day = faDate.Substring(8, 2).ToSafeInt();

                    if (prevItem?.OperatorId == item.OperatorId && item.Year == prevItem.Year && item.Month == prevItem.Month && item.Day == prevItem.Day)
                        prevItem.ProcessDuration = Math.Truncate((item.InsertDateTime - prevItem.InsertDateTime).TotalMinutes).ToSafeInt();
                }

                var sumSpentTimeResult = from r in groupedByWIDProcessList
                                         group r by new { r.OperatorId, r.FriendlyName } into g
                                         select new WorkLineSummary
                                         {
                                             OperatorId = g.Key.OperatorId,
                                             FriendlyName = g.Key.FriendlyName,
                                             ProcessDuration = g.Sum(a => a.ProcessDuration)
                                         };

                var finalJoinedResult = from r in allowedTimeResult
                                        join s in sumSpentTimeResult on r.OperatorId equals s.OperatorId
                                        select new WorkLineSummary
                                        {
                                            OperatorId = r.OperatorId,
                                            FriendlyName = r.FriendlyName,
                                            ProcessTime = r.ProcessTime,
                                            ProcessDuration = s.ProcessDuration
                                        };


                result = finalJoinedResult;

            }
            else if (reportType == 4)
            {
                var worksheetsDetails = from ws in DBContext.Worksheets
                                        join u in DBContext.Users on ws.OperatorId equals u.Id
                                        join wd in DBContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in DBContext.Products on wd.ProductId equals p.Id
                                        join cat in DBContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in DBContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in DBContext.Processes on pcat.ProcessId equals pro.Id
                                        select new WorkLineHelper
                                        {
                                            WorksheetId = ws.Id,
                                            OperatorId = (int)ws.OperatorId,
                                            OperatorName = u.FriendlyName,
                                            ProductId = wd.ProductId,
                                            ProcessId = pro.Id,
                                            ProductCode = p.Code,
                                            ProductName = cat.Name + " " + p.Name,
                                            ProcessName = pro.Name,
                                            ProcessTime = pcat.ProcessTime,
                                            InsertDateTime = ws.InsertDateTime
                                        };

                var worksheetsDetailsList = new List<WorkLineHelper>();

                if (whereClause != null)
                    worksheetsDetailsList = worksheetsDetails.Where(whereClause).ToList();
                else
                    worksheetsDetailsList = worksheetsDetails.ToList();

                //calcing allowed time
                var operatorAllowedTimeResult = from r in worksheetsDetailsList
                                                group r by new { r.InsertDateTime, r.OperatorId, r.OperatorName } into g
                                                select new WorkLineSummary
                                                {
                                                    InsertDateTime = g.Key.InsertDateTime ?? DateTime.MinValue,
                                                    OperatorId = g.Key.OperatorId,
                                                    FriendlyName = g.Key.OperatorName,
                                                    ProcessTime = g.Sum(a => a.ProcessTime)
                                                };

                var operatorAllowedTimeResultList = operatorAllowedTimeResult.ToList();
                SetDateProps(operatorAllowedTimeResultList);

                var operatorAllowedTimeResultInADay = from r in operatorAllowedTimeResultList
                                                      group r by new { r.Year, r.Month, r.Day, r.OperatorId, r.FriendlyName } into g
                                                      select new WorkLineSummary
                                                      {
                                                          Year = g.Key.Year,
                                                          Month = g.Key.Month,
                                                          Day = g.Key.Day,
                                                          OperatorId = g.Key.OperatorId,
                                                          FriendlyName = g.Key.FriendlyName,
                                                          ProcessTime = g.Sum(a => a.ProcessTime)
                                                      };

                var operatorAllowedTimeResultInADayList = operatorAllowedTimeResultInADay.ToList();

                workLinesSelectList = workLinesSelect.ToList();
                SetDataProps(workLinesSelectList);
                workLinesSelectList = workLinesSelectList.OrderBy(a => a.OperatorId).ThenBy(a => a.InsertDateTime).ToList();

                WorkLineHelper item, prevItem;
                for (int i = 0; i < workLinesSelectList.Count; i++)
                {
                    item = workLinesSelectList[i];

                    if (i != 0)
                        prevItem = workLinesSelectList[i - 1];
                    else
                        prevItem = null;

                    if (prevItem?.WorksheetId == item.WorksheetId && item.Year == prevItem.Year && item.Month == prevItem.Month && item.Day == prevItem.Day)
                        prevItem.ProcessDuration = Math.Truncate(((TimeSpan)(item.InsertDateTime - prevItem.InsertDateTime)).TotalMinutes).ToSafeInt();
                }

                var sumSpentTimeResultOperatorInADay = from r in workLinesSelectList
                                                       group r by new { r.OperatorId, r.Year, r.Month, r.Day } into g
                                                       select new WorkLineSummary
                                                       {
                                                           Year = g.Key.Year,
                                                           Month = g.Key.Month,
                                                           Day = g.Key.Day,
                                                           OperatorId = g.Key.OperatorId,
                                                           ProcessDuration = g.Sum(a => a.ProcessDuration)
                                                       };

                operatorAllowedTimeResultInADayList = operatorAllowedTimeResultInADay.ToList();
                var sumSpentTimeResultOperatorInADayList = sumSpentTimeResultOperatorInADay.ToList();

                foreach (var element in operatorAllowedTimeResultInADayList)
                {
                    var sumItem = sumSpentTimeResultOperatorInADayList.FirstOrDefault(a => a.OperatorId == element.OperatorId && a.Year == element.Year && a.Month == element.Month && a.Day == element.Day);
                    if (sumItem != null)
                    {
                        element.ProcessDuration = sumItem.ProcessDuration;
                    }
                }

                result = operatorAllowedTimeResultInADayList;
            }
            else if (reportType == 6)
            {
                var worksheetsDetails = from ws in DBContext.Worksheets
                                        join u in DBContext.Users on ws.OperatorId equals u.Id
                                        join wd in DBContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in DBContext.Products on wd.ProductId equals p.Id
                                        join cat in DBContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in DBContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in DBContext.Processes on pcat.ProcessId equals pro.Id
                                        select new WorkLineHelper
                                        {
                                            WorksheetId = ws.Id,
                                            OperatorId = (int)ws.OperatorId,
                                            OperatorName = u.FriendlyName,
                                            ProductId = wd.ProductId,
                                            ProcessId = pro.Id,
                                            ProductCode = p.Code,
                                            ProductName = cat.Name + " " + p.Name,
                                            ProcessName = pro.Name,
                                            ProcessTime = pcat.ProcessTime,
                                            InsertDateTime = ws.InsertDateTime
                                        };

                var worksheetsDetailsList = new List<WorkLineHelper>();

                if (whereClause != null)
                    worksheetsDetailsList = worksheetsDetails.Where(whereClause).ToList();
                else
                    worksheetsDetailsList = worksheetsDetails.ToList();

                //calcing allowed time
                var operatorAllowedTimeResult = from r in worksheetsDetailsList
                                                group r by new { r.InsertDateTime, r.OperatorId, r.OperatorName } into g
                                                select new WorkLineSummary
                                                {
                                                    InsertDateTime = g.Key.InsertDateTime ?? DateTime.MinValue,
                                                    OperatorId = g.Key.OperatorId,
                                                    FriendlyName = g.Key.OperatorName,
                                                    ProcessTime = g.Sum(a => a.ProcessTime)
                                                };

                var operatorAllowedTimeResultList = operatorAllowedTimeResult.ToList();
                SetDateProps(operatorAllowedTimeResultList);

                var operatorAllowedTimeResultInAMonth = from r in operatorAllowedTimeResultList
                                                        group r by new { r.Year, r.Month, r.OperatorId, r.FriendlyName } into g
                                                        select new WorkLineSummary
                                                        {
                                                            Year = g.Key.Year,
                                                            Month = g.Key.Month,
                                                            OperatorId = g.Key.OperatorId,
                                                            FriendlyName = g.Key.FriendlyName,
                                                            ProcessTime = g.Sum(a => a.ProcessTime)
                                                        };

                var operatorAllowedTimeResultInAMonthList = operatorAllowedTimeResultInAMonth.ToList();

                workLinesSelectList = workLinesSelect.ToList();
                SetDataProps(workLinesSelectList);
                workLinesSelectList = workLinesSelectList.OrderBy(a => a.OperatorId).ThenBy(a => a.InsertDateTime).ToList();

                WorkLineHelper item, prevItem;
                for (int i = 0; i < workLinesSelectList.Count; i++)
                {
                    item = workLinesSelectList[i];

                    if (i != 0)
                        prevItem = workLinesSelectList[i - 1];
                    else
                        prevItem = null;

                    if (prevItem?.WorksheetId == item.WorksheetId && item.Year == prevItem.Year && item.Month == prevItem.Month)
                        prevItem.ProcessDuration = Math.Truncate(((TimeSpan)(item.InsertDateTime - prevItem.InsertDateTime)).TotalMinutes).ToSafeInt();
                }

                var sumSpentTimeResultOperatorInAMonth = from r in workLinesSelectList
                                                         group r by new { r.OperatorId, r.Year, r.Month } into g
                                                         select new WorkLineSummary
                                                         {
                                                             Year = g.Key.Year,
                                                             Month = g.Key.Month,
                                                             OperatorId = g.Key.OperatorId,
                                                             ProcessDuration = g.Sum(a => a.ProcessDuration)
                                                         };

                operatorAllowedTimeResultInAMonthList = operatorAllowedTimeResultInAMonthList.ToList();
                var sumSpentTimeResultOperatorInAMonthList = sumSpentTimeResultOperatorInAMonth.ToList();

                foreach (var element in operatorAllowedTimeResultInAMonthList)
                {
                    var sumItem = sumSpentTimeResultOperatorInAMonthList.FirstOrDefault(a => a.OperatorId == element.OperatorId && a.Year == element.Year && a.Month == element.Month);
                    if (sumItem != null)
                    {
                        element.ProcessDuration = sumItem.ProcessDuration;
                    }
                }

                result = operatorAllowedTimeResultInAMonthList;
            }
            else if (reportType == 7 || reportType == 8) //takhir ya tajil roozanhe bar asase process
            {
                var worksheetsDetails = from ws in DBContext.Worksheets
                                        join u in DBContext.Users on ws.OperatorId equals u.Id
                                        join wd in DBContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in DBContext.Products on wd.ProductId equals p.Id
                                        join cat in DBContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in DBContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in DBContext.Processes on pcat.ProcessId equals pro.Id
                                        select new WorkLineHelper
                                        {
                                            WorksheetId = ws.Id,
                                            OperatorId = (int)ws.OperatorId,
                                            OperatorName = u.FriendlyName,
                                            ProductId = wd.ProductId,
                                            ProcessId = pro.Id,
                                            ProductCode = p.Code,
                                            ProductName = cat.Name + " " + p.Name,
                                            ProcessName = pro.Name,
                                            ProcessTime = pcat.ProcessTime,
                                            InsertDateTime = ws.InsertDateTime
                                        };

                var worksheetsDetailsList = new List<WorkLineHelper>();

                if (whereClause != null)
                    worksheetsDetailsList = worksheetsDetails.Where(whereClause).ToList();
                else
                    worksheetsDetailsList = worksheetsDetails.ToList();

                //calcing allowed time
                var operatorProcessAllowedTimeResult = from r in worksheetsDetailsList
                                                       group r by new { r.InsertDateTime, r.ProcessId, r.ProcessName, r.OperatorId, r.OperatorName } into g
                                                       select new WorkLineSummary
                                                       {
                                                           InsertDateTime = g.Key.InsertDateTime ?? DateTime.MinValue,
                                                           ProcessId = g.Key.ProcessId,
                                                           ProcessName = g.Key.ProcessName,
                                                           OperatorId = g.Key.OperatorId,
                                                           FriendlyName = g.Key.OperatorName,
                                                           ProcessTime = g.Sum(a => a.ProcessTime)
                                                       };

                var operatorProcessAllowedTimeResultList = operatorProcessAllowedTimeResult.ToList();
                SetDateProps(operatorProcessAllowedTimeResultList);

                var operatorProcessAllowedTimeResultInADay = from r in operatorProcessAllowedTimeResultList
                                                             group r by new { r.Year, r.Month, r.Day, r.ProcessId, r.ProcessName, r.OperatorId, r.FriendlyName } into g
                                                             select new WorkLineSummary
                                                             {
                                                                 Year = g.Key.Year,
                                                                 Month = g.Key.Month,
                                                                 Day = g.Key.Day,
                                                                 OperatorId = g.Key.OperatorId,
                                                                 FriendlyName = g.Key.FriendlyName,
                                                                 ProcessId = g.Key.ProcessId,
                                                                 ProcessName = g.Key.ProcessName,
                                                                 ProcessTime = g.Sum(a => a.ProcessTime)
                                                             };

                var operatorProcessAllowedTimeResultInADayList = operatorProcessAllowedTimeResultInADay.ToList();

                workLinesSelectList = workLinesSelect.ToList();
                SetDataProps(workLinesSelectList);
                workLinesSelectList = workLinesSelectList.OrderBy(a => a.OperatorId).ThenBy(a => a.InsertDateTime).ToList();

                WorkLineHelper item, prevItem;
                for (int i = 0; i < workLinesSelectList.Count; i++)
                {
                    item = workLinesSelectList[i];

                    if (i != 0)
                        prevItem = workLinesSelectList[i - 1];
                    else
                        prevItem = null;

                    if (prevItem?.WorksheetId == item.WorksheetId && item.Year == prevItem.Year && item.Month == prevItem.Month && item.Day == prevItem.Day)
                        prevItem.ProcessDuration = Math.Truncate(((TimeSpan)(item.InsertDateTime - prevItem.InsertDateTime)).TotalMinutes).ToSafeInt();
                }

                var sumSpentTimeResultOperatorProcessInADay = from r in workLinesSelectList
                                                              group r by new { r.OperatorId, r.ProcessId, r.ProcessName, r.Year, r.Month, r.Day } into g
                                                              select new WorkLineSummary
                                                              {
                                                                  Year = g.Key.Year,
                                                                  Month = g.Key.Month,
                                                                  Day = g.Key.Day,
                                                                  ProcessId = g.Key.ProcessId,
                                                                  ProcessName = g.Key.ProcessName,
                                                                  OperatorId = g.Key.OperatorId,
                                                                  ProcessDuration = g.Sum(a => a.ProcessDuration)
                                                              };

                operatorProcessAllowedTimeResultInADayList = operatorProcessAllowedTimeResultInADayList.ToList();
                var sumSpentTimeResultOperatorProcessInADayList = sumSpentTimeResultOperatorProcessInADay.ToList();

                var finalResult = new List<WorkLineSummary>();

                foreach (var element in operatorProcessAllowedTimeResultInADayList)
                {
                    var sumItem = sumSpentTimeResultOperatorProcessInADayList.FirstOrDefault(a => a.OperatorId == element.OperatorId && a.Year == element.Year && a.Month == element.Month && a.Day == element.Day && a.ProcessId == element.ProcessId);

                    //takhir
                    if (reportType == 7 && sumItem != null && element.ProcessTime < sumItem.ProcessDuration)
                    {
                        element.ProcessDuration = sumItem.ProcessDuration;
                        element.DiffTime = element.ProcessTime - element.ProcessDuration;
                        finalResult.Add(element);
                    }
                    else if (reportType == 8 && sumItem != null && element.ProcessTime > sumItem.ProcessDuration)
                    {
                        element.ProcessDuration = sumItem.ProcessDuration;
                        element.DiffTime = element.ProcessTime - element.ProcessDuration;
                        finalResult.Add(element);
                    }
                }

                result = finalResult;
            }
            else if (reportType == 9 || reportType == 10) //takhir ya tajil mahane bar asase process
            {
                var worksheetsDetails = from ws in DBContext.Worksheets
                                        join u in DBContext.Users on ws.OperatorId equals u.Id
                                        join wd in DBContext.WorksheetDetails on ws.Id equals wd.WorksheetId
                                        join p in DBContext.Products on wd.ProductId equals p.Id
                                        join cat in DBContext.Categories on p.ProductCategoryId equals cat.Id
                                        join pcat in DBContext.ProcessCategories on cat.Id equals pcat.CategoryId
                                        join pro in DBContext.Processes on pcat.ProcessId equals pro.Id
                                        select new WorkLineHelper
                                        {
                                            WorksheetId = ws.Id,
                                            OperatorId = (int)ws.OperatorId,
                                            OperatorName = u.FriendlyName,
                                            ProductId = wd.ProductId,
                                            ProcessId = pro.Id,
                                            ProductCode = p.Code,
                                            ProductName = cat.Name + " " + p.Name,
                                            ProcessName = pro.Name,
                                            ProcessTime = pcat.ProcessTime,
                                            InsertDateTime = ws.InsertDateTime
                                        };

                var worksheetsDetailsList = new List<WorkLineHelper>();

                if (whereClause != null)
                    worksheetsDetailsList = worksheetsDetails.Where(whereClause).ToList();
                else
                    worksheetsDetailsList = worksheetsDetails.ToList();

                //calcing allowed time
                var operatorProcessAllowedTimeResult = from r in worksheetsDetailsList
                                                       group r by new { r.InsertDateTime, r.ProcessId, r.ProcessName, r.OperatorId, r.OperatorName } into g
                                                       select new WorkLineSummary
                                                       {
                                                           InsertDateTime = g.Key.InsertDateTime ?? DateTime.MinValue,
                                                           ProcessId = g.Key.ProcessId,
                                                           ProcessName = g.Key.ProcessName,
                                                           OperatorId = g.Key.OperatorId,
                                                           FriendlyName = g.Key.OperatorName,
                                                           ProcessTime = g.Sum(a => a.ProcessTime)
                                                       };

                var operatorProcessAllowedTimeResultList = operatorProcessAllowedTimeResult.ToList();
                SetDateProps(operatorProcessAllowedTimeResultList);

                var operatorProcessAllowedTimeResultInADay = from r in operatorProcessAllowedTimeResultList
                                                             group r by new { r.Year, r.Month, r.ProcessId, r.ProcessName, r.OperatorId, r.FriendlyName } into g
                                                             select new WorkLineSummary
                                                             {
                                                                 Year = g.Key.Year,
                                                                 Month = g.Key.Month,
                                                                 OperatorId = g.Key.OperatorId,
                                                                 FriendlyName = g.Key.FriendlyName,
                                                                 ProcessId = g.Key.ProcessId,
                                                                 ProcessName = g.Key.ProcessName,
                                                                 ProcessTime = g.Sum(a => a.ProcessTime)
                                                             };

                var operatorProcessAllowedTimeResultInADayList = operatorProcessAllowedTimeResultInADay.ToList();

                workLinesSelectList = workLinesSelect.ToList();
                SetDataProps(workLinesSelectList);
                workLinesSelectList = workLinesSelectList.OrderBy(a => a.OperatorId).ThenBy(a => a.InsertDateTime).ToList();

                WorkLineHelper item, prevItem;
                for (int i = 0; i < workLinesSelectList.Count; i++)
                {
                    item = workLinesSelectList[i];

                    if (i != 0)
                        prevItem = workLinesSelectList[i - 1];
                    else
                        prevItem = null;

                    if (prevItem?.WorksheetId == item.WorksheetId && item.Year == prevItem.Year && item.Month == prevItem.Month)
                        prevItem.ProcessDuration = Math.Truncate(((TimeSpan)(item.InsertDateTime - prevItem.InsertDateTime)).TotalMinutes).ToSafeInt();
                }

                var sumSpentTimeResultOperatorProcessInADay = from r in workLinesSelectList
                                                              group r by new { r.OperatorId, r.ProcessId, r.ProcessName, r.Year, r.Month } into g
                                                              select new WorkLineSummary
                                                              {
                                                                  Year = g.Key.Year,
                                                                  Month = g.Key.Month,
                                                                  ProcessId = g.Key.ProcessId,
                                                                  ProcessName = g.Key.ProcessName,
                                                                  OperatorId = g.Key.OperatorId,
                                                                  ProcessDuration = g.Sum(a => a.ProcessDuration)
                                                              };

                operatorProcessAllowedTimeResultInADayList = operatorProcessAllowedTimeResultInADayList.ToList();
                var sumSpentTimeResultOperatorProcessInADayList = sumSpentTimeResultOperatorProcessInADay.ToList();

                var finalResult = new List<WorkLineSummary>();

                foreach (var element in operatorProcessAllowedTimeResultInADayList)
                {
                    var sumItem = sumSpentTimeResultOperatorProcessInADayList.FirstOrDefault(a => a.OperatorId == element.OperatorId && a.Year == element.Year && a.Month == element.Month && a.Day == element.Day && a.ProcessId == element.ProcessId);

                    //takhir
                    if (reportType == 9 && sumItem != null && element.ProcessTime < sumItem.ProcessDuration)
                    {
                        element.ProcessDuration = sumItem.ProcessDuration;
                        element.DiffTime = element.ProcessTime - element.ProcessDuration;
                        finalResult.Add(element);
                    }
                    else if (reportType == 10 && sumItem != null && element.ProcessTime > sumItem.ProcessDuration)
                    {
                        element.ProcessDuration = sumItem.ProcessDuration;
                        element.DiffTime = element.ProcessTime - element.ProcessDuration;
                        finalResult.Add(element);
                    }
                }

                result = finalResult;
            }


            return result.ToList();
        }

        private static void SetDataProps(List<WorkLineHelper> workLineHelperList)
        {
            foreach (WorkLineHelper it in workLineHelperList)
            {
                var faDate = Utility.CastToFaDate(it.InsertDateTime);
                it.PersianDate = faDate;
                it.PersianDateTime = Utility.CastToFaDateTime(it.InsertDateTime); ;
                it.Year = faDate.Substring(0, 4).ToSafeInt();
                it.Month = faDate.Substring(5, 2).ToSafeInt();
                it.Day = faDate.Substring(8, 2).ToSafeInt();
            }
        }

        private static void SetDateProps(List<WorkLineSummary> summaryList)
        {
            foreach (var it in summaryList)
            {
                var faDate = Utility.CastToFaDate(it.InsertDateTime);
                it.PersianDate = faDate;
                it.PersianDateTime = Utility.CastToFaDateTime(it.InsertDateTime); ;
                it.Year = faDate.Substring(0, 4).ToSafeInt();
                it.Month = faDate.Substring(5, 2).ToSafeInt();
                it.Day = faDate.Substring(8, 2).ToSafeInt();
            }
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
        public int ProcessDuration { get; set; }
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
        public int DiffTime { get; set; }
    }
}
