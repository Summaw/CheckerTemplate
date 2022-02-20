using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PornHubChecker
{

    internal class Program
    {

        [STAThread]
        private static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 100000000;
            Colorful.Console.SetWindowSize(35, 35);
            Colorful.Console.Title = Colorful.Console.Title = "[PornHub Checker] | Start Menu | by UmRange";

            Colorful.Console.Clear();
            Colorful.Console.WriteLine();
            Colorful.Console.Write("                             ██▓███   ▒█████   ██▀███   ███▄    █  ██░ ██  █    ██  ▄▄▄▄   \n", Color.DarkOrange);
            Colorful.Console.Write("                            ▓██░  ██▒▒██▒  ██▒▓██ ▒ ██▒ ██ ▀█   █ ▓██░ ██▒ ██  ▓██▒▓█████▄ \n", Color.DarkOrange);
            Colorful.Console.Write("                            ▓██░ ██▓▒▒██░  ██▒▓██ ░▄█ ▒▓██  ▀█ ██▒▒██▀▀██░▓██  ▒██░▒██▒ ▄██\n", Color.DarkOrange);
            Colorful.Console.Write("                            ▒██▄█▓▒ ▒▒██   ██░▒██▀▀█▄  ▓██▒  ▐▌██▒░▓█ ░██ ▓▓█  ░██░▒██░█▀  \n", Color.DarkOrange);
            Colorful.Console.Write("                            ▒██▒ ░  ░░ ████▓▒░░██▓ ▒██▒▒██░   ▓██░░▓█▒░██▓▒▒█████▓ ░▓█  ▀█▓\n", Color.DarkOrange);
            Colorful.Console.Write("                            ▒▓▒░ ░  ░░ ▒░▒░▒░ ░ ▒▓ ░▒▓░░ ▒░   ▒ ▒  ▒ ░░▒░▒░▒▓▒ ▒ ▒ ░▒▓███▀▒\n", Color.DarkOrange);
            Colorful.Console.Write("                            ░▒ ░       ░ ▒ ▒░   ░▒ ░ ▒░░ ░░   ░ ▒░ ▒ ░▒░ ░░░▒░ ░ ░ ▒░▒   ░ \n", Color.DarkOrange);
            Colorful.Console.Write("                            ░░       ░ ░ ░ ▒    ░░   ░    ░   ░ ░  ░  ░░ ░ ░░░ ░ ░  ░    ░ \n", Color.DarkOrange);
            Colorful.Console.WriteLine();

            Thread.Sleep(250);

            // Asks for threads

            Colorful.Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Color.DarkOrange);
            Colorful.Console.Write("> How many ", Color.White);
            Colorful.Console.Write("THREADS", Color.White);
            Colorful.Console.Write(" do you want to use", Color.White);
            Colorful.Console.Write(": ", Color.DarkOrange);

            try
            {
                CheckerHelper.threads = int.Parse(Colorful.Console.ReadLine());
            }
            catch
            {
                CheckerHelper.threads = 100;
            }

            for (; ; ) // if u don't know what is this, it's like while (true) it's a loop but while is gay
            {
                Colorful.Console.Write(DateTime.Now.ToString("[hh:mm:ss]"), Color.DarkOrange);
                Colorful.Console.Write("> What type of ", Color.White);
                Colorful.Console.Write("PROXIES ", Color.White);
                Colorful.Console.Write("[HTTP, SOCKS4, SOCKS5, NO]", Color.DarkOrange);
                Colorful.Console.Write(": ", Color.DarkOrange);
                CheckerHelper.proxytype = Colorful.Console.ReadLine();
                CheckerHelper.proxytype = CheckerHelper.proxytype.ToUpper();
                if (CheckerHelper.proxytype == "HTTP" || CheckerHelper.proxytype == "SOCKS4" || CheckerHelper.proxytype == "SOCKS5" || CheckerHelper.proxytype == "NO")
                {
                    break;
                }
                Colorful.Console.Write("> Please select a valid proxy format.\n\n", Color.Red);
                Thread.Sleep(2000);
            }

            Task.Factory.StartNew(delegate ()
            {
                CheckerHelper.UpdateTitle();
            });

            Colorful.Console.WriteLine();

            // Asks for combos & proxies

            string fileName;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            do
            {
                Colorful.Console.WriteLine("Select your Combos", Color.DarkOrange);
                Thread.Sleep(500);
                openFileDialog.Title = "Select Combo List";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.Filter = "Text files|*.txt";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                fileName = openFileDialog.FileName;
            }
            while (!File.Exists(fileName));

            CheckerHelper.accounts = new List<string>(File.ReadAllLines(fileName));
            CheckerHelper.LoadCombos(fileName);

            Colorful.Console.Write("> ");
            Colorful.Console.Write(CheckerHelper.total, Color.DarkOrange);
            Colorful.Console.WriteLine(" Combos added\n");

            if (CheckerHelper.proxytype != "NO")
            {

                do
                {
                    Colorful.Console.WriteLine("Select your Proxies", Color.DarkOrange);
                    Thread.Sleep(500);
                    openFileDialog.Title = "Select Proxy List";
                    openFileDialog.DefaultExt = "txt";
                    openFileDialog.Filter = "Text files|*.txt";
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.ShowDialog();
                    fileName = openFileDialog.FileName;
                }
                while (!File.Exists(fileName));

                CheckerHelper.proxies = new List<string>(File.ReadAllLines(fileName));
                CheckerHelper.LoadProxies(fileName);

                Colorful.Console.Write("> ");
                Colorful.Console.Write(CheckerHelper.proxytotal, Color.DarkOrange);
                Colorful.Console.WriteLine(" Proxies added\n");
            }

            for (int i = 1; i <= CheckerHelper.threads; i++)
            {
                new Thread(new ThreadStart(CheckerHelper.Check)).Start();
            }

            Colorful.Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
