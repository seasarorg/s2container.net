using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Seasar.Quill.Examples
{
    public partial class ExtendedForm : Form1
    {
        public ExtendedForm() : base()
        {
            InitializeComponent();
        }

        public bool ParentIsDesignMode
        {
            get { return base.DesignMode; }
        }
    }
}
