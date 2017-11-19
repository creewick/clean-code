using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
		    var tags = GetOrderedTagsList(markdown);
		    return ReplaceMdTagsToHtml(markdown, tags);
		}

	    private static List<HtmlTag> GetOrderedTagsList(string markdown)
	    {
	        var openingTags = new Stack<HtmlTag>();
	        var tags = new List<HtmlTag>();
	        for (var i = 0; i < markdown.Length; i++)
	        {
	            // ...
	        }
	        return tags.OrderBy(a => a.Index).ToList();
	    }

	    private static string ReplaceMdTagsToHtml(string markdown, List<HtmlTag> orderedTags)
	    {
            var result = new StringBuilder();
            // ...
	        return result.ToString();
	    }
	}

	[TestFixture]
	public class Md_ShouldRender
	{
        
	}
}