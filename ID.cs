using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class ID
    {
        public int Index { get; set; }
        public String Value { get; set; }
        public double NumberValue { get; set; }


        public ID(int index, String id)
        {
            Index = index;
            Value = id;
        }
    }
}
