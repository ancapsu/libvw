using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibVisWeb.Models
{
    
    public class GenericStatusModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

    }
    
    public class GenericIdModel
    {

        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public string Id { get; set; }

    }

}
