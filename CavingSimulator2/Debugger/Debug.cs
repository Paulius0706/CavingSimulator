using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Debugger
{
    public static class Debug
    {
        private static Dictionary<string, Line> lines = new Dictionary<string, Line>();

        private static List<string> linesOrder = new List<string>();
        public static List<string> logs = new List<string>();

        public static void Add(string name, int cursorPos, int linesCount)
        {
            linesOrder.Insert(0, name);
            lines.Add(name, new Line(name, cursorPos, linesCount));
        }
        public static void WriteLine(string name,int line, string str)
        {
            lines[name].WriteLine(line, str);
        }
        public static void WriteLine(string str)
        {
            logs.Add(str);
        }

        public static async void Render()
        {
            foreach(string name in linesOrder)
            {
                lines[name].Render();
            }
            int lastrow = lines.Values.Max(line => line.lineCursor + line.lineCount);
            Console.CursorTop = lastrow;
            Console.CursorLeft = 0;
            Console.WriteLine(new string('=', Console.WindowWidth));
            for(int i=Math.Min(logs.Count-1, 8); i >= 0; i--)
            {
                Console.WriteLine(logs[i]);
            }
        }
    }
}
