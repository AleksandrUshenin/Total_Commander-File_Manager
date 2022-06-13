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
                        CP(Path.Combine(DirHome, com.Split(' ')[1]), com.Split(' ')[2]);
                        break;
                    case "RM":
                        if (com.Split(' ')[1] == "-D" || com.Split(' ')[1] == "-d")
                        {
                            Rm_Withfile(Path.Combine(DirHome, com.Split(' ').Last()));
                        }
                        else
                        {
                            Rm(com.Split(' ').Last());
                        }
                        DeliteWindows();
                        StartWinMeneger();
                        break;
                    case "MKDIR":
                        Mkdir(com.Split(' ').Last());
                        break;
                    case "TOUCH":
                        Touch(com.Split(' ').Last());
                        break;
                    case "SHD":
                        if (Directory.Exists(DirHome))
                        {
                            Properties.Settings.Default.HomeDirrction = DirHome;
                            Properties.Settings.Default.Save();
                        }
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
            DirHome = com;
            DInfo = new DirectoryInfo(DirHome);
            DeliteWindows();
            StartWinMeneger();
        }
        /// <summary>
        /// создание директории 
        /// </summary>
        /// <param name="com"></param>
        void Mkdir(string com)
        {
            Directory.CreateDirectory(Path.Combine(DirHome, com));
            DeliteWindows();
            StartWinMeneger();
        }
        /// <summary>
        /// создание файла
        /// </summary>
        /// <param name="com"></param>
        void Touch(string com)
        {
            File.Create(Path.Combine(DirHome, com));
            DeliteWindows();
            StartWinMeneger();
        }
        /// <summary>
        /// удаление
        /// </summary>
        /// <param name="com"></param>
        void Rm(string com)
        {
            if (Directory.Exists(Path.Combine(DirHome, com)))
            {
                Directory.Delete(Path.Combine(DirHome, com));
                if (dirInfo.Length == 1 && fileInfo.Length == 0)
                {
                    DirHome = DInfo.Parent.FullName;
                    DInfo = new DirectoryInfo(DirHome);
                }
            }
            else if (File.Exists(Path.Combine(DirHome, com)))
            {
                File.Delete(Path.Combine(DirHome, com));
                if (dirInfo.Length == 0 && fileInfo.Length == 1)
                {
                    DirHome = DInfo.Parent.FullName;
                    DInfo = new DirectoryInfo(DirHome);
                }
            }
        }
        /// <summary>
        /// удаление не пустой директории
        /// </summary>
        /// <param name="com"></param>
        void Rm_Withfile(string com)
        {
            string[] dir_D = Directory.GetDirectories(com);
            string[] file_D = Directory.GetFiles(com);
            foreach (string fi in file_D)
            {
                File.Delete(Path.Combine(com, fi));
            }
            foreach (string di in dir_D)
            {
                Rm_Withfile(Path.Combine(com, di));
            }
            try
            {
                Directory.Delete(com);
            }
            catch
            { }
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
            if (File.Exists(fromDir))
            {
                File.Copy(fromDir, ToDir);
            }
        }
    }
}
