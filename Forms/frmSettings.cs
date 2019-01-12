using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Net;

namespace PTFLauncher
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        public static string setPath;
        public static string realmlistPath;
        public static string username;
        public static string password;
        public static string s_show_pw          = "false";
        public static string s_update_checks    = "false";
        public static string s_playandclose     = "false";
        public static string s_autologin        = "false";
        public static string s_autoclear        = "true";


        private void btnChoose_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fullPath = ofd.FileName;
                string fileName = ofd.SafeFileName;
                string path = fullPath.Replace(fileName, "");
                txtPath.Text = fullPath;
                setPath = fullPath;
                realmlistPath = path + "\\realmlist.wtf";
            }
            var ini = new ini("settings.ini");
            ini.Write("path", setPath);
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            if(classVars.b_debug)
            {
                
            }
            else
            {
               
            }
            
            this.TopMost = true;

            if(File.Exists(Application.StartupPath + "\\settings.ini"))
            {
                try
                { 
                    var ini                     = new ini("settings.ini");
                    string path                 = ini.Read("path");
                    string user                 = ini.Read("user");
                    string pass                 = ini.Read("pass");
                    string show_pw              = ini.Read("show_pw");
                    string autologin            = ini.Read("autologin");
                    string playandclose         = ini.Read("playandclose");
                    string update_checks        = ini.Read("update_checks");
                    string rl                   = path.Replace("Wow.exe", "realmlist.wtf");
                    string realmlistPath        = rl;
                    string autoclear            = ini.Read("autoclear");
                    string clientversion        = ini.Read("clVersion");
                    classVars.cl_version        = clientversion;
                    comboBox1.Text              = classVars.cl_version;
                    setPath                     = path;
                    s_show_pw                   = show_pw;
                    s_autologin                 = autologin;
                    s_update_checks             = update_checks;
                    s_playandclose              = playandclose;
                    s_autoclear                 = autoclear;
                    string input                = pass;
                    string decoded              = Crypto.Atom128.Decode(pass);
                    pass                        = decoded;

                    txtPath.Text                = path;
                    textBox1.Text               = user;
                    textBox2.Text               = pass;
                    

                    if (s_show_pw == "true")
                    {
                        checkBox1.Checked = true;
                    }
                    else if(s_show_pw == "false")
                    {
                        checkBox1.Checked = false;
                    }
                    if (s_update_checks == "true")
                    {
                        checkBox2.Checked = true;
                    }
                    else if (s_update_checks == "false")
                    {
                        checkBox2.Checked = false;
                    }
                    if (s_playandclose == "true")
                    {
                        checkBox3.Checked = true;
                    }
                    else if (s_playandclose == "false")
                    {
                        checkBox3.Checked = false;
                    }
                    if (s_autologin == "true")
                    {
                        checkBox4.Checked = true;
                    }
                    else if (s_autologin == "false")
                    {
                        checkBox4.Checked = false;
                    }
                    if(s_autoclear == "true")
                    {
                        checkBox5.Checked = true;
                    }
                    else if(s_autoclear == "false")
                    {
                        checkBox5.Checked = false;
                    }
                }
                catch { MessageBox.Show("Settings file found but empty"); }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                s_show_pw = "true";
            }
            else
            {
                s_show_pw = "false";
            }
            if (checkBox4.Checked == true)
            {
                s_autologin = "true";
            }
            else
            {
                s_autologin = "false";
            }
            if (checkBox5.Checked == true)
            {
                s_autoclear = "true";
            }
            else
            {
                s_autoclear = "false";
            }

            string input            = textBox2.Text;
            string encoded          = Crypto.Atom128.Encode(input);
            var ini = new ini("settings.ini");
            classVars.cl_version = comboBox1.Text;
            ini.Write("path", setPath);
            ini.Write("user", textBox1.Text);
            ini.Write("pass", encoded);
            ini.Write("show_pw", s_show_pw);
            ini.Write("update_checks", s_update_checks);
            ini.Write("playandclose", s_playandclose);
            ini.Write("autologin", s_autologin);
            ini.Write("autoclear", s_autoclear);
            ini.Write("clVersion", classVars.cl_version);

            MessageBox.Show("Settings saved!");
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = char.Parse("\0");
            else
                textBox2.PasswordChar = char.Parse("*");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                s_update_checks = "true";
            else
                s_update_checks = "false";
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                s_playandclose = "true";
            else
                s_playandclose = "false";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\settings.ini");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\settings.ini");
        
    }
    }
}
