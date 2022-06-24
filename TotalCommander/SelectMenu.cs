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
        void printStart()
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(listDir[SelWin].AllDirecroris[0]);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// метод для управления курсором и для ввода комманд
        /// </summary>
        /// <param name="x">отступ по х</param>
        /// <param name="xMax"></param>
        /// <param name="y">отступ поп y</param>
        /// <returns></returns>
        public void SelectCursor(int x, int xMax, int y)
        {
            Console.SetCursorPosition(2, y);
            printStart();
            List<string> HistiryCommand = new List<string>();
            HistiryCommand.Add("");
            int PositionHistCommand = 0;
            bool Razr = true;
            int select = 0; 
            int X = x;
            int Y = y;
            int[] cursorWeite = new int[] { listDir[SelWin].DirHome.Length + 4, 32 };

            StringBuilder command = new StringBuilder(255);
            do
            {
                int DlRes = listDir[SelWin].AllDirecroris.Length - ((Line1_1 - 2) * listDir[SelWin].Page);
                int yMax = DlRes < Line1_1 ? DlRes : Line1_1 - 1;
                Console.SetCursorPosition(cursorWeite[0], cursorWeite[1]);
                if (PositionHistCommand != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(HistiryCommand[PositionHistCommand]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(cursorWeite[0] + HistiryCommand[PositionHistCommand].Length, cursorWeite[1]);
                }
                Console.ForegroundColor = ConsoleColor.White;

                Console.CursorVisible = true;
                ConsoleKeyInfo infoKey = Console.ReadKey(true);
                Console.CursorVisible = false;
                switch (infoKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (Y != y)
                        {
                            PressUpp(ref Y, ref select, X);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Y != yMax)
                        {
                            PressDown(ref Y, ref select, X);
                        }
                        break;
                    case ConsoleKey.F1:
                        PressChangePage(ref Y, ref select);
                        break;
                    case ConsoleKey.F5:
                        PressF5(ref select);
                        break;
                    case ConsoleKey.Tab:
                        PressTAB(ref select, ref Y, ref X);
                        cursorWeite[0] = listDir[SelWin].DirHome.Length + 4;
                        break;
                    case ConsoleKey.Escape:
                        Razr = false;
                        break;
                    case ConsoleKey.Enter:
                        if (command.Length == 0)
                        {
                            EnterChengeDir(ref select, ref Y);
                            cursorWeite[0] = listDir[SelWin].DirHome.Length + 4;
                        }
                        else
                        {
                            HistiryCommand.Add(command.ToString());
                            EnterDoCommand(ref command);
                            cursorWeite[0] = listDir[SelWin].DirHome.Length + 4;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (command.Length > 0)
                        {
                            command.Remove(command.Length - 1, 1);
                            cursorWeite[0]--;
                            Console.SetCursorPosition(cursorWeite[0], cursorWeite[1]);
                            Console.Write(" ");
                        }
                        break;
                    default:
                        if ((infoKey.Modifiers & ConsoleModifiers.Control) != 0)
                        {
                            Print(listDir[SelWin].Page, SelWin);
                            CommandHistory(infoKey, HistiryCommand, ref PositionHistCommand);
                        }
                        if (cursorWeite[0] < WindowsWidth - 2)
                        {
                            if (infoKey.Key != ConsoleKey.UpArrow && infoKey.Key != ConsoleKey.DownArrow &&
                                infoKey.Key != ConsoleKey.LeftArrow && infoKey.Key != ConsoleKey.RightArrow)
                            {
                                command.Append(infoKey.KeyChar);
                                Console.SetCursorPosition(cursorWeite[0], cursorWeite[1]);
                                Console.Write(infoKey.KeyChar);
                                cursorWeite[0]++;
                            }
                        }
                        break;
                }
            }
            while (Razr);
        }
        /// <summary>
        /// отработка нажатие клавиши верх
        /// паралельная работа метода подсчета размера файла/папки 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="select"></param>
        /// <param name="X"></param>
        void PressUpp(ref int Y, ref int select, int X)
        {
            GetName(select, listDir[SelWin].AllDirecroris, X, Y, 1);
            Y--;
            select = select > 0 ? --select : 0;
            GetName(select, listDir[SelWin].AllDirecroris, X, Y, 0);
            int sel = select;
            Task task = new Task(() => PrintInfo(sel - 1, SelWin));
            task.Start();
        }
        /// <summary>
        /// отработка нажатие клавиши вниз
        /// паралельная работа метода подсчета размера файла/папки 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="select"></param>
        /// <param name="X"></param>
        void PressDown(ref int Y, ref int select, int X)
        {
            if (Y != 0)
            {
                GetName(select, listDir[SelWin].AllDirecroris, X, Y, 1);
            }
            Y++;
            select++;
            GetName(select, listDir[SelWin].AllDirecroris, X, Y, 0);
            //Thread myThread = new Thread(new ThreadStart(ORIO));
            //myThread.Start();
            int sel = select;
            Task task = new Task(() => PrintInfo(sel - 1, SelWin));
            task.Start();
            //await PrintInfo(select - 1, SelWin);
        }
        /// <summary>
        /// метод сметы страниццы
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="select"></param>
        void PressChangePage(ref int Y, ref int select)
        {
            Y = 1;
            if (listDir[SelWin].Page != listDir[SelWin].PageMax - 1)
            {
                if (listDir[SelWin].PageMax != 0)
                {
                    listDir[SelWin].Page++;
                    select = Line1_1 - 2;
                }
            }
            else
            {
                listDir[SelWin].Page = 0;
                select = 0;
            }
            DeliteWindows(SelWin);
            Print(listDir[0].Page, 0);
            Print(listDir[1].Page, 1);
            GetPages(listDir[SelWin].Page, SelWin);
        }
        /// <summary>
        /// нажатие на Ф5
        /// </summary>
        /// <param name="select"></param>
        void PressF5(ref int select)
        {
            DelitConsole();
            int Sel2 = SelWin == 0 ? 1 : 0;
            string com = "CP " + listDir[SelWin].AllDirecroris[select] + " " + listDir[Sel2].DInfo;
            DoCommand command = new DoCommand();
            command.DoProcess(com);
            DeliteWindows(Sel2);
            //Print(listDir[SelWin].Page, SelWin);
            listDir[Sel2].GetInfo();
            Print(listDir[Sel2].Page, Sel2);
        }
        /// <summary>
        /// метод нажатия на смену окна
        /// </summary>
        /// <param name="select"></param>
        /// <param name="Y"></param>
        /// <param name="X"></param>
        void PressTAB(ref int select, ref int Y, ref int X)
        {
            GetName(select, listDir[SelWin].AllDirecroris, X, Y, 1);
            Y = 1;
            select = 0;
            SelWin = SelWin == 0 ? 1 : 0;
            X = SelWin == 0 ? 2 : WindowsWidth / 2 + 1;
            GetName(select, listDir[SelWin].AllDirecroris, X, Y, 0);
            Console.ForegroundColor = ConsoleColor.White;
            DelitConsole();
            Console.SetCursorPosition(2, Line2_2 + 1);
            Console.Write(listDir[SelWin].DirHome + " > ");
        }
        /// <summary>
        /// смена директории на выбранную
        /// </summary>
        /// <param name="select"></param>
        void EnterChengeDir(ref int select, ref int Y)
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
                    listDir[SelWin].DInfo = new DirectoryInfo(listDir[SelWin].dirInfo[select - 1].FullName);
                    listDir[SelWin].DirHome = listDir[SelWin].dirInfo[select - 1].FullName;
                    DeliteWindows(SelWin);
                    StartWinMeneger();
                    //Print(Page);
                    select = 0;
                }
                catch (Exception ex)
                {
                    select = 0;
                    Print(listDir[SelWin].Page, SelWin);
                    PrintErrorsInfo(ex.Message);
                }
            }
        }
        /// <summary>
        /// выполнение комад
        /// </summary>
        /// <param name="command"></param>
        void EnterDoCommand(ref StringBuilder command)
        {
            DelitConsole();
            DoCommand comDo = new DoCommand();
            comDo.DoProcess(command.ToString());
            command.Remove(0, command.Length);
            Print(listDir[SelWin].Page, SelWin);
        }
        /// <summary>
        /// перебор п оистории команд
        /// </summary>
        /// <param name="key"></param>
        /// <param name="strBild"></param>
        /// <param name="PositionHistCommand"></param>
        void CommandHistory(ConsoleKeyInfo key, List<string> strBild, ref int PositionHistCommand)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (PositionHistCommand != 0)
                    {
                        PositionHistCommand--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (PositionHistCommand < strBild.Count - 1)
                    {
                        PositionHistCommand++;
                    }
                    break;
            }
        }
        /// <summary>
        /// поучение родительской директории
        /// </summary>
        void GetParentDirectory()
        {
            try
            {
                listDir[SelWin].DInfo = new DirectoryInfo(listDir[SelWin].DInfo.Parent.FullName);
                listDir[SelWin].DirHome = listDir[SelWin].DInfo.FullName;
                DeliteWindows(SelWin);
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
            if (poz <= listDir[SelWin].dirInfo.Length)
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
