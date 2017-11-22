using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Markdown
{
	public class Md
	{
		public static string RenderToHtml(string markdown)
		{
		    var tags = GetOrderedTagsList(markdown);
		    return ReplaceMdTagsToHtml(markdown, tags);
		}

	    private static List<Tag> GetOrderedTagsList(string markdown)
	    {
	        var openTags = new Stack<Tag>();
	        var tags = new List<Tag>();
	        var i = 0;
	        while (i < markdown.Length)
	        {
	            var tag = FindTag(markdown, i);
                i += tag?.Markdown.Length ?? 1;
                if (tag == null) continue;
	            if (!EquivalentToLastTag(openTags, tag))
	            {
	                if (CorrectOpenTag(tag, markdown))
	                    openTags.Push(tag);
	            }
	            else
	            {
	                if (openTags.Count == 0 || 
                        tag.Markdown != openTags.Peek().Markdown || 
                        !CorrectCloseTag(tag, markdown))
                        continue;
	                tag.IsOpen = false;
	                tags.Add(openTags.Pop());
	                tags.Add(tag);
	            }
	        }
	        return tags.OrderBy(a => a.Index).ToList();
	    }

	    private static bool CorrectCloseTag(Tag tag, string text)
	    {
	        var nextIndex = tag.Index + tag.Markdown.Length;
            return tag.Index > 0 && text[tag.Index - 1] != ' ' &&
                    text[tag.Index - 1] != '\\' &&
                    !char.IsDigit(text[tag.Index - 1]) &&
                    !char.IsDigit(text[nextIndex]);
	    }

	    private static bool CorrectOpenTag(Tag tag, string text)
	    {
	        var nextIndex = tag.Index + tag.Markdown.Length;
	        return nextIndex < text.Length && text[nextIndex] != ' ' &&
                   text[tag.Index - 1] != '\\' &&
	               !char.IsDigit(text[tag.Index - 1]) &&
	               !char.IsDigit(text[nextIndex]);
        }

	    private static bool EquivalentToLastTag(Stack<Tag> stack, Tag tag)
	    {
	        return stack.Count > 0 && stack.Peek().Markdown == tag.Markdown;
	    }

        private static readonly Dictionary<string, List<string>> tags = new Dictionary<string, List<string>>
        {
            { "__", new List<string> {"<strong>", "</strong>" }},
            { "_", new List<string> {"<em>", "</em>" }}
        };

        private static Tag FindTag(string text, int index)
        {
            foreach (var tag in tags.Keys)
	            if (text.SubstringMatch(index, tag))
                    return new Tag(index, tag, tags[tag][0], tags[tag][1]);
	        return null;
	    }

	    private static string ReplaceMdTagsToHtml(string markdown, List<Tag> orderedTags)
	    {
            var result = new StringBuilder();
	        var index = 0;
	        foreach (var tag in orderedTags)
	        {
	            result.Append(markdown.Substring(index, tag.Index - index));
	            result.Append(tag.IsOpen ? tag.HtmlOpen : tag.HtmlClose);
	            index = tag.Index + tag.Markdown.Length;
	        }
	        result.Append(markdown.Substring(index));
	        return result.ToString();
	    }
	}

    public static class StringExtensions
    {
        public static bool SubstringMatch(this string text, int index, string expected)
        {
            return index + expected.Length <= text.Length &&
                   text.Substring(index, expected.Length) == expected;
        }
    }

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

	[TestFixture]
	public class Md_ShouldRender
	{
	    [Test]
	    public void Simple_em()
	    {
	        var result = Md.RenderToHtml("_abc_");
	        result.Should().Be("<em>abc</em>");
	    }

	    [Test]
	    public void Simple_strong()
	    {
	        var result = Md.RenderToHtml("__abc__");
	        result.Should().Be("<strong>abc</strong>");
	    }

	    [Test]
	    public void SpaceAfterOpenTag_Ignore()
	    {
	        var result = Md.RenderToHtml("_ abc_");
	        result.Should().Be("_ abc_");
	    }

	    [Test]
	    public void SpaceBeforeCloseTag_Ignore()
	    {
	        var result = Md.RenderToHtml("_abc _");
	        result.Should().Be("_abc _");
	    }

	    [Test]
	    public void EmInsideStrong()
	    {
	        var result = Md.RenderToHtml("__a _b_ c__");
	        result.Should().Be("<strong>a <em>b</em> c</strong>");
        }

	    [Test]
	    public void StrongInsideEm()
	    {
	        var result = Md.RenderToHtml("_a __b__ c_");
	        result.Should().Be("<em>a <strong>b</strong> c</em>");
        }

	    [Test]
	    public void EscapeSymbol()
	    {
	        var result = Md.RenderToHtml(@"\_a_");
	        result.Should().Be(@"\_a_");
        }

	    [Test]
	    public void TagsWithDigits_Ignore()
	    {
	        var result = Md.RenderToHtml("1_2__3__4_");
	        result.Should().Be("1_2__3__4_");
        }
	}
}