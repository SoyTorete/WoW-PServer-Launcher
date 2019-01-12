using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PTFLauncher
{
    public partial class frmLauncher : Form
    {
        public frmLauncher(string arg1, string arg2)
        {
            InitializeComponent();
        }
        
        [DllImport("user32.dll")]

        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        
        public void LoadSettings()
        {
            if (!File.Exists(Application.StartupPath + "\\settings.ini"))
            {
                var myFile = File.Create(Application.StartupPath + "\\settings.ini");
                myFile.Close();
                MessageBox.Show("Please setup your launcher now!");
                frmSettings sf = new frmSettings();
                sf.Show();
            }
            else if (File.Exists(Application.StartupPath + "\\settings.ini"))
            {
                string read;
                read = File.ReadAllText(Application.StartupPath + "\\settings.ini");
                if (read == null)
                    MessageBox.Show("Settings not ready yet");
            }
            
            var ini = new ini("settings.ini");
            string path = ini.Read("path");
            string user = ini.Read("user");
            string pass = ini.Read("pass");
            string show_pw = ini.Read("show_pw");
            string update_checks = ini.Read("update_checks");
            string playandclose = ini.Read("playandclose");
            string autologin = ini.Read("autologin");
            string decoded = Crypto.Atom128.Decode(pass);
            pass = decoded;
            string clientversion = ini.Read("clVersion");
            classVars.cl_version = clientversion;
            classVars.s_autologin = autologin;
            classVars.s_path = path;
            classVars.s_user = user;
            classVars.s_update_checks = update_checks;
            classVars.s_show_pw = show_pw;
            classVars.s_pass = pass;
            classVars.s_playandclose = playandclose;
        }

        public void updateServerStatus()
        {   
            WebClient wc = new WebClient();
            string result = wc.DownloadString(classVars.url_base + "wow/files/launcher/s_online.php");
            if (result == "online")
            {
                lblServerStatus.Text = "Worldserver is online";
                lblServerStatus.ForeColor = System.Drawing.Color.Green;
            }
            else if (result == "offline")
            {
                lblServerStatus.Text = "Worldserver is offline";
                lblServerStatus.ForeColor = System.Drawing.Color.Red;
            }

        }

        public void InitLauncher()
        {

            linkLabel1.Visible = false;
            this.TopMost = true;
            bool connection = classNetworking.checkConnection();
            

            try
            {
                LoadSettings();
            }
            catch
            {
               
            }
            this.Text += " " + classVars.appversion;

            if (classVars.b_debug == true)
            {
                dEBUGToolStripMenuItem.Visible = true;
            }

            if (connection)
            {
                try
                {
                    updateServerStatus();
                    lblVersion.Text = classVars.appversion;
                    bool b = classNetworking.updateAvailable();

                    if (b && classVars.s_update_checks == "true")
                    {
                        MessageBox.Show("we have a new update !");
                        linkLabel1.Visible = true;
                    }

                    WebClient newsstream = new WebClient();
                    txtNews.Text = newsstream.DownloadString(classVars.url_news);

                    updatePlayersOnline();

                    if (!File.Exists(Application.StartupPath + "\\settings.ini"))
                    {
                        var myFile = File.Create(Application.StartupPath + "\\settings.ini");
                        myFile.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("No connection to our server!");
                    Application.Exit();
                }
                if (!File.Exists(Application.StartupPath + "\\settings.ini"))
                {
                    frmSettings set = new frmSettings();
                    set.Show();
                    set.BringToFront();
                }
            }

        }

        private void frmLauncher_Load(object sender, EventArgs e)
        {
            try
            {
                InitLauncher();
                classVars.b_settingsneeded = false;
            }
            catch
            {
                classVars.b_settingsneeded = true;
                MessageBox.Show("Something happened while loading the launcher.. You may encounter bugs , please restart your launcher.");

            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (lblServerStatus.ForeColor == System.Drawing.Color.Green)
            {
                var ini = new ini("settings.ini");
                string path = ini.Read("path");
                string autoclean = ini.Read("autoclear");
                LoadSettings();

                try
                {
                    int i = Convert.ToInt32(classVars.cl_version.Replace(".", ""));
                    classPatcher.PatchRealmlist(i);
                }
                catch(Exception re)
                {
                    MessageBox.Show(re.ToString());
                }
                

                if (lblServerStatus.ForeColor == System.Drawing.Color.Red)
                {
                    MessageBox.Show("The worldserver seems to be offline , please check back later..");
                }

                if (path != null && autoclean == "true")
                {
                    try
                    {
                        Directory.Delete(path.Replace("Wow.exe", "") + "Cache", true);
                    }
                    catch
                    {

                    }
                }
                if (classVars.s_autologin == "false")
                {
                    Process.Start(classVars.s_path);
                }
                this.TopMost = false;
                if (!File.Exists(Application.StartupPath + "\\settings.ini"))
                {
                    MessageBox.Show("Cannot launch game , update your settings please.");
                }
                else if (classVars.s_autologin == "true")
                {
                    Process proc = Process.Start(classVars.s_path);

                    while (!proc.WaitForInputIdle())
                    {
                        System.Threading.Thread.Sleep(4000);
                    }
                    
                    string u = classVars.s_user;
                    System.Threading.Thread.Sleep(2000);
                    string p = classVars.s_pass;

                    foreach (char accNameLetter in u)
                    {
                        SendMessage(proc.MainWindowHandle, classVars.WM_CHAR, new IntPtr(accNameLetter), IntPtr.Zero);
                        System.Threading.Thread.Sleep(100);
                    }
                    
                    SendMessage(proc.MainWindowHandle, classVars.WM_KEYUP, new IntPtr(classVars.VK_TAB), IntPtr.Zero);
                    SendMessage(proc.MainWindowHandle, classVars.WM_KEYDOWN, new IntPtr(classVars.VK_TAB), IntPtr.Zero);

                    foreach (char accPassLetter in p)
                    {
                        SendMessage(proc.MainWindowHandle, classVars.WM_CHAR, new IntPtr(accPassLetter), IntPtr.Zero);
                        System.Threading.Thread.Sleep(100);
                    }
                    
                    SendMessage(proc.MainWindowHandle, classVars.WM_KEYUP, new IntPtr(classVars.VK_RETURN), IntPtr.Zero);
                    SendMessage(proc.MainWindowHandle, classVars.WM_KEYDOWN, new IntPtr(classVars.VK_RETURN), IntPtr.Zero);

                }
                if (classVars.s_playandclose == "true")
                {
                    System.Threading.Thread.Sleep(1000);
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("The worldserver is offline, please try again later..");
            }
        }

        private void lnkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("");
        }

        private void frmLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings set = new frmSettings();
            set.Show();
        }

        private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ini = new ini("settings.ini");
            string path = ini.Read("path");
            if (path != null)
            {
                try
                {
                    Directory.Delete(path.Replace("Wow.exe", "") + "Cache", true);
                }
                catch
                {
                  
                }
            }

        }

        public void GetUpdate()
        {
            //bool b = updateAvailable();
            //if (b && classVars.s_update_checks == "true")
            //{
            //    string temp = "C:\\users\\public\\temp\\latestLauncher\\PTFLauncher.exe";
            //    WebClient wc = new WebClient();
            //    wc.DownloadFile(classVars.url_update, temp);
            //}
        }

        private void createAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBrowser caf = new frmBrowser(classVars.url_base + "wow/pages/register.php");
            caf.Show();
        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(classVars.url_base + "forum/");
        }

        private void überToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(classVars.url_update);
        }

        private void updatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(classVars.b_devUpdate)
            {
                MessageBox.Show("Hey ! You are using a DEVELOPMENT BUILD ! Your version is newer than the version on the server. Nothing to update here.");
            }
            else
            {
                bool b = classNetworking.updateAvailable();
                if (b)
                {
                    MessageBox.Show("Update found!" + " - version: " + classVars.version_server + " - Please download and install the update now.");
                    linkLabel1.Visible = true;
                    dOWNLOADUPDATEToolStripMenuItem.Visible = true;
                }
                else
                {
                    MessageBox.Show("No update found - your version is the latest public version.");
                    linkLabel1.Visible = false;
                    dOWNLOADUPDATEToolStripMenuItem.Visible = false;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bugtrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBugtracker bt = new frmBugtracker();
            bt.Show();

        }

        private void clientDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDownload fd = new frmDownload();
            fd.Show();
        }

        private void debug01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBrowser bt = new frmBrowser(classVars.url_base + "wow/files/bugs/tickets.php");
            bt.Show();
        }

        private void debug02ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBrowser apr = new frmBrowser(classVars.url_base + "wow/pages/apr.php");
            apr.Show();
        }

        private void debug03ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBrowser cmsa = new frmBrowser(classVars.url_base + "mw/admin/");
            cmsa.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            classVars.counter += 1;
            if (classVars.counter > 10)
            {
                MessageBox.Show("Now you managed to click on this image.. \n Congratz.. I guess..");
                classVars.counter = 0;
            }
        }

        private void dOWNLOADUPDATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(classVars.url_update);
        }

        private void restartToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void antiAFKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon");
        }

        public void updatePlayersOnline()
        {
            WebClient onlinePlayers = new WebClient();
            string op = onlinePlayers.DownloadString(classVars.url_s_players);
            lblOnlinePlayers.Text = "Players online: ";
            string s = op.Remove(0,2);
            lblOnlinePlayersValue.Text = s;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateServerStatus();
            updatePlayersOnline();
        }

        private void tmrPlayersOnline_Tick(object sender, EventArgs e)
        {

        }

        private void openClientFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string clPath = classVars.s_path;
                Process.Start(clPath.Replace("Wow.exe", ""));
            }
            catch
            {
                MessageBox.Show("Could not open your client folder. Maybe it does not exist or the path was not set correctly in the settings.");
            }
            
        }
    }

}

