using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Debugger
{
    public static class Debug
    {
        public static Dictionary<string, Line> lines = new Dictionary<string, Line>();

        public static List<string> linesOrder = new List<string>();

        public static void Add(string name, int cursorPos, int linesCount)
        {
            linesOrder.Insert(0, name);
            lines.Add(name, new Line(name, cursorPos, linesCount));
        }

        public static void Render()
        {
            foreach(string name in linesOrder)
            {
                lines[name].Render();
            }
        }
    }
}
