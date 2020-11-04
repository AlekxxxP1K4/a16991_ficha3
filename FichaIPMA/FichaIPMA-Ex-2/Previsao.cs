using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FichaIPMA
{
    public class Previsao
    {
        public string owner { get; set; }
        public string country { get; set; }
        public Daily[] data { get; set; }
        public int globalIdLocal { get; set; }
        public DateTime dataUpdate { get; set; }

        // ---- 
        public string local { get; set; }
    }
}
