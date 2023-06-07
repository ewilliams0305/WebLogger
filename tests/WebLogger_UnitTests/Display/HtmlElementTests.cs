using System.Drawing;
using WebLogger.Display;

namespace WebLogger_UnitTests.Display;

[TestClass]
public class HtmlElementTests
{
    [TestMethod]
    [DataRow("<div></div>", "" , Element.Div)]
    [DataRow("<span></span>", "" , Element.Span)]
    [DataRow("<header></header>", "" , Element.Header)]
    [DataRow("<p></p>", "" , Element.Paragraph)]
    [DataRow("<h1></h1>", "" , Element.H1)]
    [DataRow("<h2></h2>", "" , Element.H2)]
    [DataRow("<h3></h3>", "" , Element.H3)]
    [DataRow("<h4></h4>", "" , Element.H4)]
    [DataRow("<h5></h5>", "" , Element.H5)]
    public void HtmlElement_CreatesValidElement_WithoutOptions(string expected, string innerValue, Element element)
    {
        var html = new HtmlElement(element, innerValue);
        Assert.AreEqual(expected, html);
    }
        
    [TestMethod]
    [DataRow("<div>test</div>", "test" , Element.Div)]
    [DataRow("<span>hello</span>", "hello" , Element.Span)]
    [DataRow("<header>head</header>", "head" , Element.Header)]
    [DataRow("<p>??</p>", "??" , Element.Paragraph)]
    [DataRow("<h1>!@#</h1>", "!@#" , Element.H1)]
    [DataRow("<h2>Test some white space</h2>", "Test some white space", Element.H2)]
    [DataRow("<h3>Big Test</h3>", "Big Test", Element.H3)]
    [DataRow("<h4>Bigger Test</h4>", "Bigger Test", Element.H4)]
    [DataRow("<h5>Hello what happens with \" </h5>", "Hello what happens with \" ", Element.H5)]
    public void HtmlElement_CreatesValidElement_WithInnerValues(string expected, string innerValue, Element element)
    {
        var html = new HtmlElement(element, innerValue);
        Assert.AreEqual(expected, html);
    }
    
    [TestMethod]
    [DataRow("<div color=\"#000000\" font=\"blah\" style=\"bling\">test</div>", "test" , Element.Div, 0,0, 0, "blah", "bling")]
    [DataRow("<h1 color=\"#000000\" style=\"bling\"></h1>", "" , Element.H1, 0, 0, 0, "", "bling")]
    [DataRow("<span color=\"#000000\" font=\"bling\">test</span>", "test" , Element.Span, 0, 0, 0, "bling", "")]
    public void HtmlElement_CreatesValidElement_WithOptions(string expected, string innerValue, Element element, int r, int g, int b, string font, string style)
    {
        var options = new HtmlElementOptions(Color.FromArgb(r, g, b), font: font, style: style);
        var html = new HtmlElement(element, innerValue, options);
        Assert.AreEqual(expected, html);
    }

    [TestMethod]
    [DataRow("<div><h1></h1></div>", Element.Div, Element.H1)]
    [DataRow("<span><span></span></span>", Element.Span, Element.Span)]
    [DataRow("<header><header></header></header>", Element.Header, Element.Header)]
    [DataRow("<p><p></p></p>", Element.Paragraph, Element.Paragraph)]
    [DataRow("<h1><h1></h1></h1>", Element.H1, Element.H1)]
    [DataRow("<h2><h2></h2></h2>", Element.H2, Element.H2)]
    [DataRow("<h3><h3></h3></h3>", Element.H3, Element.H3)]
    [DataRow("<h4><h4></h4></h4>", Element.H4, Element.H4)]
    [DataRow("<h5><h5></h5></h5>", Element.H5, Element.H5)]
    public void HtmlElement_AppendsValidInnerElement_WithoutOptions(string expected, Element outerEl, Element innerEl)
    {
        var outer = new HtmlElement(outerEl);
        var inner = new HtmlElement(innerEl);
        var element = new HtmlElement(outer, inner);

        Assert.AreEqual(expected, element);
    }

    [TestMethod]
    [DataRow("<div color=\"#000000\" font=\"blah\" style=\"bling\"><div></div></div>", Element.Div, Element.Div, 0, 0, 0, "blah", "bling")]
    [DataRow("<div color=\"#FFFFFF\" font=\"blah\"><h1></h1></div>", Element.Div, Element.H1, 255, 255, 255, "blah", "")]
    [DataRow("<span color=\"#FFFFFF\" style=\"bling\"><span></span></span>", Element.Span, Element.Span, 255, 255, 255, "", "bling")]
    public void HtmlElement_AppendsValidInnerElement_WithOptions(string expected, Element outerEl, Element innerEl, int r, int g, int b, string font, string style)
    {
        var options = new HtmlElementOptions(Color.FromArgb(r, g, b), font: font, style: style);

        var outer = new HtmlElement(outerEl, options: options);
        var inner = new HtmlElement(innerEl);
        var element = new HtmlElement(outer, inner);

        Assert.AreEqual(expected, element);
    }
        
    public void HtmlElement_AppendsValidInnerElement_WithOptionsWithoutColor(string expected, Element outerEl, Element innerEl)
    {
        var outer = new HtmlElement(outerEl);
        var inner = new HtmlElement(innerEl);
        var element = new HtmlElement(outer, inner);

        Assert.AreEqual(expected, element);
    }
}