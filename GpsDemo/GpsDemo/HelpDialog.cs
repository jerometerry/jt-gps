using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JeromeTerry.GpsDemo
{
    public partial class HelpDialog : Form
    {
        public HelpDialog()
        {
            InitializeComponent();
        }

        private void HelpDialog_Load(object sender, EventArgs e)
        {
            string dir = Application.StartupPath;
            string file = System.IO.Path.Combine(dir, "help.rtf");
            string help = System.IO.File.ReadAllText(file);
            this._helpText.Rtf = help;
        }
    }
}
