using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class AscentSAInfo
    {
        public String Input { get; set; }
        public String Sign { get; set; }
        public String Stack { get; set; }
        public String Base { get; set; }
        public String PolishNote { get; set; }

        public AscentSAInfo()
        {
            Base = "";
        }
    }
}
