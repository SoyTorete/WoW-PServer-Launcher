using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTFLauncher
{
    public partial class frmBrowser : Form
    {
        public string url;
        public frmBrowser(string urlpassed)
        {
            url = urlpassed;
            InitializeComponent();
        }

        private void frmBrowser_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            webBrowser1.Navigate(url);
            this.Text = webBrowser1.DocumentTitle;
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
            this.Text = webBrowser1.DocumentTitle;
        }

        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
            this.Text = webBrowser1.DocumentTitle;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            this.Text = webBrowser1.DocumentTitle;
        }
    }
}
