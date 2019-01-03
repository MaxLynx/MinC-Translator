using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC02Translator
{
    class AutomateRow
    {
        //ПОЧАТКОВИЙ СТАН АВТОМАТУ
        public String FirstState 
        {
            set; get;
        }
        //ЛЕКСЕМА НА ВХОДІ
        public String Lexem 
        {
            set; get;
        }
        //КІНЦЕВИЙ СТАН АВТОМАТУ
        public String SecondState 
        {
            set; get;
        }
        //СТАН, ЯКИЙ ЗАПИСУЄТЬСЯ ДО СТЕКУ
        public String StackWrite 
        {
            set; get;
        }
        //ПОМИЛКА ПРИ НЕПОРІВНЯННІ
        public Boolean Error 
        {
            set; get;
        }
        //ВИХІД З АВТОМАТУ
        public Boolean Exit 
        {
            set; get;
        }
        //ПЕРЕХІД ДО НАСТУПНОЇ ЛЕКСЕМИ
        public Boolean Inc 
        {
            set; get;
        }
        public AutomateRow(String firstState, String lexem, String secondState, String stackWrite, Boolean error, Boolean exit, Boolean inc)
        {
            FirstState = firstState;
            Lexem = lexem;
            SecondState = secondState;
            StackWrite = stackWrite;
            Error = error;
            Exit = exit;
            Inc = inc;
        }

        public String getInfo()
        {
            return FirstState + " " + Lexem + " " + SecondState + " " + StackWrite + " " + Error + " " + Exit + " " + Inc + "\r\n";
        }
    }
}
