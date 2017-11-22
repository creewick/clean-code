using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
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
