using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class ProcessCategory
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Process Process { get; set; }
        public int ProcessId { get; set; }

        public int Order { get; set; }

    }
}
