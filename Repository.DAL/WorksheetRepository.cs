using Repository.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL
{
    public class WorksheetRepository : BaseRepository<Worksheet>
    {
        public Worksheet GetByIdWithDetails(int id)
        {
            var det = MTOContext.WorksheetDetails.Where(d => d.WorksheetId == id)
                    .Include(b => b.Product).ToList();

            var w = MTOContext.Worksheets.SingleOrDefault(a => a.Id == id);
            w.WorksheetDetails = det;
            return w;
        }

        public int GetMaxId()
        {
            if (!MTOContext.Worksheets.Any())
                return 0;

            var id = MTOContext.Worksheets.Select(a => a.Id).Max();
            return id;
        }
        public object GetWorksheetForPrint(int id)
        {
            throw new NotImplementedException();
        }

        public List<int> GetWorksheetProcesses(int worksheetId)
        {
            var result = new List<int>();

            var w = MTOContext.Worksheets.Include(a=>a.WorksheetDetails)
                                .FirstOrDefault(a => a.Id == worksheetId);

            foreach(WorksheetDetail d in w.WorksheetDetails)
            {
                var product = MTOContext.Products.Include(a=>a.ProductCategory).FirstOrDefault(a => a.Id == d.ProductId);
                var processesOfThisCat = MTOContext.ProcessCategories.Where(a => a.CategoryId == product.ProductCategoryId);

                foreach(var p in processesOfThisCat)
                {
                    if (!result.Contains(p.ProcessId))
                        result.Add(p.ProcessId);
                }
            }

            result.Sort();

            return result;
        }
    }
}
