using System.Drawing;
using System.Net.Sockets;
using System.Text;
using WebLogger.Display;

namespace WebLogger_UnitTests.Display
{
    [TestClass]
    public class StringBuilderMarkupExtensionsTests
    {

        [TestMethod]
        [DataRow("<div>", Element.Div)]
        [DataRow("<header>", Element.Header)]
        [DataRow("<p>", Element.Paragraph)]
        [DataRow("<h1>", Element.H1)]
        [DataRow("<h2>", Element.H2)]
        [DataRow("<h3>", Element.H3)]
        [DataRow("<h4>", Element.H4)]
        [DataRow("<h5>", Element.H5)]
        public void AppendOpener_ReturnsElement_WithoutOptions(string expected, Element element)
        {
            var builder = new StringBuilder();

            builder.AppendOpener(element);
            
            Assert.AreEqual(expected, builder.ToString());
        }

        [TestMethod]
        [DataRow("<div color=\"#000000\" font=\"blah\" style=\"bling\">", Element.Div, 0, 0, 0, "blah", "bling")]
        [DataRow("<header color=\"#000000\" font=\"blah\" style=\"blam\">", Element.Header, 0, 0, 0, "blah", "blam")]
        [DataRow("<p color=\"#000000\" font=\"blah\" style=\"bling\">", Element.Paragraph, 0, 0, 0, "blah", "bling")]
        [DataRow("<h1 color=\"#000000\" font=\"blah\" style=\"bling\">", Element.H1, 0, 0, 0, "blah", "bling")]
        [DataRow("<h2 color=\"#000000\" font=\"blah\" style=\"bling\">", Element.H2, 0, 0, 0, "blah", "bling")]
        [DataRow("<h3 color=\"#000000\" font=\"blah\" style=\"bling\">", Element.H3, 0, 0, 0, "blah", "bling")]
        [DataRow("<h4 color=\"#000000\" font=\"blah\" style=\"bling\">", Element.H4, 0, 0, 0, "blah", "bling")]
        [DataRow("<h5 color=\"#000000\" font=\"blah\" style=\"bling\">", Element.H5, 0, 0, 0, "blah", "bling")]
        [DataRow("<div color=\"#000000\" font=\"blah\">", Element.Div, 0, 0, 0, "blah", "")]
        [DataRow("<header color=\"#000000\" font=\"blah\">", Element.Header, 0, 0, 0, "blah", "")]
        [DataRow("<p color=\"#000000\" font=\"blah\">", Element.Paragraph, 0, 0, 0, "blah", "")]
        [DataRow("<h1 color=\"#000000\" font=\"blah\">", Element.H1, 0, 0, 0, "blah", "")]
        [DataRow("<h2 color=\"#000000\" font=\"blah\">", Element.H2, 0, 0, 0, "blah", "")]
        [DataRow("<h3 color=\"#000000\" font=\"blah\">", Element.H3, 0, 0, 0, "blah", "")]
        [DataRow("<h4 color=\"#000000\" font=\"blah\">", Element.H4, 0, 0, 0, "blah", "")]
        [DataRow("<h5 color=\"#000000\" font=\"blah\">", Element.H5, 0, 0, 0, "blah", "")]
        [DataRow("<div color=\"#000000\" style=\"bling\">", Element.Div, 0, 0, 0, "", "bling")]
        [DataRow("<header color=\"#000000\" style=\"blam\">", Element.Header, 0, 0, 0, "", "blam")]
        [DataRow("<p color=\"#000000\" style=\"bling\">", Element.Paragraph, 0, 0, 0, "", "bling")]
        [DataRow("<h1 color=\"#000000\" style=\"bling\">", Element.H1, 0, 0, 0, "", "bling")]
        [DataRow("<h2 color=\"#000000\" style=\"bling\">", Element.H2, 0, 0, 0, "", "bling")]
        [DataRow("<h3 color=\"#000000\" style=\"bling\">", Element.H3, 0, 0, 0, "", "bling")]
        [DataRow("<h4 color=\"#000000\" style=\"bling\">", Element.H4, 0, 0, 0, "", "bling")]
        [DataRow("<h5 color=\"#000000\" style=\"bling\">", Element.H5, 0, 0, 0, "", "bling")]
        public void AppendOpener_ReturnsElement_WithOptions(string expected, Element element, int r, int g, int b, string font, string style)
        {
            var builder = new StringBuilder();

            var options = new HtmlElementOptions(Color.FromArgb(r, g, b), font: font, style: style);

            builder.AppendOpener(element, options);

            Assert.AreEqual(expected, builder.ToString());
        }

