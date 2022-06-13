using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TotalCommander
{
    internal class SelectMenu : WindowsManeger
    {
        /// <summary>
        /// метод для управления курсором и для ввода комманд
        /// </summary>
        /// <param name="x">отступ по х</param>
        /// <param name="xMax"></param>
        /// <param name="y">отступ поп y</param>
        /// <returns></returns>
        public async Task SelectCursor(int x, int xMax, int y)
        {
            List<string> history = new List<string>();
            history.Add(" ");
            bool Razr = true;
            Console.SetCursorPosition(2, y);
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(AllDirecroris[0]);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            int select = 0;
            int Xhistory = 0;
            int Y = y;
            int[] cursorWrite = new int[] { 2, 32 };

            StringBuilder command = new StringBuilder(255);
            do
            {
              cursorWrite[0] = nowDirConsile() + command.Length;
                if (command.Length != 0)
                {
                    Console.Write(command.ToString());
                }
                else if (Xhistory != 0)
                {
                    Console.SetCursorPosition(cursorWrite[0] + 1, cursorWrite[1]);
                    Console.Write(history[Xhistory].ToString());
                }

                int DlRes = AllDirecroris.Length - ((Line1_1 - 2) * Page);
                int yMax = DlRes < Line1_1 ? DlRes : Line1_1 - 1;

                //Console.SetCursorPosition(cursorWrite[0] + history[Xhistory].Length, cursorWrite[1]);

                Console.CursorVisible = true;
                ConsoleKeyInfo infoKey = Console.ReadKey(true);
                Console.CursorVisible = false;
                switch (infoKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (Y != y)
                        {
                            GetName(select, AllDirecroris, x, Y, 1);
                            Y--;
                            select--;
                            GetName(select, AllDirecroris, x, Y, 0);
                            await PrintInfo(select - 1);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Y != yMax)
                        {
                            if (Y != 0)
                            {
                                GetName(select, AllDirecroris, x, Y, 1);
                            }
                            Y++;
                            select++;
                            GetName(select, AllDirecroris, x, Y, 0);
                            await PrintInfo(select - 1);
                        }
                        break;
                    case ConsoleKey.Tab:
                        Y = 1;
                        if (Page != PageMax - 1)
                        {
                            Page++;
                            select = Line1_1 - 2;
                        }
                        else
                        {
                            Page = 0;
                            select = 0;
                        }
                        DeliteWindows();
                        Print(Page);
                        GetPages(Page);
                        break;
                    case ConsoleKey.Escape:
                        Razr = false;
                        break;
                    case ConsoleKey.Enter:
                        if (command.Length == 0 && Xhistory == 0)
                        {
                            Y = 1;
                            if (select == 0)
                            {
                                GetParentDirectory();
                            }
                            else
                            {
                                try
                                {
                                    DInfo = new DirectoryInfo(dirInfo[select - 1].FullName);
                                    DirHome = dirInfo[select - 1].FullName;
                                    StartWinMeneger();
                                    DeliteWindows();
                                    Print(Page);
                                    select = 0;
                                }
                                catch (Exception ex)
                                {
                                    select = 0;
                                    Print(Page);
                                    PrintErrorsInfo(ex.Message);
                                }
                            }
                        }
                        else if (Xhistory != 0 && command.Length == 0)
                        {
                            DoCommand comDo = new DoCommand();
                            comDo.DoProcess(history[Xhistory]);
                            Xhistory = 0;
                            DelitConsole();
                            cursorWrite[0] = 2;
                        }
                        else
                        {
                            DoCommand comDo = new DoCommand();
                            comDo.DoProcess(command.ToString());
                            history.Add(command.ToString());
                            command.Remove(0, command.Length);
                            DelitConsole();
                            cursorWrite[0] = 2;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (command.Length > 0)
                        {
                            command.Remove(command.Length - 1, 1);
                            cursorWrite[0]--;
                            Console.SetCursorPosition(cursorWrite[0], cursorWrite[1]);
                            Console.Write(" ");
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Xhistory != 0)
                        {
                            Xhistory--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Xhistory < history.Count - 1)
                        {
                            Xhistory++;
                        }
                        break;
                    default:
                        if (cursorWrite[0] < WindowsWidth - 2)
                        {
                            command.Append(infoKey.KeyChar);;
                            Xhistory = 0;
                        }
                        break;
                }
            }
            while (Razr);
        }
        /// <summary>
        /// поучение родительской директории
        /// </summary>
        void GetParentDirectory()
        {
            try
            {
                DInfo = new DirectoryInfo(DInfo.Parent.FullName);
                DirHome = DInfo.FullName;
                DeliteWindows();
                StartWinMeneger();
            }
            catch (Exception ex)
            {
                PrintErrorsInfo(ex.Message);
            }
        }
        /// <summary>
        /// получение имени для отрисовки в курсоре
        /// </summary>
        /// <param name="poz">позиция курсора</param>
        /// <param name="direcroris"></param>
        /// <param name="x">позиция по х</param>
        /// <param name="y">позиция по y</param>
        /// <param name="color">номер цвета</param>
        void GetName(int poz, string[] direcroris, int x, int y, int color)
        {
            if (poz <= dirInfo.Length)
            {
                Print(x, y, direcroris[poz], color);
            }
            else
            {
                if (color == 1)
                {
                    Print(x, y, direcroris[poz], 2);
                }
                else
                {
                    Print(x, y, direcroris[poz], color);
                }
            }
        }
        /// <summary>
        /// отрисовка 
        /// </summary>
        /// <param name="x">позиция по х</param>
        /// <param name="y">позиция по y</param>
        /// <param name="res">строка</param>
        /// <param name="color">номер цвета</param>
        void Print(int x, int y, string res, int color)
        {
            switch (color)
            {
                case 0:
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 1:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.SetCursorPosition(x, y);
            Console.Write(res);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
