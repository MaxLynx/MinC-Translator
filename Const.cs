using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class Const
    {
        public int Index { get; set; }
        public String Value { get; set; }

        public Const(int index, String val)
        {
            Index = index;
            Value = val;
        }
    }
}
