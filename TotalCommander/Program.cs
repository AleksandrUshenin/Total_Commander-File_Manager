using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TotalCommander
{
    internal class Program
    {
        public const int WindowHeight = 34;
        public const int WindowsWidth = 100;

        public const int Line1_1 = 23;
        public const int Line1_2 = 24;
        public const int Line2_1 = 30;
        public const int Line2_2 = 31;
        /// <summary>
        /// ТОчка входа 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowsWidth;
            Console.BufferHeight = WindowHeight;
            Console.BufferWidth = WindowsWidth + 1;
            Console.Title = "File Maneger";
            Display dis = new Display();
            dis.Print(WindowHeight, WindowsWidth);
            Console.CursorVisible = false;
            Body();
        }
        /// <summary>
        /// метод по запуску программы 
        /// </summary>
        static void Body()
        {
            if (Properties.Settings.Default.HomeDirrction == "")
            {
                Properties.Settings.Default.HomeDirrction = Directory.GetCurrentDirectory();
            }
            if (Properties.Settings.Default.HomeDirrction2 == "")
            {
                Properties.Settings.Default.HomeDirrction2 = Directory.GetCurrentDirectory();
            }
            Properties.Settings.Default.Save();
            if (!Directory.Exists(Properties.Settings.Default.HomeDirrction))
            {
                WindowsManeger wm = new WindowsManeger(Directory.GetCurrentDirectory());
                Properties.Settings.Default.HomeDirrction2 = Directory.GetCurrentDirectory();
                Properties.Settings.Default.Save();
                wm.StartWinMeneger();
            }
            else
            {
                WindowsManeger wm = new WindowsManeger(Properties.Settings.Default.HomeDirrction);
                wm.StartWinMeneger();
            }

            SelectMenu se = new SelectMenu();
            se.SelectCursor(2, WindowHeight - 2, 1);
        }
        static public void SaveErrors(string Err)
        {
            string _dir = Directory.GetCurrentDirectory();
            //_dir = Path.Combine(_dir, "errorss", "random_name_exception.txt");
            if (!Directory.Exists(Path.Combine(_dir, "errorss")))
            {
                Directory.CreateDirectory(Path.Combine(_dir, "errorss"));
            }
            if (!File.Exists((Path.Combine(_dir, "errorss", "random_name_exception.txt"))))
            {
                File.Create((Path.Combine(_dir, "errorss", "random_name_exception.txt"))).Dispose();
            }
            File.AppendAllText((Path.Combine(_dir, "errorss", "random_name_exception.txt")), Err + " : " + DateTime.Now);
        }
    }
}
