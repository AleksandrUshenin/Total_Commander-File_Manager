using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TotalCommander
{
    internal class DoCommand : WindowsManeger
    {
        /// <summary>
        /// выбор команды 
        /// </summary>
        /// <param name="com">команда</param>
        public void DoProcess(string com)
        {
            try
            {
                switch (com.Split(' ').First().ToUpper())
                {
                    case "CD":
                        ChangeDir(com.Split(' ').Last());
                        break;
                    case "CP":
                        CP(Path.Combine(listDir[SelWin].DirHome, com.Split(' ')[1]), com.Split(' ')[2]);
                        break;
                    case "RM":
                        if (com.Split(' ')[1] == "-D" || com.Split(' ')[1] == "-d")
                        {
                            Rm_D(Path.Combine(listDir[SelWin].DirHome, com.Split(' ').Last()));
                        }
                        Rm(com.Split(' ').Last());
                        break;
                    case "MKDIR":
                        Mkdir(com.Split(' ').Last());
                        break;
                    case "TOUCH":
                        Touch(com.Split(' ').Last());
                        break;
                    case "SHD":
                        Properties.Settings.Default.HomeDirrction = listDir[0].DirHome;
                        Properties.Settings.Default.HomeDirrction2 = listDir[1].DirHome;
                        Properties.Settings.Default.Save();
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintErrorsInfo(ex.Message);
            }
        }
        /// <summary>
        /// смена директории
        /// </summary>
        /// <param name="com"></param>
        void ChangeDir(string com)
        {
            listDir[SelWin].DirHome = com;
            listDir[SelWin].DInfo = new DirectoryInfo(listDir[SelWin].DirHome);
            DeliteWindows(SelWin);
            StartWinMeneger();
        }
        /// <summary>
        /// создание директории 
        /// </summary>
        /// <param name="com"></param>
        void Mkdir(string com)
        {
            Directory.CreateDirectory(Path.Combine(listDir[SelWin].DirHome, com));
            DeliteWindows(SelWin);
            StartWinMeneger();
        }
        /// <summary>
        /// создание файла
        /// </summary>
        /// <param name="com"></param>
        void Touch(string com)
        {
            File.Create(Path.Combine(listDir[SelWin].DirHome, com));
            DeliteWindows(SelWin);
            StartWinMeneger();
        }
        /// <summary>
        /// удаление
        /// </summary>
        /// <param name="com"></param>
        void Rm(string com)
        {
            if (File.Exists(Path.Combine(listDir[SelWin].DirHome, com)))
            {
                File.Delete(Path.Combine(listDir[SelWin].DirHome, com));
            }
            else if (Directory.Exists(Path.Combine(listDir[SelWin].DirHome, com)))
            {
                Directory.Delete(Path.Combine(listDir[SelWin].DirHome, com));
                if (listDir[SelWin].dirInfo.Length == 1 && listDir[SelWin].fileInfo.Length == 0)
                {
                    listDir[SelWin].DirHome = listDir[SelWin].DInfo.Parent.FullName;
                    listDir[SelWin].DInfo = new DirectoryInfo(listDir[SelWin].DirHome);
                }
            }
            DeliteWindows(SelWin);
            StartWinMeneger();
        }
        /// <summary>
        /// удаление не пустой директории
        /// </summary>
        /// <param name="com"></param>
        void Rm_D(string com)
        {
            string[] fileinfo = Directory.GetFiles(com);
            foreach (string Dfile in fileinfo)
            {
                File.Delete(Dfile);
            }
            string[] dirinfo = Directory.GetDirectories(com);
            foreach (string Ddir in dirinfo)
            {
                Rm_D(Ddir);
            }
            Directory.Delete(Path.Combine(listDir[SelWin].DirHome, com));
        }
        /// <summary>
        /// копирование 
        /// </summary>
        /// <param name="fromDir">откуда</param>
        /// <param name="ToDir">куда</param>
        void CP(string fromDir, string ToDir)
        {
            if (Directory.Exists(fromDir))
            {
                ToDir = Path.Combine(ToDir, fromDir.Split('\\').Last());
                Directory.CreateDirectory(ToDir);
                foreach (string str in Directory.GetFiles(fromDir))
                {
                    File.Copy(str, Path.Combine(ToDir, Path.GetFileName(str)));
                }
                foreach (string str in Directory.GetDirectories(fromDir))
                {
                    CP(str, Path.Combine(ToDir, Path.GetFileName(str)));
                }
            }
            else if (File.Exists(fromDir))
            {
                File.Copy(fromDir, Path.Combine(ToDir, fromDir.Split('\\').Last()));
            }
        }
    }
}
