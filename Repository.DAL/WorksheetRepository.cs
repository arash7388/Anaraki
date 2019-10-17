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

        public object GetWorksheetForPrint(int v)
        {
            throw new NotImplementedException();
        }
    }
}
