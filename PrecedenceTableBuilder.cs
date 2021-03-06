﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class PrecedenceTableBuilder
    {
        private Sentence[] grammar =
       {
            new Sentence("@программа", "id program @объявление { @список_операторов1 }."),
            new Sentence("@список_операторов1", "@список_операторов"),
            new Sentence("@объявление", "@объявление_переменных ;"),
            new Sentence("@объявление", "@объявление_переменных ; @объявление_меток ;"),
            new Sentence("@объявление_переменных", "float @список_ид1"),
            new Sentence("@список_ид1", "@список_ид"),
            new Sentence("@объявление_меток", "label @список_меток1"),
            new Sentence("@список_меток1", "@список_меток"),
            new Sentence("@список_ид", ", id"),
            new Sentence("@список_ид", "@список_ид , id"),
            new Sentence("@список_меток", "lbl"),
            new Sentence("@список_меток", "@список_меток , lbl"),
            new Sentence("@список_операторов", "@оператор ;"),
            new Sentence("@список_операторов", "@список_операторов @оператор ;"),
            new Sentence("@оператор", "id = @выражение1"),
            new Sentence("@оператор", "read ( @список_ид1 )"),
            new Sentence("@оператор", "write ( @список_ид1 )"),
            new Sentence("@оператор", "lbl :"),
            new Sentence("@оператор", "if ( @отношение ) then goto lbl"),
            new Sentence("@лог_выражение1", "@лог_выражение"),
            new Sentence("@оператор", "for id = @выражение1 by @выражение1 while @лог_выражение1 do @оператор ]"),
            new Sentence("@выражение1", "@выражение"),
            new Sentence("@выражение", "@терм1"),
            new Sentence("@выражение", "@выражение + @терм1"),
            new Sentence("@терм1", "@терм"),
            new Sentence("@выражение", "@выражение - @терм1"),
            new Sentence("@выражение", "- @терм1"),
            new Sentence("@терм", "@множитель"),
            new Sentence("@терм", "@терм * @множитель"),
            new Sentence("@терм", "@терм / @множитель"),
            new Sentence("@множитель", "( @выражение2 )"),
            new Sentence("@выражение2", "@выражение1"),
            new Sentence("@множитель", "const"),
            new Sentence("@множитель", "id"),
            new Sentence("@лог_выражение", "@лог_терм1"),
            new Sentence("@лог_выражение", "@лог_выражение || @лог_терм1"),
            new Sentence("@лог_терм1", "@лог_терм"),
            new Sentence("@лог_терм", "@лог_множитель"),
            new Sentence("@лог_терм", "@лог_терм && @лог_множитель"),
            new Sentence("@лог_множитель", "[ @лог_выражение1 ]"),
            new Sentence("@лог_множитель", "not @лог_множитель"),
            new Sentence("@лог_множитель", "@отношение"),
            new Sentence("@отношение", "@выражение < @выражение1"),
            new Sentence("@отношение", "@выражение > @выражение1"),
            new Sentence("@отношение", "@выражение <= @выражение1"),
            new Sentence("@отношение", "@выражение >= @выражение1"),
            new Sentence("@отношение", "@выражение == @выражение1"),
            new Sentence("@отношение", "@выражение != @выражение1"),

        };

        private List<Symbol> symbols;

        private String[,] table;

        private String[,] ErrMsg { set; get; }

        private List<EqualPair> pairs;

        public PrecedenceTableBuilder()
        {
            ErrMsg = new String[1, 1];
            HashSet<String> symbols = new HashSet<String>();
            pairs = new List<EqualPair>();
            foreach (Sentence sentence in grammar)
            {
                foreach (String symbol in sentence.Tail)
                {
                    symbols.Add(symbol);
                }
            }
            this.symbols = new List<Symbol>();
            table = new String[symbols.Count + 2, symbols.Count + 2];
            String str = " ";
            for (int i = 0; i < (int)Math.Sqrt(table.Length); i++)
            {
                for (int j = 0; j < (int)Math.Sqrt(table.Length); j++)
                {
                    table[i, j] = str;
                }
            }
            str = "1\\2";
            table[0, 0] = str;
            int index = 1;
            foreach (String symbol in symbols)
            {
                this.symbols.Add(new Symbol(symbol, getFirstSet(symbol), getLastSet(symbol)));
                table[index, 0] = symbol;
                table[0, index] = index.ToString();
                index++;
            }
            String sharp = "#";
            str = "<";
            table[index, 0] = sharp;
            for (int i = 1; i < (int)Math.Sqrt(table.Length); i++)
            {
                table[index, i] = str;
            }
            table[0, index] = sharp;
            str = ">";
            for (int i = 1; i < (int)Math.Sqrt(table.Length); i++)
            {
                table[i, index] = str;
            }
            str = " ";
            table[index, index] = str;
        }


        public String[,] process()
        {
            if (!fillEquals())
            {
                return ErrMsg;
            }
            else
            {
                if (!fillRelations())
                {
                    return ErrMsg;
                }
                else
                    return table;
            }
        }

        public int getSymbolIndex(String symbol)
        {
            if (symbol.Equals("#"))
                return 61;
            int i = 1;
            foreach (Symbol symb in symbols)
            {
                if (symb.Name.Equals(symbol))
                    return i;
                i++;
            }
            return 0;
        }
        public String findAndChange(List<String> symbols)
        {
            bool foundFlag;
            foreach (Sentence sentence in grammar)
            {
                if (sentence.Tail.Length == symbols.Count)
                {
                    int i = 0;
                    foundFlag = true;
                    foreach (String symbol in sentence.Tail)
                    {
                        if (!symbol.Equals(symbols[i++]))
                        {
                            foundFlag = false;
                            break;
                        }
                    }
                    if (foundFlag)
                    {
                        return sentence.Head;
                    }
                }
            }
            return " ";
        }

        public List<Symbol> getSymbols()
        {
            return symbols;
        }
        private String showTable()
        {
            String result = "";
            result += String.Format("{0, -24}", table[0, 0]);
            for (int i = 0; i < (int)Math.Sqrt(table.Length); i++)
            {
                if (i != 0)
                {
                    result += String.Format("{0, 2}. ", i);
                    result += String.Format("{0, -20}", table[i, 0]);
                }
                for (int j = 1; j < (int)Math.Sqrt(table.Length); j++)
                {
                    result += "|";
                    result += String.Format("{0, 3}", table[i, j]);
                }
                result += "\r\n";
            }
            return result;
        }
        private HashSet<String> getFirstSet(String symbol)
        {
            HashSet<String> firstSet = new HashSet<String>();
            if (symbol[0] == '@')
            {
                foreach (Sentence sentence in grammar)
                {
                    if (sentence.Head == symbol)
                    {
                        if (sentence.Tail[0] != sentence.Head)
                        {
                            firstSet.Add(sentence.Tail[0]);
                            if (sentence.Tail[0][0] == '@')
                                firstSet.UnionWith(getFirstSet(sentence.Tail[0]));
                        }
                    }
                }
            }
            return firstSet;
        }
        private HashSet<String> getLastSet(String symbol)
        {
            HashSet<String> lastSet = new HashSet<String>();
            if (symbol[0] == '@')
            {
                foreach (Sentence sentence in grammar)
                {
                    if (sentence.Head == symbol)
                    {
                        if (sentence.Tail[sentence.Tail.Length - 1] != sentence.Head)
                        {
                            lastSet.Add(sentence.Tail[sentence.Tail.Length - 1]);
                            if (sentence.Tail[sentence.Tail.Length - 1][0] == '@')
                                lastSet.UnionWith(getLastSet(sentence.Tail[sentence.Tail.Length - 1]));
                        }
                    }
                }
            }
            return lastSet;
        }
        private bool fillEquals()
        {
            foreach (Sentence sentence in grammar)
            {
                for (int i = 0; i < sentence.Tail.Length - 1; i++)
                {
                    if (!writeSign(sentence.Tail[i], sentence.Tail[i + 1], "="))
                    {
                        return false;
                    }
                    if (sentence.Tail[i + 1][0] == '@' && !(sentence.Tail[i][0] == '@'))
                        pairs.Add(new EqualPair(sentence.Tail[i], sentence.Tail[i + 1], 1));
                    else
                        if (sentence.Tail[i][0] == '@' && sentence.Tail[i + 1][0] == '@')
                        pairs.Add(new EqualPair(sentence.Tail[i], sentence.Tail[i + 1], 3));
                    else
                            if (sentence.Tail[i][0] == '@' && !(sentence.Tail[i + 1][0] == '@'))
                        pairs.Add(new EqualPair(sentence.Tail[i], sentence.Tail[i + 1], 2));
                }
            }
            return true;

        }
        private bool fillRelations()
        {
            foreach (EqualPair pair in pairs)
            {
                HashSet<String> firstSet;
                HashSet<String> lastSet;
                switch (pair.Type)
                {
                    case 1:
                        firstSet = returnFirstSet(pair.Symbol2);
                        foreach (String symbol2 in firstSet)
                        {
                            if (!writeSign(pair.Symbol1, symbol2, "<"))
                            {
                                ErrMsg[0, 0] += "КОНФЛІКТ! НЕ МОЖНА ВCТАНОВИТИ ЗНАК МІЖ " + pair.Symbol1 + " ТА " + symbol2 + "\r\n";
                                return false;
                            }
                        }
                        break;
                    case 2:
                        lastSet = returnLastSet(pair.Symbol1);
                        if (pair.Symbol2[0] != '@')
                        {
                            foreach (String symbol1 in lastSet)
                            {
                                if (!writeSign(symbol1, pair.Symbol2, ">"))
                                {
                                    ErrMsg[0, 0] += "КОНФЛІКТ! НЕ МОЖНА ВCТАНОВИТИ ЗНАК МІЖ " + symbol1 + " ТА " + pair.Symbol2 + "\r\n";
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            firstSet = returnFirstSet(pair.Symbol2);
                            foreach (String symbol1 in lastSet)
                            {
                                foreach (String symbol2 in firstSet)
                                {
                                    if (!writeSign(symbol1, symbol2, ">"))
                                    {
                                        ErrMsg[0, 0] += "КОНФЛІКТ! НЕ МОЖНА ВCТАНОВИТИ ЗНАК МІЖ " + symbol1 + " ТА " + symbol2 + "\r\n";
                                        return false;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        lastSet = returnLastSet(pair.Symbol1);
                        firstSet = returnFirstSet(pair.Symbol2);
                        foreach (String symbol1 in lastSet)
                        {
                            foreach (String symbol2 in firstSet)
                            {
                                if (!writeSign(symbol1, symbol2, ">"))
                                {
                                    ErrMsg[0, 0] += "КОНФЛІКТ! НЕ МОЖНА ВТАНОВИТИ ЗНАК МІЖ " + symbol1 + " ТА " + symbol2 + "\r\n";
                                    return false;
                                }
                            }
                        }
                        break;
                }
            }
            return true;
        }
        private bool writeSign(String symbol1, String symbol2, String sign)
        {
            int index1 = 1;
            bool flag1 = false;
            int index2 = 1;
            bool flag2 = false;
            foreach (Symbol symbol in symbols)
            {
                if (symbol.Name == symbol1)
                    flag1 = true;
                if (symbol.Name == symbol2)
                    flag2 = true;
                if (!flag1)
                    index1++;
                if (!flag2)
                    index2++;
                if (flag1 && flag2)
                    break;
            }
            if (table[index1, index2] == " " || table[index1, index2] == sign)
            {
                table[index1, index2] = sign;
                return true;
            }
            else
            {
                return false;
            }
        }

        private HashSet<String> returnFirstSet(String name)
        {
            foreach (Symbol symbol in symbols)
            {
                if (symbol.Name.Equals(name))
                    return symbol.FirstPlusSet;
            }
            return null;
        }
        private HashSet<String> returnLastSet(String name)
        {
            foreach (Symbol symbol in symbols)
            {
                if (symbol.Name.Equals(name))
                    return symbol.LastPlusSet;
            }
            return null;
        }
    }
}
