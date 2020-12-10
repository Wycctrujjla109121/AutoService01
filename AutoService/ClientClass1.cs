using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.ApplicationData
{
    public partial class Client
    {
        public int AmountVisit
        {
            get
            {
                return ClientService.Count;
            }
        }

        public DateTime? LastVisit
        {
            get
            {
                if (AmountVisit > 0)
                {
                    return ClientService.Max(c => c.DateTimeStart);
                }
                return null;
            }
        }
    }
}
