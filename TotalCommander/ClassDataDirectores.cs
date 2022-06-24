using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TotalCommander
{
    internal class ClassDataDirectores
    {
        public string DirHome;
        public DirectoryInfo DInfo;
        public DirectoryInfo[] dirInfo;
        public FileInfo[] fileInfo;
        public int Page;
        public int PageMax;
        public string[] AllDirecroris;

        public void SetDInfo()
        {
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
    }
}
