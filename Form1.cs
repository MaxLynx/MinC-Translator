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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LexAnalyzator la = new LexAnalyzator();
            String laResult = la.process(textBox1.Text);
            if (la.HasErrors)
                textBox2.Text = laResult;
            else
            {
                textBox3.Text = laResult;
                SA3 sa = new SA3();
                
                MessageBox.Show(sa.process(la.getTokens()));
                if (!sa.Errors)
                {
                    la.process(textBox1.Text);
                    IntermediateCodeGenerator icg = new IntermediateCodeGenerator(la.getTokens());
                    icg.process();
                    dataGridView1.ColumnCount = 3;
                    dataGridView1.Columns[0].Width = dataGridView1.Columns[0].Width + 650;
                    dataGridView1.Columns[1].Width = dataGridView1.Columns[1].Width + 100;
                    dataGridView1.Columns[0].Name = "ПОЛИЗ";
                    dataGridView1.Columns[1].Name = "СТЕК";
                    dataGridView1.Columns[2].Name = "ВХОД";
                    foreach (ICGInfo info in icg.Information)
                    {
                        String[] row = new String[3];
                        row[0] = info.Result;
                        row[1] = info.Stack;
                        row[2] = info.Input;
                        dataGridView1.Rows.Add(row);
                    }
                    textBox2.Text = icg.Information[icg.Information.Count - 1].Result;
                    Executor exec = new Executor(icg.Result, la.getIDs().Cast<ID>()
                                    .ToList());
                    while (!exec.process())
                    {
                        exec.ReadVarValue = Double.Parse(
                            Microsoft.VisualBasic.Interaction.InputBox("Будь ласка, введіть значення " + exec.ReadVar + ":", "Введіть значення", "0"));
                    }
                    MessageBox.Show("ПРОГРАМА УСПІШНО ВИКОНАНА\r\n" + exec.Result);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "firstProg program\r\n" +
    "float ,x ,y ,z;\r\n" +
            "label lbl1;\r\n" +
            "{\r\n" +
                "read(,x);\r\n" +
                "z = 0.1;\r\n" +
                "for y = 0 by 2 while y < 10 do z = z + x ];\r\n" +
    "if (z >= 5) then goto lbl1;\r\n" +
                "z = z + 5E-4;\r\n" +
                "lbl1:;\r\n" +
                "write(,x ,y ,z);\r\n" +
            "}.\r\n";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new PrecedenceTableView().Show();
        }
    }
}
