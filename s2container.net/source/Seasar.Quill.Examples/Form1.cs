using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Seasar.Quill.Examples
{
    public partial class Form1 : Form
    {
        protected ICulcLogic culcLogic = null;
        
        public Form1()
        {
                InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ret = culcLogic.Plus(1, 2);
            Console.WriteLine(ret);
        }

    }
}