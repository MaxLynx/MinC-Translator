using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class Token
    {
        public int Row { get; set; }
        public String Value { get; set; }
        public int Index { get; set; }
        public int ClassInnerIndex { get; set; }
        public String GeneralizedValue {
            get
            {
                if (Index == 37)
                    return "id";
                if (Index == 38)
                    return "const";
                if (Index == 39)
                    return "lbl";
                return Value;
            }
        }

        public Token(int row, String token, int index)
        {
            Row = row;
            Value = token;
            Index = index;
            ClassInnerIndex = 0;
        }
        public Token(Token token)
        {
            Row = token.Row;
            Value = token.Value;
            Index = token.Index;
            ClassInnerIndex = token.ClassInnerIndex;
        }
        public Token(int row, String token, int index, int classInnerIndex)
        {
            Row = row;
            Value = token;
            Index = index;
            ClassInnerIndex = classInnerIndex;
        }
    }
}
