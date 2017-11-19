using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HtmlTag
    {
        public readonly int Index;
        public static readonly string MDName;
        public static readonly string HtmlName; 

        public static bool IsOpenTag(string text, int index)
        {
            return false;
        }

        public static bool IsCLoseTag(string text, int index)
        {
            return false;
        }
    }

    public class ItalicTag : HtmlTag
    {
        public new readonly string MDName = "_";
        public new readonly string HtmlName = "em";
    }
}
