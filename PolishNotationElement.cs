using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class PolishNotationElement
    {
        public String Value { get; set; }
        public Boolean IsOperator { get; set; }
        public Boolean IsVariable { get; set; }


        public PolishNotationElement(String value, Boolean isOperator)
        {
            Value = value;
            IsOperator = isOperator;
            IsVariable = false;
        }
        public PolishNotationElement(String value, Boolean isOperator, Boolean isVariable)
        {
            Value = value;
            IsOperator = isOperator;
            IsVariable = isVariable;
        }
    }
}
