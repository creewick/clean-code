using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tag
    {
        public readonly int Index;
        public static readonly string MDName;
        public static readonly string HtmlName; 

        public static bool IsOpenTag(string text, int index)
        {
            return false;
        }

        public static bool IsCloseTag(string text, int index)
        {
            return false;
        }

        public static string ReplaceTag(string text)
        {
            return text;
        }
    }

    public class ItalicTag : Tag
    {
        public new readonly string MDName = "_";
        public new readonly string HtmlName = "em";
    }
}
