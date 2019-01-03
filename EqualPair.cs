using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class EqualPair
    {
        public String Symbol1 { get; set; }
        public String Symbol2 { get; set; }
        public int Type { get; set; }

        public EqualPair(String s1, String s2, int type)
        {
            Symbol1 = s1;
            Symbol2 = s2;
            Type = type;
        }
    }
}
