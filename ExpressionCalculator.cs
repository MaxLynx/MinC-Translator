using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class ExpressionCalculator
    {
        public static string calculate(List<PolishNotationElement> notation)
        {
            while (notation.Count > 1)
            {
                int i = 0;
                List<PolishNotationElement> buffer = new List<PolishNotationElement>();
                while (i < notation.Count - 2)
                {
                    if (notation[i + 1].Value.Equals("@") && !notation[i].IsOperator)
                    {
                        buffer.Add(new PolishNotationElement((-Double.Parse(notation[i].Value)).ToString(), false));
                        i += 2;
                        break;
                    }
                    else
                    if (notation[i + 2].IsOperator && !notation[i].IsOperator && !notation[i + 1].IsOperator
                        && !notation[i + 2].Value.Equals("@"))
                    {
                        switch (notation[i + 2].Value)
                        {
                            case "+":
                                buffer.Add(new PolishNotationElement((Double.Parse(notation[i].Value) +
                                    Double.Parse(notation[i + 1].Value)).ToString(), false));
                                break;
                            case "-":
                                buffer.Add(new PolishNotationElement((Double.Parse(notation[i].Value) -
                                    Double.Parse(notation[i + 1].Value)).ToString(), false));
                                break;
                            case "*":
                                buffer.Add(new PolishNotationElement((Double.Parse(notation[i].Value) *
                                    Double.Parse(notation[i + 1].Value)).ToString(), false));
                                break;
                            case "/":
                                buffer.Add(new PolishNotationElement((Double.Parse(notation[i].Value) /
                                    Double.Parse(notation[i + 1].Value)).ToString(), false));
                                break;
                        }
                        i += 3;
                        break;
                    }
                    else
                    {
                        buffer.Add(notation[i++]);
                    }

                }
                while (i < notation.Count)
                {
                    buffer.Add(notation[i++]);
                }

                notation = buffer;
            }
            return notation[0].Value;
        }
    }
}
