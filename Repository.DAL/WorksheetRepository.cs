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
    }
}
