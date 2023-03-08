using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            string rootDirectory = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            string[] userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Split(Path.DirectorySeparatorChar);

            tbWorkingDirectory.Text = Path.Combine(rootDirectory, userDirectory[1]);

            tbCommands.Text = immutableString;
            tbCommands.Focus();
            tbCommands.SelectionStart = tbCommands.Text.Length;
        }

        private async void tbCommands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    int nextWordStart = tbCommands.Text.IndexOf(' ', tbCommands.SelectionStart);
                    int deleteStart = tbCommands.SelectionStart;
                    int deleteLength = nextWordStart >= 0 ? nextWordStart - deleteStart : tbCommands.Text.Length - deleteStart;
                    tbCommands.Text = tbCommands.Text.Remove(deleteStart, deleteLength);
                    tbCommands.SelectionStart = deleteStart;

                }
                else if (e.KeyCode == Keys.Back)
                {
                    int lastWordStart = tbCommands.Text.LastIndexOf(' ', tbCommands.SelectionStart - 1);
                    int deleteStart = lastWordStart >= 0 ? lastWordStart : 0;
                    int deleteLength = tbCommands.SelectionStart - deleteStart;
                    tbCommands.Text = tbCommands.Text.Remove(deleteStart, deleteLength);
                    tbCommands.SelectionStart = deleteStart;

                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Back:
                    e.SuppressKeyPress = tbCommands.SelectionStart <= immutableString.Length ? true : false;
                    break;
                case Keys.Delete:
                    e.SuppressKeyPress = tbCommands.SelectionStart < immutableString.Length ? true : false;
                    break;
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    string command = tbCommands.Text;
                    tbCommands.Text += Environment.NewLine + "> ";
                    tbCommands.SelectionStart = tbCommands.Text.Length;
                    tbCommands.ScrollToCaret();
                    immutableString = tbCommands.Text;
                    int index = command.IndexOf(immutableString);
                    if (index != -1)
                        command = command.Remove(index, immutableString.Length).Replace("\r\n",string.Empty).TrimStart();
        
                    await StartProcessAsync(command);
                    break;
                default:
                    break;
            }

        }

        private async Task StartProcessAsync(string command)
        {
            await Task.Run(() =>
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"cmd.exe",
                        Arguments = $"/c {command}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = tbWorkingDirectory.Text
                    }
                };

                proc.Start();

                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();
                tbOutput.Invoke(new Action(() =>
                {
                    tbOutput.Text = output;
                }));
            });
        }



        private void tbCommands_TextChanged(object sender, EventArgs e)
        {
    
            if (tbCommands.Text == "")
                tbCommands.Text = immutableString;
        }

        private void tbCommands_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\u007f')
                e.KeyChar = '\b';
        }
    }
}
