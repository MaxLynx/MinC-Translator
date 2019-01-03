using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class Sentence
    {
        public String Head { get; set; }
        public String[] Tail { get; set; }

        public Sentence(String head, String tail)
        {
            Head = head;
            Tail = tail.Split(' ');
        }
    }
}
