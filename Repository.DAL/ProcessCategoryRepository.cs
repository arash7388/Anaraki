using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;
using Repository.Data.Migrations;
using Repository.Entity.Domain;

namespace Repository.DAL
{
    public class ProcessCategoryRepository:BaseRepository<ProcessCategory>
    {
        public List<ProcessCategoryHelper> GetByCatIdWithDetails(int catId)
        {
            var result = (from pc in MTOContext.ProcessCategories
                          join p in MTOContext.Processes on pc.ProcessId equals p.Id
                          join c in MTOContext.Categories on pc.CategoryId equals c.Id
                          where pc.CategoryId==catId
                         
                          select new ProcessCategoryHelper
                          {
                              Id = pc.Id,
                              CategoryId = pc.CategoryId,
                              CategoryName = c.Name,
                              ProcessId = pc.ProcessId,
                              ProcessName = p.Name,
                              Order = pc.Order
                          }).ToList();

            return result;
        }

        public List<ProCatHelper> GetGroupedList()
        {
            var result = (from pc in MTOContext.ProcessCategories
                         join p in MTOContext.Processes on pc.ProcessId equals p.Id
                         join c in MTOContext.Categories on pc.CategoryId equals c.Id
                         group pc by new { pc.CategoryId, c.Name } into g
                         select new ProCatHelper
                         {
                             CategoryId = g.Key.CategoryId,
                             CategoryName = g.Key.Name,
                             //ProcessId=p.Id,
                             //ProcessNames= string.Join(",", MTOContext.ProcessCategories.Include("Process").Where(a=>a.CategoryId== g.Key.CategoryId).Select(a=>a.Process.Name).ToArray())
                             //ProcessNames = MTOContext.ProcessCategories.Select(a => a.ProcessId.ToString()).FirstOrDefault()
                         }).ToList();

            foreach(var r in result)
            {
                r.ProcessNames = string.Join("," , MTOContext.ProcessCategories.Include("Process").Where(a => a.CategoryId == r.CategoryId).Select(a => a.Process.Name).ToArray());
            }

            return result.ToList();
        }
    }

    public class ProCatHelper
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string ProcessNames { get; set; }
    }

    public class ProcessCategoryHelper
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int ProcessId { get; set; }
        public string ProcessName { get; set; }

        public int Order { get; set; }
    }
}
