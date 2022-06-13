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
        public static string DirHome;
        public static DirectoryInfo DInfo;
        public static DirectoryInfo[] dirInfo;
        public static FileInfo[] fileInfo;
        public static int Page;
        public static int PageMax;
        public static string[] AllDirecroris;
        public WindowsManeger()
        {
        }
        public WindowsManeger(string _dir)
        {
            DirHome = _dir;
            DInfo = new DirectoryInfo(DirHome);
        }
        /// <summary>
        /// получение информации о текущем каталоге
        /// </summary>
        public void GetInfo()
        {
            dirInfo = DInfo.GetDirectories();
            fileInfo = DInfo.GetFiles();

            AllDirecroris = new string[dirInfo.Length + fileInfo.Length + 1];
            AllDirecroris[0] = @"\..";

            for (int i = 0; i < dirInfo.Length; i++)
            {
                AllDirecroris[i + 1] = dirInfo[i].Name;
            }
            int i2 = 0;
            for (int i = dirInfo.Length + 1; i2 < fileInfo.Length; i++, i2++)
            {
                AllDirecroris[i] = fileInfo[i2].Name;
            }
        }
        /// <summary>
        /// запуск отрисовки и сбора информации главного окна
        /// </summary>
        public void StartWinMeneger()
        {
            GetInfo();
            Page = 0;
            GetPages(Page);
            Print(Page);
        }
        /// <summary>
        /// отрисовка в окне 1 (окно с директориями)
        /// </summary>
        /// <param name="page">номер страницы для отрисовки</param>
        public void Print(int page)
        {
            int poz = 1;
            for (int i = (Line1_1 - 2) * page; i < AllDirecroris.Length; i++, poz++)
            {
                if (poz <= Line1_1 - 1)
                {
                    if (i <= dirInfo.Length)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.SetCursorPosition(2, poz);
                    Console.Write(AllDirecroris[i]);
                }
                else
                {
                    break;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// метод для получения количества страниц
        /// </summary>
        /// <param name="nowPage">текущая страница</param>
        public void GetPages(int nowPage)
        {
            int dlina = dirInfo.Length + fileInfo.Length;
            Console.ForegroundColor = ConsoleColor.Blue;
            PageMax = dlina / (Line1_1 - 2) + (dlina % Line1_1 - 2 == 0 ? 0 : 1);
            string p = "╣ Page " + (nowPage + 1) + "/" + PageMax + " ╠";
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
        /// <summary>
        /// метод для очистки окна с информацией 
        /// </summary>
        public void InfoDelit()
        {
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
        public async Task PrintInfo(int select)
        {
            if (select > -1)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                InfoDelit();
                Console.SetCursorPosition(2, Line1_2 + 1);

                if (select < dirInfo.Length)
                {
                    long Sum = GetSizeDirectory(dirInfo[select]);
                    Console.Write(dirInfo[select].Attributes + " ---- " + dirInfo[select].Name + " ---- " + Sum / 1024 + " Mb");
                    Console.SetCursorPosition(2, Line1_2 + 2);
                    Console.Write(dirInfo[select].CreationTime);
                }
                else
                {
                    select -= dirInfo.Length;
                    long Sum = GetSizeFile(fileInfo[select]);
                    Console.Write(fileInfo[select].Attributes + " ---- " + fileInfo[select].Name + " ---- " + Sum / 1024 + " Mb");
                    Console.SetCursorPosition(2, Line1_2 + 2);
                    Console.Write(fileInfo[select].LastWriteTime + " ---- " + fileInfo[select].Extension);
                }
            }
        }
        /// <summary>
        /// метод для отрисовки об ошибках
        /// </summary>
        /// <param name="error">сообщение об ошибке</param>
        public void PrintErrorsInfo(string error)
        {
            Console.SetCursorPosition(3, Line1_2 + 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(error);
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
        public int nowDirConsile()
        {
            StringBuilder shortPathName = new StringBuilder((int)API.MAX_PATH);
            API.GetShortPathName(DirHome, shortPathName, API.MAX_PATH);
            PrintnowDirConsile(shortPathName.ToString());
            return shortPathName.Length + 2;
        }
        void PrintnowDirConsile(string Path)
        {
            DelitConsole();
            Console.SetCursorPosition(1, Line2_2 + 1);
            Console.Write(Path + "> ");
        }
    }
}
