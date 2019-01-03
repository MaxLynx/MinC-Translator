using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MC02Translator
{
    public partial class PrecedenceTableView : Form
    {
        String[,] table;
        PrecedenceTableBuilder builder;
        public PrecedenceTableView()
        {
            InitializeComponent();
            button2.Enabled = false;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            builder = new PrecedenceTableBuilder();
            table = builder.process();
            if (table.Length == 1)
                textBox1.Text = table[0, 0];
            else
            {
                button2.Enabled = true;
                textBox1.Text = "КОНФЛІКТІВ НЕМАЄ.\r\nТАБЛИЦЯ УСПІШНО ПОБУДОВАНА!";
                List<Symbol> symbols = builder.getSymbols();
                foreach (Symbol symbol in symbols)
                {
                    listBox1.Items.Add(symbol.Name);
                    listBox2.Items.Add(symbol.Name);
                    listBox1.SetSelected(0, true);
                    listBox2.SetSelected(0, true);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String firstSymbol = listBox1.SelectedItem.ToString();
            String secondSymbol = listBox2.SelectedItem.ToString();
            String sign = table[builder.getSymbolIndex(firstSymbol), builder.getSymbolIndex(secondSymbol)];
            if (sign.Equals(" "))
                sign = "Між цими символами відсутнє\r\nвідношення передування";
            textBox4.Text = sign;
        }
    }
}
