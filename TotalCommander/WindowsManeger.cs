using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TotalCommander
{
    /// <summary>
    /// клвсс для отображение иформации в рамках
    /// </summary>
    internal class WindowsManeger : Program
    {
        public static List<ClassDataDirectores> listDir = new List<ClassDataDirectores>();
        public static int SelWin;
        public WindowsManeger()
        {
        }
        public WindowsManeger(string _dir)
        {
            SelWin = 0;
            ClassDataDirectores dataDirectory = new ClassDataDirectores();
            dataDirectory.DirHome = _dir;
            dataDirectory.SetDInfo();
            listDir.Add(dataDirectory);
            listDir.Add(new ClassDataDirectores());
            listDir[1].DirHome = Properties.Settings.Default.HomeDirrction2;
            listDir[1].SetDInfo();
            listDir[1].GetInfo();
        }
        /// <summary>
        /// запуск отрисовки и сбора информации главного окна
        /// </summary>
        public void StartWinMeneger()
        {
            listDir[0].GetInfo();
            listDir[0].Page = 0;
            Print(listDir[0].Page, 0);
            GetPages(listDir[0].Page, 0);

            listDir[1].GetInfo();
            Print(listDir[1].Page, 1);
            GetPages(listDir[1].Page, 1);
        }
        /// <summary>
        /// отрисовка в окне 1 (окно с директориями)
        /// </summary>
        /// <param name="page">номер страницы для отрисовки</param>
        public void Print(int page, int NumDisplay)
        {
            int poz = 1;
            int Wind;
            if (NumDisplay == 0)
            {
                Wind = 2;
            }
            else
            {
                Wind = WindowsWidth / 2 + 1;
            }
            for (int i = (Line1_1 - 2) * page; i < listDir[NumDisplay].AllDirecroris.Length; i++, poz++)
            {
                if (poz <= Line1_1 - 1)
                {
                    if (i <= listDir[NumDisplay].dirInfo.Length)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.SetCursorPosition(Wind, poz);
                    Console.Write(listDir[NumDisplay].AllDirecroris[i]);
                }
                else
                {
                    break;
                }
            }
            Display dis = new Display();
            dis.PrinntDisplay2Line();

            if (SelWin == NumDisplay)
            {
                Console.ForegroundColor = ConsoleColor.White;
                DelitConsole();
                Console.SetCursorPosition(2, Line2_2 + 1);
                Console.Write(listDir[NumDisplay].DirHome + " > ");
            }
        }
        /// <summary>
        /// метод для получения количества страниц
        /// </summary>
        /// <param name="nowPage">текущая страница</param>
        public void GetPages(int nowPage, int NumDisplay)
        {
            int dlina = listDir[NumDisplay].dirInfo.Length + listDir[NumDisplay].fileInfo.Length;
            Console.ForegroundColor = ConsoleColor.Blue;
            listDir[NumDisplay].PageMax = dlina / (Line1_1 - 2) + (dlina % Line1_1 - 2 == 0 ? 0 : 1);
            string p = "╣ Page " + (nowPage + 1) + "/" + listDir[NumDisplay].PageMax + " ╠";
            PrintPage(p);
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// метод для отрисовки знака с количеством страниц и текущей страниц
        /// </summary>
        /// <param name="pages"></param>
        void PrintPage(string pages)
        {
            Console.SetCursorPosition(WindowsWidth / 2 - pages.Length / 2, Line1_1);
            Console.Write(pages);
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// метод для очистки верхнего окна 
        /// </summary>
        public void DeliteWindows()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 1; i < WindowsWidth - 1; i++)
            {
                for (int j = 1; j < Line1_1; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(" ");
                }
            }
        }
        public void DeliteWindows(int NumDisp)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            int start, end;
            if (NumDisp == 0)
            {
                start = 1;
                end = (WindowsWidth - 1) / 2;
            }
            else
            {
                end = WindowsWidth - 1;
                start = WindowsWidth / 2;
            }
            for (int i = start; i < end; i++)
            {
                for (int j = 1; j < Line1_1; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(" ");
                }
            }
        }
        /// <summary>
        /// метод для очистки окна с информацией 
        /// </summary>
        public void InfoDelit()
        {
            Console.CursorVisible = false;
            for (int i = 1; i < WindowsWidth - 2; i++)
            {
                for (int j = Line1_2 + 1; j < Line2_1; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(" ");
                }
            }
        }
        /// <summary>
        /// асинхронный метод для получения в окно с иформацией
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public void PrintInfo(int select, int NumDisp)//async Task
        {
            Console.CursorVisible = false;
            if (select > -1)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                InfoDelit();
                if (select < listDir[NumDisp].dirInfo.Length)
                {
                    long Sum = GetSizeDirectory(listDir[NumDisp].dirInfo[select]);
                    string mess = listDir[NumDisp].dirInfo[select].Name + "  " + listDir[NumDisp].dirInfo[select].LastWriteTime
                        + "  " + Sum / 1024 + " Mb";
                    Console.SetCursorPosition(2, Line1_2 + 1);
                    PrintStroks(mess);
                }
                else
                {
                    select -= listDir[NumDisp].dirInfo.Length;
                    long Sum = GetSizeFile(listDir[NumDisp].fileInfo[select]);
                    string mess = listDir[NumDisp].fileInfo[select].Name + "  " + listDir[NumDisp].fileInfo[select].LastWriteTime
                        + "  " + Sum / 1024 + " Mb";
                    Console.SetCursorPosition(2, Line1_2 + 1);
                    PrintStroks(mess);
                }
            }
        }
        void PrintStroks(string mess)
        {
            if (mess.Length > WindowsWidth - 2)
            {
                string mess1 = mess.Substring(0, WindowsWidth / 2);
                string mess2 = mess.Substring((WindowsWidth / 2) + 1, mess.Length - 1);
                Console.Write(mess1);
                Console.SetCursorPosition(2, Line1_2 + 2);
                Console.Write(mess2);
            }
            else
            {
                Console.Write(mess);
            }
        }
        /// <summary>
        /// метод для отрисовки об ошибках
        /// </summary>
        /// <param name="error">сообщение об ошибке</param>
        public void PrintErrorsInfo(string error)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(2, Line1_2 + 3);
            Console.ForegroundColor = ConsoleColor.Red;
            PrintStroks(error);
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                SaveErrors(error);
            }
            catch
            { }
        }
        /// <summary>
        /// метод для получения размера директории
        /// </summary>
        /// <param name="infoD">выбранная директория</param>
        /// <returns></returns>
        long GetSizeDirectory(DirectoryInfo infoD)
        {
            try
            {
                long Sum = 0;
                FileInfo[] filesss = infoD.GetFiles();
                foreach (FileInfo fi in filesss)
                {
                    Sum += fi.Length;
                }
                DirectoryInfo[] dir2 = infoD.GetDirectories();
                if (dir2.Length > 0)
                {
                    for (int i = 0; i < dir2.Length; i++)
                    {
                        Sum += GetSizeDirectory(dir2[i]);
                    }
                }
                return Sum;
            }
            catch (Exception ex)
            {
                PrintErrorsInfo(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// метод для получения размера файла
        /// </summary>
        /// <param name="fileInfo">выбранный файл</param>
        /// <returns></returns>
        long GetSizeFile(FileInfo fileInfo)
        {
            try
            {
                return fileInfo.Length;
            }
            catch (Exception ex)
            {
                PrintErrorsInfo(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// метод для очистки окна консоли
        /// </summary>
        public void DelitConsole()
        {
            Console.SetCursorPosition(1, Line2_2 + 1);
            for (int i = 0; i < WindowsWidth - 2; i++)
            {
                Console.Write(" ");
            }
        }
        //public int nowDirConsile()
        //{
        //    StringBuilder shortPathName = new StringBuilder((int)API.MAX_PATH);
        //    API.GetShortPathName(DirHome, shortPathName, API.MAX_PATH);
        //    PrintnowDirConsile(shortPathName.ToString());
        //    return shortPathName.Length + 2;
        //}
        void PrintnowDirConsile(string Path)
        {
            DelitConsole();
            Console.SetCursorPosition(1, Line2_2 + 1);
            Console.Write(Path + "> ");
        }
    }
}
