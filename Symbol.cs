using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class Symbol
    {
        public String Name { get; set; }
        public HashSet<String> FirstPlusSet { get; set; }
        public HashSet<String> LastPlusSet { get; set; }

        public Symbol(String name, HashSet<String> first, HashSet<String> last)
        {
            Name = name;
            FirstPlusSet = first;
            LastPlusSet = last;
        }
    }
}
