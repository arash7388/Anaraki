using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class WorkLine: BaseEntity
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Process Process { get; set; }
        public int ProcessId { get; set; }

        public User Operator { get; set; }
        public int OperatorId { get; set; }

    }
}
