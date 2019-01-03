using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class Label
    {
        public int Index { get; set; }
        public String Name { get; set; }
        public bool Used { get; set; }
        public bool Referenced { get; set; }


        public Label(int index, String name)
        {
            Index = index;
            Name = name;
            Used = false;
            Referenced = false;
        }
    }
}
