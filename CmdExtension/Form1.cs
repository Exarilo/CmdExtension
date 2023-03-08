using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CmdExtension
{
    public partial class Form1 : Form
    {
        private string immutableString = "> ";

        public Form1()
        {
            InitializeComponent();
            tbCommands.ScrollBars = ScrollBars.Vertical;
            toolStripCbLangage.SelectedIndex = 0;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbCommands.Focus();
        }

        private void tbCommands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && tbCommands.SelectionStart <= immutableString.Length)
            {
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                string command = tbCommands.Text;
                tbCommands.Text = "> ";
            }
        }

        private void tbCommands_KeyPress(object sender, KeyPressEventArgs e)
        {


        }
    }
}
