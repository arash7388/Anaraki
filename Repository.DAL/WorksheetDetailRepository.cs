using Repository.Entity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL
{
    public class WorksheetDetailRepository : BaseRepository<WorksheetDetail>
    {
    }

    public class WorksheetDetailHelper
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

}
