using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class LexAnalyzator
    {
        private String[] terminalLexemes =  {"program", "float", "label", "read", "write",
                                        "if", "then", "goto", "for", "by", "while", "do", "not", "{",
                                        "}.", ";", ",", "=", "(", ")", ":", "+", "-", "-",
                                          "*", "/", "&&", "||", "[", "]", "<", "<=", ">", ">=", "==", "!="};
        private char[] separators = { '{', ';', ',', '(', ')', ':', '*', '/', '[', ']' };

        private String lex;
        private int rowNumber;
        private String classifiedSource;
        private String result;
        private Boolean varDeclaration;
        private Boolean lblDeclaration;

        private List<Token> tokens;
        private List<ID> ids;
        private List<Label> labels;
        private List<Const> cons;

        public Boolean HasErrors { get; set; }

        public List<Token> getTokens()
        {
            return tokens;
        }

        public List<ID> getIDs()
        {
            return ids;
        }

        public string process(string source)
        {
            HasErrors = false;
            source += "  ";
            lex = "";
            rowNumber = 1;
            classifiedSource = "";
            result = "";
            int state = 1;
            tokens = new List<Token>();
            ids = new List<ID>();
            labels = new List<Label>();
            cons = new List<Const>();
            varDeclaration = false;
            lblDeclaration = false;
            int i = 0;
            char ch = source[i++];
            while (i < source.Length)
            {
                switch (state)
                {
                    case 1:
                        if ((ch == ' ') || (ch == '\r') || (ch == '\t'))
                        {
                            classifiedSource += ch;
                            ch = source[i++];
                            state = 1;
                        }
                        else
                        if (ch == '\n')
                        {
                            classifiedSource += ch;
                            ch = source[i++];
                            state = 1;
                            rowNumber++;
                        }
                        else
                        if (Char.IsLetter(ch))
                        {
                            if ((ch == 'E') || (ch == 'e'))
                                classifiedSource += 'E';
                            else
                            {
                                classifiedSource += 'Б';
                            }
                            lex += ch;
                            ch = source[i++];
                            state = 2;
                        }
                        else
                            if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 3;
                        }
                        else
                            if (ch == '.')
                        {
                            classifiedSource += '.';
                            lex += ch;
                            ch = source[i++];
                            state = 4;
                        }
                        else
                            if ((ch == '{') || (ch == ';') || (ch == ',') || (ch == '(') || (ch == ')')
                                || (ch == ':') || (ch == '*') || (ch == '/') || (ch == '[') || (ch == ']'))
                        {
                            classifiedSource += "ОР";
                            lex += ch;
                            tokens.Add(new Token(rowNumber, lex, getTRNIndex(lex)));
                            lex = "";
                            if ((varDeclaration) && (ch == ';'))
                                varDeclaration = false;
                            if ((lblDeclaration) && (ch == ';'))
                                lblDeclaration = false;
                            if (ch == ':')
                                if (tokens[tokens.Count - 2].Index == 39) {
                                    string lblName = tokens[tokens.Count - 2].Value;
                                    foreach (Label lb in labels)
                                    {
                                        if (lb.Name == lblName)
                                        {
                                            if (!lb.Used)
                                                lb.Used = true;
                                            else
                                            {
                                                classifiedSource = "";
                                                result += "СПРОБА ВСТАНОВИТИ МІТКУ В БІЛЬШ НІЖ ОДНОМУ МІСЦІ: " + lb.Name + " В РЯДКУ №" + rowNumber + "\n";
                                                state = 0;
                                            }
                                        }
                                    }
                        }
                            ch = source[i++];
                            if(state != 0)
                                state = 1;
                        }
                        else
                            if (ch == '+')
                        {
                            classifiedSource += "З";
                            tokens.Add(new Token(rowNumber, "+", getTRNIndex("+")));
                            ch = source[i++];
                            state = 1;
                        }
                        else
                        if (ch == '-')
                        {
                            classifiedSource += "З";
                            if((tokens[tokens.Count - 1].Index == 37) || (tokens[tokens.Count - 1].Index == 38))
                            {
                                tokens.Add(new Token(rowNumber, "-", 24));
                            }
                            else
                                tokens.Add(new Token(rowNumber, "-", 23));
                            ch = source[i++];
                            state = 1;
                        }
                        else
                        if (ch == '}')
                        {
                            classifiedSource += '}';
                            lex += ch;
                            ch = source[i++];
                            state = 9;
                        }
                        else
                            if (ch == '<')
                        {
                            classifiedSource += '<';
                            lex += ch;
                            ch = source[i++];
                            state = 10;
                        }
                        else
                            if (ch == '>')
                        {
                            classifiedSource += '>';
                            lex += ch;
                            ch = source[i++];
                            state = 11;
                        }
                        else
                            if (ch == '!')
                        {
                            classifiedSource += '!';
                            lex += ch;
                            ch = source[i++];
                            state = 12;
                        }
                        else
                            if (ch == '=')
                        {
                            classifiedSource += '=';
                            lex += ch;
                            ch = source[i++];
                            state = 13;
                        }
                        else
                        if (ch == '&')
                        {
                            classifiedSource += '&';
                            lex += ch;
                            ch = source[i++];
                            state = 14;
                        }
                        else
                        if (ch == '|')
                        {
                            classifiedSource += '|';
                            lex += ch;
                            ch = source[i++];
                            state = 15;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                    case 2:
                        if (Char.IsLetter(ch))
                        {
                            if ((ch == 'E') || (ch == 'e'))
                                classifiedSource += 'E';
                            else
                            {
                                classifiedSource += 'Б';
                            }
                            lex += ch;
                            ch = source[i++];
                            state = 2;
                        }
                        else
                            if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 2;
                        }
                        else
                        {
                            int index = getTRNIndex(lex);
                            if (getTRNIndex(lex) != 0)
                            {
                                tokens.Add(new Token(rowNumber, lex, index));
                                if (lex == "float")
                                    varDeclaration = true;
                                if (lex == "label")
                                    lblDeclaration = true;
                            }
                            else
                            {
                                if (ids.Count == 0)
                                {
                                    ids.Add(new ID(1, lex));
                                    tokens.Add(new Token(rowNumber, lex, 37, 1));
                                } else
                                if(varDeclaration)
                                {
                                    if (getIDIndex(lex) != 0)
                                    {
                                        classifiedSource = "";
                                        result += "ПОВТОРНЕ ОГОЛОШЕННЯ ЗМІННОЇ: " + lex + " В РЯДКУ №" + rowNumber + "\n";
                                        state = 0;
                                    }
                                    else
                                    {
                                        ids.Add(new ID(ids.Count + 1, lex));
                                        tokens.Add(new Token(rowNumber, lex, 37, ids.Count));
                                    }
                                } else
                                if (lblDeclaration)
                                {
                                    if ((getIDIndex(lex) != 0) || (getLblIndex(lex) != 0))
                                    {
                                        classifiedSource = "";
                                        result += "СПРОБА НАЗВАТИ МІТКУ ВЖЕ ВИКОРИСТАНИМ У ПРОГРАМІ ІМЕНЕМ: " + lex + " В РЯДКУ №" + rowNumber + "\n";
                                        state = 0;
                                    }
                                    else
                                    {
                                        labels.Add(new Label(labels.Count + 1, lex));
                                        tokens.Add(new Token(rowNumber, lex, 39, labels.Count));
                                    }
                                } else
                                {
                                    int idIndex = getIDIndex(lex);
                                    int lblIndex = getLblIndex(lex);
                                    if(idIndex != 0)
                                    {
                                        tokens.Add(new Token(rowNumber, lex, 37, idIndex));
                                    } else
                                    if(lblIndex != 0)
                                    {
                                        if (tokens[tokens.Count - 1].Value == "goto")
                                            labels[lblIndex - 1].Referenced = true;
                                        tokens.Add(new Token(rowNumber, lex, 39, lblIndex));
                                    } else
                                    {
                                        classifiedSource = "";
                                        result += "ВИКОРИСТАННЯ НЕОГОЛОШЕНОЇ ЗМІННОЇ АБО МІТКИ: " + lex + " В РЯДКУ №" + rowNumber + "\n";
                                        state = 0;
                                    }
                                }
                                
                            }
                            lex = "";
                            if(state != 0)
                                state = 1;
                        } 
                        break;
                    case 3:
                        if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 3;
                        }
                        else
                        if (ch == '.')
                        {
                            classifiedSource += '.';
                            lex += ch;
                            ch = source[i++];
                            state = 5;
                        }
                        else
                        if ((ch == 'E') || (ch == 'e'))
                        {
                            classifiedSource += 'E';
                            lex += ch;
                            ch = source[i++];
                            state = 6;
                        }
                        else
                        {
                            cons.Add(new Const(cons.Count + 1, lex));
                            tokens.Add(new Token(rowNumber, lex, 38, cons.Count));
                            lex = "";
                            state = 1;
                        }
                        break;
                    case 4:
                        if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 5;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                    case 5:
                        if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 5;
                        }
                        else
                        if ((ch == 'E') || (ch == 'e'))
                        {
                            classifiedSource += 'E';
                            lex += ch;
                            ch = source[i++];
                            state = 6;
                        }
                        else
                        {
                            cons.Add(new Const(cons.Count + 1, lex));
                            tokens.Add(new Token(rowNumber, lex, 38, cons.Count));
                            lex = "";
                            state = 1;
                        }
                        break;
                    case 6:
                        if ((ch == '+') || (ch == '-'))
                        {
                            classifiedSource += 'З';
                            lex += ch;
                            ch = source[i++];
                            state = 7;
                        }
                        else
                        if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 8;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                    case 7:
                        if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 8;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                    case 8:
                        if (Char.IsNumber(ch))
                        {
                            classifiedSource += 'Ц';
                            lex += ch;
                            ch = source[i++];
                            state = 8;
                        }
                        else
                        {
                            cons.Add(new Const(cons.Count + 1, lex));
                            tokens.Add(new Token(rowNumber, lex, 38, cons.Count));
                            lex = "";
                            state = 1;
                        }
                        break;
                    case 9:
                        if (ch == '.')
                        {
                            classifiedSource += '.';
                            lex += ch;
                            ch = source[i++];
                            tokens.Add(new Token(rowNumber, lex, getTRNIndex(lex)));                            
                            lex = "";
                            state = 1;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                    case 10:
                    case 11:
                    case 13:
                        if (ch == '=')
                        {
                            classifiedSource += '=';
                            lex += ch;
                            ch = source[i++];
                            tokens.Add(new Token(rowNumber, lex, getTRNIndex(lex)));
                            lex = "";
                            state = 1;
                        }
                        else
                        {
                            tokens.Add(new Token(rowNumber, lex, getTRNIndex(lex)));
                            lex = "";
                            state = 1;
                        }
                        break;
                    case 12:
                        if (ch == '=')
                        {
                            classifiedSource += '=';
                            lex += ch;
                            ch = source[i++];
                            tokens.Add(new Token(rowNumber, lex, getTRNIndex(lex)));
                            lex = "";
                            state = 1;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                    case 14:
                        if (ch == '&')
                        {
                            classifiedSource += '&';
                            lex += ch;
                            ch = source[i++];
                            tokens.Add(new Token(rowNumber, lex, getTRNIndex(lex)));
                            lex = "";
                            state = 1;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                    case 15:
                        if (ch == '|')
                        {
                            classifiedSource += '|';
                            lex += ch;
                            ch = source[i++];
                            tokens.Add(new Token(rowNumber, lex, getTRNIndex(lex)));
                            lex = "";
                            state = 1;
                        }
                        else
                        {
                            classifiedSource = "";
                            result += "НЕКОРЕКТНИЙ СИМВОЛ: " + lex + ch + " В РЯДКУ №" + rowNumber + "\n";
                            state = 0;
                        }
                        break;
                }
                if (state == 0)
                    break;
            }
            if (state != 0)
            {
                foreach(Label lb in labels)
                {
                    result += lb.Referenced;
                    result += lb.Used;
                    if((lb.Referenced)&&(!lb.Used))
                        return "ПОСИЛАННЯ НА НЕРОЗМІЩЕНУ МІТКУ: " + lb.Name + "\r\n";
                }
                result += "ВИХІДНА ТАБЛИЦЯ:\r\n";
                foreach (Token token in tokens)
                    result += token.Value + " " + token.Index + " " + token.ClassInnerIndex + "\r\n";
                result += "ТАБЛИЦЯ ЗМІННИХ:\r\n";
                foreach (ID id in ids)
                    result += (id.Index - 1) + " " + id.Value + "\r\n";
                result += "ТАБЛИЦЯ МІТОК:\r\n";
                foreach (Label lbl in labels)
                    result += lbl.Index + " " + lbl.Name + "\r\n";
                result += "ТАБЛИЦЯ КОНСТАНТ:\r\n";
                foreach (Const con in cons)                
                    result += con.Index + " " + con.Value + "\r\n";               
            }
            else
            {
                HasErrors = true;
            }
            return classifiedSource + "\r\n" + result + "\r\n";
        }

        private int getTRNIndex(String token)
        {
            int i = 0;
            foreach(string str in terminalLexemes)
            {
                i++;
                if (str == token)
                    return i;
            }
            return 0;
        }
        private int getIDIndex(String token)
        {
            int i = 0;
            foreach (ID id in ids)
            {
                i++;
                if (id.Value == token)
                    return i;
            }
            return 0;
        }
        private int getLblIndex(String token)
        {
            int i = 0;
            foreach (Label lbl in labels)
            {
                i++;
                if (lbl.Name == token)
                    return i;
            }
            return 0;
        }
    }
}