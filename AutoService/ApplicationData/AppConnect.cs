using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.ApplicationData
{
    class AppConnect
    {
        public static AutoServiceEntities modelOdb { get; } = new AutoServiceEntities();
    }
}
