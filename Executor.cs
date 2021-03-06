﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class Executor
    {
        List<String> Code { get; set; }
        List<ID> IDs { get; set; }
        private List<String> stack;
        public String Result { get; set; }
        private int currentIndex = 0;
        private String[] operators = {
            "wr", "rd", "+", "-", "*", "/", "@", "<", "<=", ">", ">=", "==", "!=",
            "=", "not", "&&", "||", "УПЛ", "БП"
        };
        public String ReadVar;
        public Double ReadVarValue;

        public Executor(List<String> code, List<ID> ids)
        {
            Code = code;
            IDs = ids;
            Result = "";
        }

        public bool process()
        {
            if (currentIndex == 0)
                stack = new List<String>();
            else
            {
                getIDByName(ReadVar).NumberValue = ReadVarValue;
            }
            for (int i = currentIndex; i < Code.Count; i++)
            {
                if (contains(operators, Code[i]))
                {
                    if (Code[i].Equals("wr"))
                    {
                        Result += getIDByName(stack[stack.Count - 1]).Value + " = "
                            + getIDByName(stack[stack.Count - 1]).NumberValue + "\r\n";
                    }
                    else
                    if (Code[i].Equals("+"))
                    {
                        stack[stack.Count - 2] = (getNumberValue(stack[stack.Count - 2])
                            + getNumberValue(stack[stack.Count - 1])).ToString();
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("-"))
                    {
                        stack[stack.Count - 2] = (getNumberValue(stack[stack.Count - 2])
                            - getNumberValue(stack[stack.Count - 1])).ToString();
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("@"))
                    {
                        stack[stack.Count - 1] = (-getNumberValue(stack[stack.Count - 1])).ToString();
                    }
                    else
                    if (Code[i].Equals("*"))
                    {
                        stack[stack.Count - 2] = (getNumberValue(stack[stack.Count - 2])
                            * getNumberValue(stack[stack.Count - 1])).ToString();
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("/"))
                    {
                        stack[stack.Count - 2] = (getNumberValue(stack[stack.Count - 2])
                            / getNumberValue(stack[stack.Count - 1])).ToString();
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("@"))
                    {
                        stack[stack.Count - 2] = (-getNumberValue(stack[stack.Count - 1])).ToString();
                    }
                    else
                    if (Code[i].Equals("<"))
                    {
                        stack[stack.Count - 2] = getNumberValue(stack[stack.Count - 2])
                            < getNumberValue(stack[stack.Count - 1]) ?
                            "True" : "False";
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("<="))
                    {
                        stack[stack.Count - 2] = getNumberValue(stack[stack.Count - 2])
                            <= getNumberValue(stack[stack.Count - 1]) ?
                            "True" : "False";
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals(">"))
                    {
                        stack[stack.Count - 2] = getNumberValue(stack[stack.Count - 2])
                            > getNumberValue(stack[stack.Count - 1]) ?
                            "True" : "False";
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals(">="))
                    {
                        stack[stack.Count - 2] = getNumberValue(stack[stack.Count - 2])
                            >= getNumberValue(stack[stack.Count - 1]) ?
                            "True" : "False";
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("=="))
                    {
                        stack[stack.Count - 2] = getNumberValue(stack[stack.Count - 2])
                            == getNumberValue(stack[stack.Count - 1]) ?
                            "True" : "False";
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("!="))
                    {
                        stack[stack.Count - 2] = getNumberValue(stack[stack.Count - 2])
                            != getNumberValue(stack[stack.Count - 1]) ?
                            "True" : "False";
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("not"))
                    {
                        stack[stack.Count - 1] =
                            stack[stack.Count - 1].Equals("False") ?
                            "True" : "False";
                    }
                    else
                    if (Code[i].Equals("&&"))
                    {
                        if (stack[stack.Count - 2].Equals("True") && stack[stack.Count - 1].Equals("True"))
                        {

                            stack[stack.Count - 2] = "True";
                        }
                        else
                        {
                            stack[stack.Count - 2] = "False";
                        }

                        stack.RemoveAt(stack.Count - 1);
                    }
                    if (Code[i].Equals("||"))
                    {
                        if (stack[stack.Count - 2].Equals("False") && stack[stack.Count - 1].Equals("False"))
                        {
                            stack[stack.Count - 2] = "False";
                        }
                        else
                        {
                            stack[stack.Count - 2] = "True";
                        }

                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    if (Code[i].Equals("="))
                    {
                        ID variable = getIDByName(stack[stack.Count - 2]);
                        variable.NumberValue = getNumberValue(stack[stack.Count - 1]);
                        stack.RemoveAt(stack.Count - 1);
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                if (Code[i].Equals("БП"))
                    {
                        int lblPlace = getLblPlace(stack[stack.Count - 1]);
                        if (lblPlace != -1)
                        {
                            i = lblPlace;
                            stack.RemoveAt(stack.Count - 1);
                            continue;
                        }
                    }
                    else
                if (Code[i].Equals("УПЛ"))
                    {
                        if (stack[stack.Count - 2].Equals("False"))
                        {
                            int lblPlace = getLblPlace(stack[stack.Count - 1]);
                            if (lblPlace != -1)
                            {
                                i = lblPlace;
                                continue;
                            }
                        }
                        stack.RemoveAt(stack.Count - 1);
                        stack.RemoveAt(stack.Count - 1);

                    }
                    else
                if (Code[i].Equals("rd"))
                    {
                        currentIndex = i + 1;
                        ReadVar = stack[stack.Count - 1];
                        stack.RemoveAt(stack.Count - 1);
                        return false;
                    }
                }
                else
                if (Code[i][Code[i].Length - 1] == ':')
                {
                    continue;
                }
                else
                {
                    stack.Add(Code[i]);
                }
            }

            return true;
        }

        private static bool contains(String[] array, String str)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(str)) return true;
            }
            return false;
        }
        private Double getNumberValue(String value)
        {
            double result;
            if (Double.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            else
            {
                return getIDByName(value).NumberValue;
            }
        }
        private ID getIDByName(String name)
        {
            foreach (ID id in IDs)
            {
                if (id.Value.Equals(name))
                {
                    return id;
                }
            }
            IDs.Add(new ID(IDs.Count + 1, name));
            return IDs[IDs.Count - 1];
        }
        private int getLblPlace(String name)
        {
            for (int i = 0; i < Code.Count; i++)
            {
                if (Code[i].Equals(name + ":"))
                    return i;
            }
            return -1;
        }
    }
}
