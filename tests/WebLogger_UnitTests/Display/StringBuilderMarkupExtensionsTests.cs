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
        [DataRow("<div style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.Div, 0, 0, 0, "blah", 12)]
        [DataRow("<header style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.Header, 0, 0, 0, "blah", 12)]
        [DataRow("<p style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.Paragraph, 0, 0, 0, "blah", 12)]
        [DataRow("<h1 style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.H1, 0, 0, 0, "blah", 12)]
        [DataRow("<h2 style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.H2, 0, 0, 0, "blah", 12)]
        [DataRow("<h3 style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.H3, 0, 0, 0, "blah", 12)]
        [DataRow("<h4 style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.H4, 0, 0, 0, "blah", 12)]
        [DataRow("<h5 style=\"color:#000000;font-family:blah;font-size:12px;\">", Element.H5, 0, 0, 0, "blah", 12)]
        [DataRow("<div style=\"color:#000000;font-family:blah;\">", Element.Div, 0, 0, 0, "blah", 0)]
        [DataRow("<header style=\"color:#000000;font-family:blah;\">", Element.Header, 0, 0, 0, "blah", 0)]
        [DataRow("<p style=\"color:#000000;font-family:blah;\">", Element.Paragraph, 0, 0, 0, "blah", 0)]
        [DataRow("<h1 style=\"color:#000000;font-family:blah;\">", Element.H1, 0, 0, 0, "blah", 0)]
        [DataRow("<h2 style=\"color:#000000;font-family:blah;\">", Element.H2, 0, 0, 0, "blah", 0)]
        [DataRow("<h3 style=\"color:#000000;font-family:blah;\">", Element.H3, 0, 0, 0, "blah", 0)]
        [DataRow("<h4 style=\"color:#000000;font-family:blah;\">", Element.H4, 0, 0, 0, "blah", 0)]
        [DataRow("<h5 style=\"color:#000000;font-family:blah;\">", Element.H5, 0, 0, 0, "blah", 0)]
        [DataRow("<div style=\"color:#000000;font-size:32px;\">", Element.Div, 0, 0, 0, "", 32)]
        [DataRow("<header style=\"color:#000000;font-size:32px;\">", Element.Header, 0, 0, 0, "", 32)]
        [DataRow("<p style=\"color:#000000;font-size:12px;\">", Element.Paragraph, 0, 0, 0, "", 12)]
        [DataRow("<h1 style=\"color:#000000;font-size:12px;\">", Element.H1, 0, 0, 0, "", 12)]
        [DataRow("<h2 style=\"color:#000000;font-size:12px;\">", Element.H2, 0, 0, 0, "", 12)]
        [DataRow("<h3 style=\"color:#000000;font-size:12px;\">", Element.H3, 0, 0, 0, "", 12)]
        [DataRow("<h4 style=\"color:#000000;font-size:12px;\">", Element.H4, 0, 0, 0, "", 12)]
        [DataRow("<h5 style=\"color:#000000;font-size:12px;\">", Element.H5, 0, 0, 0, "", 12)]
        public void AppendOpener_ReturnsElement_WithOptions(string expected, Element element, int r, int g, int b, string font, int size)
        {
            var builder = new StringBuilder();

            var options = new HtmlElementOptions(Color.FromArgb(r, g, b), fontFamily: font, fontSize: size);

            builder.AppendOpener(element, options);

            Assert.AreEqual(expected, builder.ToString());
        }

        [TestMethod]
        [DataRow("<div style=\"font-family:blah;font-size:12px;\">", Element.Div, "blah", 12)]
        [DataRow("<header style=\"font-family:blah;font-size:20px;\">", Element.Header, "blah", 20)]
        [DataRow("<p style=\"font-family:blah;font-size:24px;\">", Element.Paragraph, "blah", 24)]
        [DataRow("<h1 style=\"font-family:blah;font-size:25px;\">", Element.H1, "blah", 25)]
        [DataRow("<h2 style=\"font-family:blah;font-size:75px;\">", Element.H2, "blah", 75)]
        [DataRow("<h3 style=\"font-family:blah;font-size:85px;\">", Element.H3, "blah", 85)]
        [DataRow("<h5 style=\"font-family:blah;font-size:85px;\">", Element.H5, "blah", 85)]
        [DataRow("<div style=\"font-family:blah;\">", Element.Div, "blah", null)]
        [DataRow("<header style=\"font-family:blah;\">", Element.Header, "blah", 0)]
        [DataRow("<p style=\"font-family:blah;\">", Element.Paragraph, "blah", 0)]
        [DataRow("<h1 style=\"font-family:blah;\">", Element.H1, "blah", 0)]
        [DataRow("<h2 style=\"font-family:blah;\">", Element.H2, "blah", 0)]
        [DataRow("<h3 style=\"font-family:blah;\">", Element.H3, "blah", 0)]
        [DataRow("<h4 style=\"font-family:blah;\">", Element.H4, "blah", 0)]
        [DataRow("<h5 style=\"font-family:blah;\">", Element.H5, "blah", 0)]
        [DataRow("<div style=\"font-family:bling;\">", Element.Div, "bling", null)]
        [DataRow("<header style=\"font-size:50px;\">", Element.Header, "", 50)]
        [DataRow("<p style=\"font-size:85px;\">", Element.Paragraph, "", 85)]
        [DataRow("<h1 style=\"font-size:50px;\">", Element.H1, "", 50)]
        [DataRow("<h2 style=\"font-size:50px;\">", Element.H2, "", 50)]
        [DataRow("<h3 style=\"font-size:50px;\">", Element.H3, "", 50)]
        [DataRow("<h4 style=\"font-size:50px;\">", Element.H4, "", 50)]
        [DataRow("<h5 style=\"font-size:50px;\">", Element.H5, "", 50)]
        public void AppendOpener_ReturnsElement_WithOptionsWithoutColor(string expected, Element element, string font, int size)
        {
            var builder = new StringBuilder();

            var options = new HtmlElementOptions(fontFamily: font, fontSize: size);

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
        [DataRow("style=\"color:#000000;\"", 0, 0, 0, "", null)]
        [DataRow("style=\"color:#FFFFFF;font-size:20px;\"", 255, 255, 255, "", 20)]
        [DataRow("style=\"color:#FFFFFF;font-family:arial;font-size:12px;\"", 255, 255, 255, "arial", 12)]
        [DataRow("style=\"color:#FFFFFF;font-family:seguio;font-size:16px;\"", 255, 255, 255, "seguio", 16)]
        [DataRow("style=\"color:#000000;font-size:85px;\"", 0, 0, 0, "", 85)]
        [DataRow("style=\"color:#000000;font-family:fonts;font-size:125px;\"", 0, 0, 0, "fonts", 125)]
        public void RenderAttributes_CreatesValidAttributes_WithOptions(string expected, int r, int g, int b, string font, int size)
        {
            var options = new HtmlElementOptions(Color.FromArgb(r,g,b), fontFamily: font, fontSize: size);
            var builder = new StringBuilder(string.Empty);

            builder.RenderAttributes(options);
            var value = builder.ToString();
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        [DataRow("style=\"font-family:arial;font-size:20px;\"", "arial", 20)]
        [DataRow("style=\"font-family:seguio;font-size:45px;\"", "seguio", 45)]
        [DataRow("style=\"font-size:55px;\"", "", 55)]
        [DataRow("style=\"font-family:fonts;\"", "fonts", null)]
        public void RenderAttributes_CreatesValidAttributes_WithOptionsWithoutColor(string expected, string font, int size)
        {
            var options = new HtmlElementOptions(fontFamily: font, fontSize: size);
            var builder = new StringBuilder(string.Empty);

            builder.RenderAttributes(options);
            var value = builder.ToString();
            Assert.AreEqual(expected, value);
        }


    }
}
