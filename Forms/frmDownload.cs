using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace PTFLauncher
{
    public partial class frmDownload : Form
    {
        public frmDownload()
        {
            InitializeComponent();
        }
        public static string s_dlpath;
        WebClient webClient;               // Our WebClient that will be doing the downloading for us
        Stopwatch sw = new Stopwatch();
        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            label2.Text = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar1.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            label3.Text = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            label1.Text = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        // The event that will trigger when the WebClient is completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.");
            }
            else
            {
                MessageBox.Show("Download completed!");
                button1.Enabled = true;
            }
        }
        public void DownloadFile(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, location);
                    //If canceled do this 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Möchtest du den 2.4.3 TBC Client herunterladen ? \n" +
                "Der Download kann nicht angehalten werden , der Launcher wird beendet wenn du den Download stoppst!", "Download", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.BringToFront();
                button3.Enabled = true;
                //Download to folder of users choice
                string downloadfolder;
                SaveFileDialog sfd = new SaveFileDialog();
                //MessageBox.Show("Please enter a filename to download to , for example TBC.zip " +
                //"\nMake sure to write .zip at the end !!");
                sfd.ShowDialog();
                downloadfolder = sfd.FileName + ".zip";
                s_dlpath = downloadfolder;
                button2.Enabled = false;
                label1.Text = "preparing ..";
                try
                {
                    File.Delete(downloadfolder);

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }

                try
                {
                    DownloadFile("dl.m1mk.de/private/wow_clients/tbc.zip", downloadfolder);
                    this.Text += " - " + downloadfolder;
                }
                catch (Exception eee)
                {
                    MessageBox.Show(eee.ToString());
                }
            }
            else if (result == DialogResult.No)
            {
                //do else
            }
            
        }

        private void frmDownload_Load(object sender, EventArgs e)
        {
            label3.Text = "";
            label1.Text = "";
            this.TopMost = true;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sw.Reset();
            try
            {
                File.Delete(s_dlpath);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
            MessageBox.Show("Download cant be canceled without the application to exit. We are sorry but you have to restart this program manually.");
            System.Windows.Forms.Application.Restart();
            System.Windows.Forms.Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\users\\public\\tbc.zip");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please click on the tbc.zip file which is wow 2.4.3 Burning Crusade.");
            Process.Start("http://WEBSITE.net/private/wow_clients/");
        }
    }
}
