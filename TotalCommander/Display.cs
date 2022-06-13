using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander
{
    /// <summary>
    /// ллас для отрисовки рамки
    /// </summary>
    internal class Display : Program
    {
        /// <summary>
        /// метод для отрисовки рамки
        /// </summary>
        /// <param name="Height">высота</param>
        /// <param name="Width">ширина</param>
        public void Print(int Height, int Width)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            PrintConsole(0, 0, '╔');
            PrintConsole(Width - 1, 0, '╗');
            for (int i = 0; i < Width - 2; i++)
            {
                PrintConsole(i + 1, 0, '═');
                PrintConsole(i + 1, Line1_1, '═');
                PrintConsole(i + 1, Line1_2, '═');
                PrintConsole(i + 1, Line2_1, '═');
                PrintConsole(i + 1, Line2_2, '═');
                PrintConsole(i + 1, Height - 1, '═');
            }
            for (int i = 1; i < Height - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
                Console.SetCursorPosition(Width - 1, i);
                Console.Write("║");
            }

            PrintConsole(0, Height - 1, '╚');
            PrintConsole(Width - 1, Height - 1, '╝');
            PrintConsole(0, Line1_1, '╚');
            PrintConsole(0, Line2_1, '╚');
            PrintConsole(Width - 1, Line1_1, '╝');
            PrintConsole(Width - 1, Line2_1, '╝');
            PrintConsole(0, Line1_2, '╔');
            PrintConsole(0, Line2_2, '╔');
            PrintConsole(Width - 1, Line1_2, '╗');
            PrintConsole(Width - 1, Line2_2, '╗');

            Console.ForegroundColor = ConsoleColor.White;
        }
        void PrintConsole(int x, int y, char znak)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(znak);
        }
    }
}
