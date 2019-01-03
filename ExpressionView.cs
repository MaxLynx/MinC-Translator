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
    public partial class ExpressionView : Form
    {
        SA3 sa;
        public ExpressionView()
        {
            InitializeComponent();
            textBox1.Text = "b*(-a*a+1)/2";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            LexAnalyzator la = new LexAnalyzator();
            la.process(textBox1.Text);
            if (!la.HasErrors)
            {
                sa = new SA3();
                textBox2.Text = sa.process(la.getTokens());
                if (!sa.Errors)
                {
                    HashSet<String> ids = new HashSet<String>();
                    foreach (PolishNotationElement el in sa.PolishNotation)
                    {
                        if (el.IsVariable)
                            ids.Add(el.Value);
                        textBox4.Text += el.Value + " ";
                    }
                    dataGridView1.ColumnCount = 2;
                    dataGridView1.Columns[0].Name = "ЗМІННА";
                    dataGridView1.Columns[1].Name = "ЗНАЧЕННЯ";
                    foreach (String id in ids)
                    {
                        String[] row = new String[5];
                        row[0] = id;
                        row[1] = "0";
                        dataGridView1.Rows.Add(row);
                    }
                    List<PolishNotationElement> toBeCalculated = new List<PolishNotationElement>();
                    foreach (PolishNotationElement el in sa.PolishNotation)
                    {
                        if (!el.IsVariable)
                            toBeCalculated.Add(el);
                        else
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.Cells[0].Value.ToString().Equals(el.Value))
                                {
                                    toBeCalculated.Add(new PolishNotationElement(row.Cells[1].Value.ToString(), false));
                                    break;
                                }
                            }
                        }
                    }
                    textBox3.Text = ExpressionCalculator.calculate(toBeCalculated);
                }
                dataGridView2.Rows.Clear();
                dataGridView2.ColumnCount = 5;
                dataGridView2.Columns[1].Width = dataGridView2.Columns[1].Width + 80;
                dataGridView2.Columns[3].Width = dataGridView2.Columns[3].Width + 200;
                dataGridView2.Columns[4].Width = dataGridView2.Columns[4].Width * 2;
                dataGridView2.Columns[0].Name = "СТЕК";
                dataGridView2.Columns[1].Name = "ЗНАК";
                dataGridView2.Columns[2].Name = "ВХІД";
                dataGridView2.Columns[3].Name = "ОСНОВА";
                dataGridView2.Columns[4].Name = "ПОЛІЗ";
                if (sa.Information != null)
                    foreach (AscentSAInfo info in sa.Information)
                    {
                        String[] row = new String[5];
                        row[0] = info.Stack;
                        row[1] = info.Sign;
                        row[2] = info.Input;
                        row[3] = info.Base;
                        row[4] = info.PolishNote;
                        dataGridView2.Rows.Add(row);
                    }
            }
            else
            {
                textBox2.Text = "ПОМИЛКА У ВИРАЗІ";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            List<PolishNotationElement> toBeCalculated = new List<PolishNotationElement>();
            foreach (PolishNotationElement el in sa.PolishNotation)
            {
                if (!el.IsVariable)
                    toBeCalculated.Add(el);
                else
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[0].Value.ToString().Equals(el.Value))
                        {
                            toBeCalculated.Add(new PolishNotationElement(row.Cells[1].Value.ToString(), false));
                            break;
                        }
                    }
                }
            }
            textBox3.Text = ExpressionCalculator.calculate(toBeCalculated);
        }

        private void ExpressionView_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
