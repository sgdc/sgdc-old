using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Compiler
{
    internal sealed class SourceControl<T>
    {
        public List<SourceLine> Lines;
        public T Data;

        public SourceControl(string code)
            : this(code, default(T))
        {
        }

        public SourceControl(string code, T data)
            : this(new List<SourceLine>(), data)
        {
            int count = 0;
            int i;
            bool inString = false;
            for (i = 0; i + count < code.Length; count++)
            {
                switch (code[i + count])
                {
                    case '\n':
                        if (!inString)
                        {
                            count++;
                            this.Lines.Add(new SourceLine(code.Substring(i, count).TrimEnd(), this.Lines.Count + 1, 0));
                            i += count;
                            count = 0;
                        }
                        break;
                    case '"':
                        inString = !inString;
                        break;
                }
            }
            if (count > 1)
            {
                this.Lines.Add(new SourceLine(code.Substring(i, count + 1).TrimEnd(), this.Lines.Count + 1, 0));
            }
        }

        public SourceControl(List<SourceLine> lines, T data)
        {
            this.Lines = lines;
            this.Data = data;
        }
    }

    internal sealed class SourceLine
    {
        public string Line;
        public int LineNumber, ColNumber;

        public SourceLine(string line, int num, int col)
        {
            if (line.EndsWith("\r\n"))
            {
                line = line.Substring(0, line.Length - 2);
            }
            else if (line.EndsWith("\n"))
            {
                line = line.Substring(0, line.Length - 1);
            }
            this.Line = line;
            this.LineNumber = num;
            this.ColNumber = col;
        }

        public override string ToString()
        {
            return this.Line;
        }
    }
}
