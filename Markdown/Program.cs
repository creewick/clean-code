using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Markdown
{
	class Program
	{
		static void Main(string[] args)
		{
            Console.WriteLine(Md.RenderToHtml(@"\\_a_"));
            Console.ReadLine();
		}
	}
}
