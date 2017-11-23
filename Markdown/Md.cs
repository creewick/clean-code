using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return !text.IsPrevSpace(tag) &&
                   !text.IsEscaped(tag) &&
                   !text.IsDigitNear(tag);
        }

	    private static bool CorrectOpenTag(Tag tag, string text)
	    {
	        return !text.IsNextSpace(tag) &&
                   !text.IsEscaped(tag) &&
                   !text.IsDigitNear(tag);
        }

	    private static bool EquivalentToLastTag(Stack<Tag> stack, Tag tag)
	    {
	        return stack.Count > 0 && stack.Peek().Markdown == tag.Markdown;
	    }

        private static readonly Dictionary<string, List<string>> AllTags = new Dictionary<string, List<string>>
        {
            { "__", new List<string> {"<strong>", "</strong>" }},
            { "_", new List<string> {"<em>", "</em>" }}
        };

        private static Tag FindTag(string text, int index)
        {
            return AllTags.Keys
                .Where(key => text.SubstringMatch(index, key))
                .Select(key => new Tag(index, key, AllTags[key][0], AllTags[key][1]))
                .FirstOrDefault();
	    }

	    private static string ReplaceMdTagsToHtml(string markdown, List<Tag> orderedTags)
	    {
            var result = new StringBuilder();
	        var index = 0;
	        foreach (var tag in orderedTags)
	        {
	            result.Append(markdown.EscapedSubstring(index, tag.Index - index));
	            result.Append(tag.IsOpen ? tag.HtmlOpen : tag.HtmlClose);
	            index = tag.Index + tag.Markdown.Length;
	        }
	        result.Append(markdown.Substring(index));
	        return result.ToString();
	    }
	}

    public static class StringExtensions
    {
        public static string EscapedSubstring(this string text, int index, int length)
        {
            var result = new StringBuilder();
            var i = index;
            while (i < index + length)
            {
                if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\\')
                {
                    result.Append('\\');
                    i += 2;
                }
                else
                {
                    result.Append(text[i]);
                    i++;
                }
            }
            return result.ToString();
        }

        public static bool SubstringMatch(this string text, int index, string expected)
        {
            return index + expected.Length <= text.Length &&
                   text.Substring(index, expected.Length) == expected;
        }

        public static bool IsPrevSpace(this string text, Tag tag)
        {
            return tag.Index > 0 && text[tag.Index - 1] == ' ';
        }

        public static bool IsNextSpace(this string text, Tag tag)
        {
            var nextIndex = tag.Index + tag.Markdown.Length;
            return nextIndex < text.Length && text[nextIndex] == ' ';
        }

        public static bool IsEscaped(this string text, Tag tag)
        {
            return tag.Index > 0 && text[tag.Index - 1] == '\\' && 
                (tag.Index < 2 || text[tag.Index - 2] != '\\');
        }

        public static bool IsDigitNear(this string text, Tag tag)
        {
            var nextIndex = tag.Index + tag.Markdown.Length;
            return ( tag.Index > 0 && char.IsDigit(text[tag.Index - 1]) ) ||
                   ( nextIndex < text.Length && char.IsDigit(text[nextIndex]) );
        }
    }
}