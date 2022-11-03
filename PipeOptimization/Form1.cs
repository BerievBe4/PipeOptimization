using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipeOptimization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = 5000.ToString();
            textBox2.Text = 0.01d.ToString();
            textBox3.Text = 100d.ToString();
            textBox4.Text = 0.025d.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var steps = Double.Parse(textBox1.Text);

            var left_border = Double.Parse(textBox2.Text);
            var right_border = Double.Parse(textBox3.Text);
            var threshold = Double.Parse(textBox4.Text);

            var task = new TaskDefinition();

            task.solve_task(steps, left_border, right_border, threshold, func01);
        }
    }
}
