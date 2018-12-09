
namespace PTFLauncher
{
    public class classVars
    {
        #region Var
        public static string url_base = "http://WEBSITE.net/";
        public static string url_version = url_base + "wow/files/launcher/latest.ini";
        public static string url_update = url_base + "wow/files/launcher/files/latest/";
        public static string url_news = url_base + "wow/files/launcher/news.txt";
        public static string url_s_online = url_base + "wow/files/launcher/s_online.php";
        public static string url_s_players = url_base + "wow/files/launcher/s_players.php";
        public static string ip_worldserver = "";
        public static int port_worldserver = 8085;
        public static string s_playersOnline = "-";
        //Settings stuff (.ini)
        public static string clientpath;
        public static string realmlistpath;
        public static string s_show_pw = "false";
        public static string s_update_checks = "false";
        public static string s_path = "Path not set !";
        public static string s_user = "User Not set !";
        public static string s_pass = "User Not set !";
        public static string s_playandclose = "false";
        public static string s_autologin = "false";
        public static bool b_settingsneeded;
        //Update Stuff
        public static string version_local;
        public static string version_server;
        public static bool playable;
        public static string appversion = "0.0.9";
        //public static string buildDate        = Properties.Resources.BuildDate;
        public static bool b_debug = false;
        public static int counter = 0;
        //! Key codes to send to the client
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint WM_CHAR = 0x0102;
        public const int VK_RETURN = 0x0D;
        public const int VK_TAB = 0x09;
        #endregion
    }
}