        [TestMethod]
        [DataRow("<div font=\"blah\" style=\"bling\">", Element.Div, "blah", "bling")]
        [DataRow("<header font=\"blah\" style=\"blam\">", Element.Header, "blah", "blam")]
        [DataRow("<p font=\"blah\" style=\"bling\">", Element.Paragraph, "blah", "bling")]
        [DataRow("<h1 font=\"blah\" style=\"bling\">", Element.H1, "blah", "bling")]
        [DataRow("<h2 font=\"blah\" style=\"bling\">", Element.H2, "blah", "bling")]
        [DataRow("<h3 font=\"blah\" style=\"bling\">", Element.H3, "blah", "bling")]
        [DataRow("<h4 font=\"blah\" style=\"bling\">", Element.H4, "blah", "bling")]
        [DataRow("<h5 font=\"blah\" style=\"bling\">", Element.H5, "blah", "bling")]
        [DataRow("<div font=\"blah\">", Element.Div, "blah", "")]
        [DataRow("<header font=\"blah\">", Element.Header, "blah", "")]
        [DataRow("<p font=\"blah\">", Element.Paragraph, "blah", "")]
        [DataRow("<h1 font=\"blah\">", Element.H1, "blah", "")]
        [DataRow("<h2 font=\"blah\">", Element.H2, "blah", "")]
        [DataRow("<h3 font=\"blah\">", Element.H3, "blah", "")]
        [DataRow("<h4 font=\"blah\">", Element.H4, "blah", "")]
        [DataRow("<h5 font=\"blah\">", Element.H5, "blah", "")]
        [DataRow("<div style=\"bling\">", Element.Div, "", "bling")]
        [DataRow("<header style=\"blam\">", Element.Header, "", "blam")]
        [DataRow("<p style=\"bling\">", Element.Paragraph, "", "bling")]
        [DataRow("<h1 style=\"bling\">", Element.H1, "", "bling")]
        [DataRow("<h2 style=\"bling\">", Element.H2, "", "bling")]
        [DataRow("<h3 style=\"bling\">", Element.H3, "", "bling")]
        [DataRow("<h4 style=\"bling\">", Element.H4, "", "bling")]
        [DataRow("<h5 style=\"bling\">", Element.H5, "", "bling")]
        public void AppendOpener_ReturnsElement_WithOptionsWithoutColor(string expected, Element element, string font, string style)
        {
            var builder = new StringBuilder();

            var options = new HtmlElementOptions(font: font, style: style);

            builder.AppendOpener(element, options);

            Assert.AreEqual(expected, builder.ToString());
        }

        [TestMethod]
        [DataRow("</div>", Element.Div)]
        [DataRow("</header>", Element.Header)]
        [DataRow("</p>", Element.Paragraph)]
        [DataRow("</h1>", Element.H1)]
        [DataRow("</h2>", Element.H2)]
        [DataRow("</h3>", Element.H3)]
        [DataRow("</h4>", Element.H4)]
        [DataRow("</h5>", Element.H5)]
        public void AppendClosure_ReturnsElement(string expected, Element element)
        {
            var builder = new StringBuilder();

            builder.AppendCloser(element);

            Assert.AreEqual(expected, builder.ToString());
        }

        [TestMethod]
        public void RenderColor_ReturnsEmpty_WithDefaultColor()
        {
            Color color = default;
            var builder = new StringBuilder();

            builder.RenderColor(color);

            var value = builder.ToString();

            Assert.AreEqual(string.Empty, value);
        }

        [TestMethod]
        [DataRow("#000000", 0, 0, 0)]
        [DataRow("#125266", 18, 82, 102)]
        [DataRow("#745698", 116, 86, 152)]
        [DataRow("#FFFFFF", 255, 255, 255)]
        public void RenderColor_ReturnsColorText_MatchingRGBValues(string expected, int r, int g, int b)
        {
            var color = Color.FromArgb(r, g, b);
            var builder = new StringBuilder();

            builder.RenderColor(color);

            var value = builder.ToString();

            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void RenderAttributes_ReturnsEmpty_WithoutOptions()
        {
            var builder = new StringBuilder(string.Empty);

            builder.RenderAttributes();
            var value = builder.ToString();
            Assert.AreEqual(string.Empty, value);
        }
        
        [TestMethod]
        [DataRow("color=\"#000000\"", 0, 0, 0, "", "")]
        [DataRow("color=\"#FFFFFF\"", 255, 255, 255, "", "")]
        [DataRow("color=\"#FFFFFF\" font=\"arial\" style=\"ux\"", 255, 255, 255, "arial", "ux")]
        [DataRow("color=\"#FFFFFF\" font=\"seguio\" style=\"styles\"", 255, 255, 255, "seguio", "styles")]
        [DataRow("color=\"#000000\" style=\"styles\"", 0, 0, 0, "", "styles")]
        [DataRow("color=\"#000000\" font=\"fonts\"", 0, 0, 0, "fonts", "")]
        public void RenderAttributes_CreatesValidAttributes_WithOptions(string expected, int r, int g, int b, string font, string style)
        {
            var options = new HtmlElementOptions(Color.FromArgb(r,g,b), font, style);
                var builder = new StringBuilder(string.Empty);

            builder.RenderAttributes(options);
            var value = builder.ToString();
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        [DataRow("font=\"arial\" style=\"ux\"", "arial", "ux")]
        [DataRow("font=\"seguio\" style=\"styles\"", "seguio", "styles")]
        [DataRow("style=\"styles\"","", "styles")]
        [DataRow("font=\"fonts\"", "fonts", "")]
        public void RenderAttributes_CreatesValidAttributes_WithOptionsWithoutColor(string expected, string font, string style)
        {
            var options = new HtmlElementOptions(font: font, style: style);
                var builder = new StringBuilder(string.Empty);

            builder.RenderAttributes(options);
            var value = builder.ToString();
            Assert.AreEqual(expected, value);
        }


    }
}
