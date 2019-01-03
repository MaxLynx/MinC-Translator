using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class SA1
    {
        private List<Token> lexemes;
        private String Result { set; get; }
        private int currentRow;
        private int i;

        public SA1()
        {
            Result = "";
            currentRow = 1;
            i = 0;
        }

        public String process(List<Token> lexemes)
        {
            this.lexemes = lexemes;
            Result = "";
            currentRow = 1;
            i = 0;
            _program_();
            return Result;
        }

        private bool _program_()
        {
            try
            {
                if ((check("program")) && (check("id")) && check("float") && check("id") && checkIDList()
                    && check(";"))
                {
                    if (check("label"))
                    {
                        if(!(check("lbl") && checkLblList() && check(";")))
                        {
                            Result += "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nПРОГРАМА МАЄ НЕПРАВИЛЬНИЙ СИНТАКСИС\r\n";
                            return false;
                        }
                    }
                    
                    if(check("{") && _operator_() && check(";") && checkOperatorList() && check("}."))
                    {
                        while ((i < lexemes.Count) && ((lexemes[i]).GeneralizedValue.Equals("NL")))
                        {
                            i++;
                        }
                        if (i < lexemes.Count)
                        {
                            Result = "ВИДАЛІТЬ КОД ПІСЛЯ КІНЦЯ ПРОГРАМИ\r\n";
                            return false;
                        }
                        else
                        {
                            Result = "ПРОГРАМА МАЄ КОРЕКТНИЙ СИНТАКСИС!\r\n";
                            return true;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            Result += "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nПРОГРАМА МАЄ НЕПРАВИЛЬНИЙ СИНТАКСИС\r\n";
            return false;
        }
        private bool _operator_()
        {
            try
            {
                int saveID = i;
                if (check("id") && check("=") && _expression_())
                {
                    return true;
                }
                else
                {
                    i = saveID;
                    if (check("read") && check("(") && check("id") && checkIDList() && check(")"))
                        return true;
                    else
                    {
                        i = saveID;
                        if (check("write") && check("(") && check("id") && checkIDList() && check(")"))
                            return true;
                        else
                        {
                            i = saveID;
                            if (check("for") && check("id") && check("=") && _expression_() && check("by")
                                && _expression_() && check("while") && _logicalExpression_() && check("do")
                                    && _operator_())
                                return true;
                            else
                            {
                                i = saveID;
                                if (check("if") && check("(") && _expression_() && _relationSign_() && _expression_()
                                    && check(")") && check("then") && check("goto")
                                    && check("lbl"))
                                {
                                    return true;
                                }

                                else
                                {
                                    i = saveID;
                                    if (check("lbl") && check(":"))
                                    {
                                        return true;
                                    }
                                }

                            }
                        }

                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            return false;
        }
        private bool _expression_()
        {
            try
            {
                check("-");
                if (_term_())
                {
                    int savedI = i;
                    while (check("+") || (check("-")))
                    {
                        if (!_term_())
                            return false;
                        else
                        {
                            savedI = i;
                        }
                    }
                    i = savedI;
                    return true;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            return false;
        }
        private bool _term_()
        {
            try
            {
                if (_multiplier_())
                {
                    int savedI = i;
                    while (check("*") || (check("/")))
                    {
                        if (!_multiplier_())
                            return false;
                        else
                        {
                            savedI = i;
                        }
                    }
                    i = savedI;
                    return true;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            return false;
        }

        private bool _multiplier_()
        {
            try
            {
                if (check("const") || check("id"))
                {
                    return true;
                }
                else
                {
                    if (check("(") && _expression_() && check(")"))
                        return true;
                }

            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            Result += "ТУТ ПОВИННА БУТИ ЗМІННА, КОНСТАНТА АБО ВИРАЗ У ДУЖКАХ\r\n";
            return false;
        }
        private bool _logicalExpression_()
        {
            try
            {
                if (_logicalTerm_())
                {
                    int savedI = i;
                    while (check("||"))
                    {
                        if (!_logicalTerm_())
                            return false;
                        else
                        {
                            savedI = i;
                        }
                    }
                    i = savedI;
                    return true;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            Result += "НЕПРАВИЛЬНИЙ ЛОГІЧНИЙ ВИРАЗ\r\n";
            return false;
        }
        private bool _logicalTerm_()
        {
            try
            {
                if (_logicalMultiplier_())
                {
                    int savedI = i;
                    while (check("&&"))
                    {
                        if (!_logicalMultiplier_())
                            return false;
                        else
                        {
                            savedI = i;
                        }
                    }
                    i = savedI;
                    return true;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            Result += "НЕПРАВИЛЬНИЙ ЛОГІЧНИЙ ТЕРМ\r\n";
            return false;
        }
        private bool _logicalMultiplier_()
        {
            try
            {

                int savedI = i;
                while (check("not"))
                {
                    savedI = i;
                }
                i = savedI;
                if (_expression_() && _relationSign_() && _expression_())
                {
                    return true;
                }
                i = savedI;
                if (check("[") && _logicalExpression_() && check("]"))
                    return true;


            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            return false;
        }
        private bool _relationSign_()
        {
            try
            {
                if (check("<") || check(">") || check("==")
                        || check("!=") || check("<=") || check(">="))
                    return true;

            }
            catch (ArgumentOutOfRangeException e)
            {
                Result = "ПОМИЛКА В РЯДКУ №" + currentRow + "\r\nНЕСПОДІВАНИЙ КІНЕЦЬ ПРОГРАМИ\r\n";
                return false;
            }
            Result += "НЕКОРЕКТНИЙ ЗНАК ВІДНОШЕННЯ\r\n";
            return false;
        }
        private bool checkIDList()
        {
            int saved = i;
            while (next().Equals(","))
            {
                i++;
                if (next().Equals("id"))
                {
                    saved = ++i;
                }
                else
                    return false;
            }
            i = saved;
            return true;
        }
        private bool checkLblList()
        {
            int saved = i;
            while (next().Equals(","))
            {
                i++;
                if (next().Equals("lbl"))
                {
                    saved = ++i;
                }
                else
                    return false;
            }
            i = saved;
            return true;
        }
        private bool checkOperatorList()
        {
            int saved = i;
            while (_operator_())
            {
                if (next().Equals(";"))
                {
                    saved = ++i;
                }
                else
                    return false;
            }
            i = saved;
            return true;
        }
        private bool check(String str)
        {
            if (next().Equals(str))
            {
                i++;
                return true;
            }
            return false;
        }
        private String next()
        {
            Token lex = lexemes[i];
            currentRow = lex.Row;
            return lex.GeneralizedValue;
        }
    }
}
