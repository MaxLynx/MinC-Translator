using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class SA3
    {
        private List<Token> lexemes;
        public List<AscentSAInfo> Information { set; get; }
        public List<PolishNotationElement> PolishNotation { set; get; }
        private int currentRow;
        private List<Token> inputLine;
        private List<Token> workLine;
        PrecedenceTableBuilder builder;
        String[,] table;

        public Boolean Errors { set; get; }

        public String process(List<Token> lexemes)
        {
            builder = new PrecedenceTableBuilder();
            PolishNotation = new List<PolishNotationElement>();
            table = builder.process();
            Errors = false;
            this.lexemes = lexemes;
            Information = new List<AscentSAInfo>();
            currentRow = 1;
            inputLine = lexemes;
            inputLine.Add(new Token(lexemes[lexemes.Count - 1].Row, "#", 0, 0));
            workLine = new List<Token>();
            workLine.Add(new Token(1, "#", 0, 0));
            while (inputLine.Count != 0)
            {
                AscentSAInfo info = new AscentSAInfo();
                if ((workLine.Count == 2) && (workLine[1].GeneralizedValue.Equals("@программа")))
                {
                    if (inputLine.Count != 1)
                    {
                        Errors = true;
                        PolishNotation = null;
                        return "У ПРОГРАМИ НЕПРАВИЛЬНИЙ СИНТАКСИС\r\n";
                    }
                    return "У ПРОГРАМИ ПРАВИЛЬНИЙ СИНТАКСИС!\r\n";
                }
                info.Input = "";
                foreach (Token lex in inputLine)
                {
                    info.Input += lex.GeneralizedValue + " ";
                }
                info.Stack += "";
                foreach (Token el in workLine)
                {
                    info.Stack += el.GeneralizedValue + " ";
                }
                String sign = getSign(workLine[workLine.Count - 1].GeneralizedValue, inputLine[0].GeneralizedValue);
                info.Sign = workLine[workLine.Count - 1].GeneralizedValue + " " + sign + " "
                    + inputLine[0].GeneralizedValue;
                if (sign.Contains("<")
                    || sign.Contains("=")
                    || sign.Equals(" "))
                {
                    currentRow = inputLine[0].Row;
                    workLine.Add(inputLine[0]);
                    inputLine.Remove(inputLine[0]);
                    info.PolishNote = "";
                    foreach (PolishNotationElement el in PolishNotation)
                    {
                        info.PolishNote += el.Value + " ";
                    }
                    Information.Add(info);
                    continue;
                }
                if (sign.Contains(">"))
                {
                    List<String> baseList = new List<String>();
                    int i = workLine.Count;
                    do
                    {
                        i--;
                        baseList.Add(workLine[i].GeneralizedValue);
                    }
                    while (getSign(workLine[i - 1].GeneralizedValue, workLine[i].GeneralizedValue).Contains("="));
                    baseList.Reverse();
                    info.Base = "";
                    foreach (String el in baseList)
                    {
                        info.Base += el + " ";
                    }
                    String idName = "";
                    String conValue = "";
                    if (baseList.Count == 1)
                    {
                        if (workLine[workLine.Count - 1].GeneralizedValue.Equals("id"))
                            idName = workLine[workLine.Count - 1].Value;
                        if (workLine[workLine.Count - 1].GeneralizedValue.Equals("const"))
                            conValue = workLine[workLine.Count - 1].Value;
                    }
                    String newStackEl = change(baseList, idName, conValue);
                    workLine.RemoveRange(i, workLine.Count - i);
                    workLine.Add(new Token(0, newStackEl, 0, currentRow));
                    info.PolishNote = "";
                    foreach (PolishNotationElement el in PolishNotation)
                    {
                        info.PolishNote += el.Value + " ";
                    }
                    Information.Add(info);
                    continue;
                }
                {
                    Information.Add(info);
                    Errors = true;
                    PolishNotation = null;
                    return "У ПРОГРАМИ НЕПРАВИЛЬНИЙ СИНТАКСИС\r\n" + "Помилка в рядку " + currentRow + "\r\n";
                }
            }
            if ((workLine.Count == 2) && (workLine[1].Equals("@программа")))
            {
                return "У ПРОГРАМИ ПРАВИЛЬНИЙ СИНТАКСИС!\r\n";
            }
            else
            {
                Errors = true;
                return "У ПРОГРАМИ НЕПРАВИЛЬНИЙ СИНТАКСИС\r\n";
            }
        }

        private String getSign(String sign1, String sign2)
        {
            return table[builder.getSymbolIndex(sign1), builder.getSymbolIndex(sign2)];
        }
        private String change(List<String> symbols, String idName, String conValue)
        {
            String result = builder.findAndChange(symbols);
            if (!result.Equals(" "))
            {
                if (compare(symbols, new Sentence("", "@выражение + @терм1").Tail))
                    PolishNotation.Add(new PolishNotationElement("+", true));
                else
                if (compare(symbols, new Sentence("", "@выражение - @терм1").Tail))
                    PolishNotation.Add(new PolishNotationElement("-", true));
                else
                if (compare(symbols, new Sentence("", "@терм * @множитель").Tail))
                    PolishNotation.Add(new PolishNotationElement("*", true));
                else
                if (compare(symbols, new Sentence("", "@терм / @множитель").Tail))
                    PolishNotation.Add(new PolishNotationElement("/", true));
                else
                if (compare(symbols, new Sentence("", "- @терм1").Tail))
                    PolishNotation.Add(new PolishNotationElement("@", true));
                else
                if (compare(symbols, new Sentence("", "id").Tail))
                    PolishNotation.Add(new PolishNotationElement(idName, false, true));
                else
                if (compare(symbols, new Sentence("", "const").Tail))
                    PolishNotation.Add(new PolishNotationElement(conValue, false, false));
            }
            return result;
        }

        private bool compare(List<String> symbols1, String[] symbols2)
        {
            if (symbols1.Count == symbols2.Length)
            {
                int i = 0;
                foreach (String symbol in symbols1)
                {
                    if (!symbol.Equals(symbols2[i++]))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
        }
    }
}
