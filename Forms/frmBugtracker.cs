using System;
using System.Windows.Forms;
using System.Net;//For web connectivity
using System.IO;//Reading and writing stuff to disk

namespace PTFLauncher
{
    public partial class frmBugtracker : Form
    {
        public frmBugtracker()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // - TODO
            if(textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {

                ////All boxes have infos
                ////Proceed
                try
                {
                    //Compile text array
                    //Need a fingerprint first otherwise all users will upload the same file 
                    int rng;
                    Random rnd          = new Random();
                    rng                 = rnd.Next(0, 9999);
                    //Get username to string
                    var ini             = new ini("settings.ini");
                    string username     = ini.Read("user");
                    //Finally append all stuff together 
                    string path         = "C:\\users\\public\\" 
                        + username + "_" + rng + "bugreport.txt";
                    //Get it written on disk 
                    File.WriteAllText(path, "\n" + DateTime.Now.ToString() + " Account Name: " + textBox1.Text + " Ort: " + textBox2.Text +
                       " Was genau : " + textBox3.Text + " Sonstiges : " + textBox4.Text + "\n");
                    //upload
                    WebClient uploader    = new WebClient();
                    uploader.Headers.Add("Content-Type", "binary/octet-stream");
                    byte[] result       = uploader.UploadFile(classVars.url_base + "wow/files/bugs/server.php", "POST", path);
                    //string s1;
                    //s1 = System.Text.Encoding.UTF8.GetString(result, 0, result.Length);
                    //File.Delete(path);
                }//Error handling
                catch  { MessageBox.Show("Da ist etwas schief gegangen.. Probier es bitte erneut."); }
                //Success!
                MessageBox.Show("Vielen Dank für deine Mithilfe! " +
                    "\n" +
                    "Ein Gamemaster wird sich jetzt um dein Problen kümmern.");
                this.Close();//Back to biz
            }
            else
            {
                MessageBox.Show("Bitte fülle alle Felder aus.");
            }
        }

        private void frmBugtracker_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            try
            {   //Basically just reading out the username to be prepared in the
                //Form window
                var ini = new ini("settings.ini");
                string username = ini.Read("user");
                textBox1.Text = username;
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(classVars.url_base + "wow/files/bugs/tickets.php");
        }
    }
}
