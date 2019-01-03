using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class SA2
    {
        //ТАБЛИЦЯ ПЕРЕХОДІВ
        public AutomateRow[] automateTable =
        {
            //ПРОГРАМА
            new AutomateRow("1", "program", "2", null, true, false, true),
            new AutomateRow("2", "id", "3", null, true, false, true),
            new AutomateRow("3", "float", "4", null, true, false, true),
            new AutomateRow("4", "id", "5", null, true, false, true),
            new AutomateRow("5", ",", "4", null, false, false, true),
            new AutomateRow("5", ";", "6", null, true, false, true),
            new AutomateRow("6", "label", "7", null, false, false, true),
            new AutomateRow("6", "{", "2-1", "10", true, false, true),
            new AutomateRow("7", "lbl", "8", null, true, false, true),
            new AutomateRow("8", ",", "7", null, false, false, true),
            new AutomateRow("8", ";", "9", null, true, false, true),
            new AutomateRow("9", "{", "2-1", "10", true, false, true),
            new AutomateRow("10", ";", "11", null, true, false, true),
            new AutomateRow("11", "}.", null, null, false, true, false),
            new AutomateRow("11", null, "2-1", "10", false, false, false),
            //ОПЕРАТОР
            new AutomateRow("2-1", "id", "2-2", null, false, false, true),
            new AutomateRow("2-1", "read", "2-4", null, false, false, true),
            new AutomateRow("2-1", "write", "2-4", null, false, false, true),
            new AutomateRow("2-1", "lbl", "2-7", null, false, false, true),
            new AutomateRow("2-1", "if", "2-8", null, false, false, true),
            new AutomateRow("2-1", "for", "2-14", null, true, false, true),
            new AutomateRow("2-2", "=", "3-1", "2-3", true, false, true),
            new AutomateRow("2-3", null, null, null, false, true, false),
            new AutomateRow("2-4", "(", "2-5", null, true, false, true),
            new AutomateRow("2-5", "id", "2-6", null, true, false, true),
            new AutomateRow("2-6", ",", "2-5", null, false, false, true),
            new AutomateRow("2-6", ")", null, null, true, true, true),
            new AutomateRow("2-7", ":", null, null, true, true, true),
            new AutomateRow("2-8", "(", "3-1", "2-9", true, false, true),
            new AutomateRow("2-9", "<", "3-1", "2-10", false, false, true),
            new AutomateRow("2-9", "<=", "3-1", "2-10", false, false, true),
            new AutomateRow("2-9", ">", "3-1", "2-10", false, false, true),
            new AutomateRow("2-9", ">=", "3-1", "2-10", false, false, true),
            new AutomateRow("2-9", "==", "3-1", "2-10", false, false, true),
            new AutomateRow("2-9", "!=", "3-1", "2-10", false, false, true),
            new AutomateRow("2-9", null, null, null, true, false, false),
            new AutomateRow("2-10", ")", "2-11", null, true, false, true),
            new AutomateRow("2-11", "then", "2-12", null, true, false, true),
            new AutomateRow("2-12", "goto", "2-13", null, true, false, true),
            new AutomateRow("2-13", "lbl", null, null, true, true, true),
            new AutomateRow("2-14", "id", "2-15", null, true, false, true),
            new AutomateRow("2-15", "=", "3-1", "2-16", true, false, true),
            new AutomateRow("2-16", "by", "3-1", "2-17", true, false, true),
            new AutomateRow("2-17", "while", "6-1", "2-18", true, false, true),
            new AutomateRow("2-18", "do", "2-1", "2-19", true, false, true),
            new AutomateRow("2-19", null, null, null, false, true, false),
            //ВИРАЗ
            new AutomateRow("3-1", "-", "3-2", null, false, false, true),
            new AutomateRow("3-1", "(", "3-1", "3-3", false, false, true),
            new AutomateRow("3-1", "const", "3-4", null, false, false, true),
            new AutomateRow("3-1", "id", "3-4", null, true, false, true),
            new AutomateRow("3-2", "(", "3-1", "3-3", false, false, true),            
            new AutomateRow("3-2", "const", "3-4", null, false, false, true),
            new AutomateRow("3-2", "id", "3-4", null, true, false, true),
            new AutomateRow("3-3", ")", "3-4", null, true, false, true),
            new AutomateRow("3-4", "*", "3-1", null, false, false, true),
            new AutomateRow("3-4", "/", "3-1", null, false, false, true),
            new AutomateRow("3-4", "+", "3-1", null, false, false, true),
            new AutomateRow("3-4", "-", "3-1", null, false, false, true),
            new AutomateRow("3-4", null, null, null, false, true, false),
            //ЛОГІЧНИЙ ВИРАЗ
            new AutomateRow("6-1", "not", "6-1", null, false, false, true),
            new AutomateRow("6-1", "[", "6-1", "6-2", false, false, true),
            new AutomateRow("6-1", null, "3-1", "6-3", false, false, false),
            new AutomateRow("6-2", "]", "6-4", null, true, false, true),
            new AutomateRow("6-3", ">", "3-1", "6-4", false, false, true),
            new AutomateRow("6-3", "<", "3-1", "6-4", false, false, true),
            new AutomateRow("6-3", ">=", "3-1", "6-4", false, false, true),
            new AutomateRow("6-3", "<=", "3-1", "6-4", false, false, true),
            new AutomateRow("6-3", "==", "3-1", "6-4", false, false, true),
            new AutomateRow("6-3", "!=", "3-1", "6-4", true, false, true),
            new AutomateRow("6-4", "&&", "6-1", null, false, false, true),
            new AutomateRow("6-4", "||", "6-1", null, false, false, true),
            new AutomateRow("6-4", null, null, null, false, true, false)
        };

        private List<Token> lexemes; 
        private String Result { set; get; }
        private int currentRow;
        private int i;
        private Stack<String> stack; 
        private String debug; 

        public SA2()
        {
            stack = new Stack<String>();
            Result = "";
            currentRow = 1;
            i = 0;
        }

        public String process(List<Token> lexemes)
        {
            //Точка входа
            String firstState = "1";
            stack = new Stack<String>();
            this.lexemes = lexemes;
            Result = "";
            debug = "";
            currentRow = 1;
            i = 0;
            do
            {
                currentRow = lexemes[i].Row;
                Result = changeState(firstState, lexemes[i].GeneralizedValue);
                if (Result[0] == 'П')
                {
                    if (i + 1 == lexemes.Count)
                        return "ПРОГРАМА МАЄ КОРЕКТНИЙ СИНТАКСИС!\r\n";
                    else
                    {
                        return "Є КОД ПІСЛЯ ЗАКІНЧЕННЯ ПРОГРАМИ\r\n";
                    }
                }
                if (Result[0] == 'Н' || Result[0] == 'С')
                {
                    return Result;
                }
                firstState = Result;
                debug += Result + " " + lexemes[i].GeneralizedValue + "\r\n";
            } while (true);
        }

        private String changeState(String state, String lex)
        {
            debug += lex + ": ";
            foreach (AutomateRow row in automateTable)
            {
                if (row.FirstState.Equals(state))
                {
                    debug += row.getInfo();
                    if (row.Lexem == null || row.Lexem.Equals(lex))
                    {
                        if (row.StackWrite != null)
                            stack.Push(row.StackWrite);
                        if (row.Inc)
                        {
                            i++;
                            if (i >= lexemes.Count)
                                return "НЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ В РЯДКУ " + currentRow + "\r\n";
                        }
                        if (row.Exit)
                        {
                            if (stack.Count > 0)
                            {
                                return stack.Pop();
                            }
                            else
                                return "ПОРОЖНІЙ СТЕК";
                        }
                        if (row.Lexem == null && row.Error)
                            return "СИНТАКСИЧНА ПОМИЛКА В РЯДКУ " + currentRow + "\r\n";
                        return row.SecondState;
                    }
                    else
                    {
                        if (row.Error)
                            return "СИНТАКСИЧНА ПОМИЛКА В РЯДКУ " + currentRow + "\r\n";
                    }

                }
            }
            return null;
        }
    }
}
