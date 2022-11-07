using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Debugger
{
    public class Line
    {
        public readonly string name = "";
        public List<string> lines = new List<string>();
        public readonly int lineCount;
        public readonly int lineCursor;

        public Line(string name, int lineCursor, int lineCount)
        {
            this.name = name;
            this.lineCount = lineCount;
            this.lineCursor = lineCursor;
            for(int i = 0; i < lineCount; i++) { lines.Add(""); }
        }
        public void WriteLine(int line, string str)
        {
            if (line < 0) throw new ArgumentException("line > 0 : current " + line);
            if (line >= lineCount) throw new ArgumentException("line < " + lineCount + " : current " + line);
            if (str == null) throw new ArgumentNullException("str");
            lines[line] = str;
        }
        public void Render()
        {
            for (int i = 0; i < lineCount; i++) 
            {
                Console.CursorTop = lineCursor;
                Console.Write(lines[i]);
            }
        }


    }
}
