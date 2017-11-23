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
        public void DoubleEscapeSymbol_Ignore()
        {
            var result = Md.RenderToHtml(@"\\_a_");
            result.Should().Be(@"\<em>a</em>");
        }

        [TestCase("_ab2_", TestName = "CloseTagAfterDigit")]
        [TestCase("_abc_2", TestName = "CloseTagBeforeDigit")]
        [TestCase("1_abc_", TestName = "OpenTagAfterDigit")]
        [TestCase("_1bc_", TestName = "OpenTagBeforeDigit")]
        public void TagsWithDigits_Ignore(string input)
        {
            var result = Md.RenderToHtml(input);
            result.Should().Be(input);
        }

        [Test]
        public void TagsTogether_LongerFirst()
        {
            var result = Md.RenderToHtml(@"___a_bc__");
            result.Should().Be(@"<strong><em>a</em>bc</strong>");
        }

        [Test]
        public void TagsTogether_ShorterFirst_Ignore()
        {
            var result = Md.RenderToHtml(@"___a__bc_");
            result.Should().Be(@"___a__bc_");
        }
    }
}
