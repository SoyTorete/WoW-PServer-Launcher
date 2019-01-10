using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace PTFLauncher
{
    public class classNetworking
    {
        //TODO : Move entire networking code here
        public bool testWorldServer()
        {
            //Outsourced to php scripts .. bug with ISP
            TcpClient tc = new TcpClient();
            try
            {
                tc.Connect(classVars.ip_worldserver, classVars.port_worldserver);
                bool stat = tc.Connected;
                if (stat)
                    return true;

                tc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            tc.Close();
            return false;
        }
        public static bool updateAvailable()
        {
            //Check if theres a update on the server
            try
            {   //Init 
                WebClient wc = new WebClient();
                //Get server version (latest launcher version)
                string s = wc.DownloadString(classVars.url_version);
                //Set local version
                string s2 = classVars.appversion;
                classVars.version_server = s;//Store latest version in memory

                if (s != s2)//Compare the versions
                {
                    //Process our version variables to actual int numbers
                    int i = Convert.ToInt32(s.Replace(".", ""));//Server
                    int i2 = Convert.ToInt32(s2.Replace(".", ""));//Local
                    //When the server version is actually GREATER than our version
                    if (i > i2)
                    {
                        //Theres a higher version available
                        return true;
                    }
                    else if (i < i2)//When using a newer version than server has
                    {
                        classVars.b_devUpdate = true;
                        //MessageBox.Show("Oops ! You are using a development version! Your version is newer than the latest public version on our servers!");
                        return false;
                    }
                    //When the versions are not equal BUT NOT GREATER
                    return false;
                }
                //Return fail anyways
                return false;
            }
            catch
            {
                //Failsafe!
                return false;
            }
        }
        public static bool checkConnection()
        {
            WebClient wc = new WebClient();
            try
            {
                wc.DownloadString(classVars.url_s_online);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
