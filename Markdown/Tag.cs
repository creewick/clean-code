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
        public readonly string Markdown;
        public readonly string HtmlOpen;
        public readonly string HtmlClose;
        public bool IsOpen;

        public Tag(int index, string markdown, string htmlOpen, string htmlClose)
        {
            Index = index;
            Markdown = markdown;
            HtmlOpen = htmlOpen;
            HtmlClose = htmlClose;
            IsOpen = true;
        }
    }
}
