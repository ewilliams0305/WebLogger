using System.Drawing;
using System.Text;
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
        Assert.AreEqual(expected, html.Render());
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
        Assert.AreEqual(expected, html.Render());
    }
    
    [TestMethod]
    [DataRow("<div style=\"color:#000000;font-family:verdana;font-size:50px;\">test</div>", "test", Element.Div, 0,0, 0, "verdana", 50)]
    [DataRow("<div style=\"color:#FF0000;font-size:50px;\">test</div>", "test", Element.Div, 255,0, 0, "", 50)]
    [DataRow("<div style=\"color:#000000;font-family:verdana;\">test</div>", "test", Element.Div, 0,0, 0, "verdana", 0)]
    public void HtmlElement_CreatesValidElement_WithOptions(string expected, string innerValue, Element element, int r, int g, int b, string font, int size)
    {
        var options = new HtmlElementOptions(Color.FromArgb(r, g, b), fontFamily: font, fontSize: size);
        var html = new HtmlElement(element, innerValue, options);
        Assert.AreEqual(expected, html.Render());
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

        Assert.AreEqual(expected, element.Render());
    }

    [TestMethod]
    [DataRow("<div style=\"color:#000000;font-family:seguio;font-size:16px;\"><div></div></div>", Element.Div, Element.Div, 0, 0, 0, "seguio", 16)]
    [DataRow("<div style=\"color:#FFFFFF;font-family:seguio;\"><h1></h1></div>", Element.Div, Element.H1, 255, 255, 255, "seguio", null)]
    [DataRow("<span style=\"color:#FFFFFF;font-size:16px;\"><span></span></span>", Element.Span, Element.Span, 255, 255, 255, "", 16)]
    public void HtmlElement_AppendsValidInnerElement_WithOptions(string expected, Element outerEl, Element innerEl, int r, int g, int b, string font, int size)
    {
        var options = new HtmlElementOptions(Color.FromArgb(r, g, b), fontFamily: font, fontSize: size);

        var outer = new HtmlElement(outerEl, options: options);
        var inner = new HtmlElement(innerEl);
        var element = new HtmlElement(outer, inner);

        Assert.AreEqual(expected, element.Render());
    }

    [TestMethod]
    [DataRow("<div style=\"font-family:seguio;font-size:16px;\"><div></div></div>", Element.Div, Element.Div, "seguio", 16)]
    [DataRow("<div style=\"font-family:seguio;font-size:16px;\"><h1></h1></div>", Element.Div, Element.H1, "seguio", 16)]
    [DataRow("<span style=\"font-size:16px;\"><span></span></span>", Element.Span, Element.Span, "", 16)]
    public void HtmlElement_AppendsValidInnerElement_WithOptionsWithoutColor(string expected, Element outerEl, Element innerEl, string font, int size)
    {
        var options = new HtmlElementOptions(fontFamily: font, fontSize: size);

        var outer = new HtmlElement(outerEl, options: options);
        var inner = new HtmlElement(innerEl);
        var element = new HtmlElement(outer, inner);

        Assert.AreEqual(expected, element.Render());
    }

    [TestMethod]
    [DataRow("<div></div>value", "value")]
    [DataRow("<div></div>", "")]
    [DataRow("<div></div>test a space", "test a space")]
    [DataRow("<div></div>test !@#$", "test !@#$")]
    public void Append_AddsStringValue_WhenValueIsProvided(string expected, string value)
    {
        var element = new HtmlElement(Element.Div)
            .Append(new StringBuilder(value));

        Assert.AreEqual(expected, element.Render());
    }
    
    [TestMethod]
    [DataRow("<div></div>value", "value")]
    [DataRow("<div></div>", "")]
    [DataRow("<div></div>test a space", "test a space")]
    [DataRow("<div></div>test !@#$", "test !@#$")]
    public void Append_AddsBuilderValue_WhenValueIsProvided(string expected, string value)
    {
        var builder = new StringBuilder(value);
        var element = new HtmlElement(Element.Div)
            .Append(builder);

        Assert.AreEqual(expected, element.Render());
    }
    
    [TestMethod]
    [DataRow("<div></div><h1></h1>", Element.Div, Element.H1)]
    [DataRow("<div></div><div></div>", Element.Div, Element.Div)]
    [DataRow("<h1></h1><span></span>", Element.H1, Element.Span)]
    public void Append_AddsBuilderValue_WhenValueIsProvided(string expected, Element outer, Element inner)
    {
        var appended = new HtmlElement(inner);

        var element = new HtmlElement(outer)
            .Append(appended);

        Assert.AreEqual(expected, element.Render());
    }

    [TestMethod]
    [DataRow("<div><h1></h1></div>", Element.Div, Element.H1)]
    [DataRow("<h1><span></span></h1>", Element.H1, Element.Span)]
    [DataRow("<span><span></span></span>", Element.Span, Element.Span)]
    public void Insert_InsertsElement_WhenValueIsProvided(string expected, Element outer, Element inner)
    {
        var insert = new HtmlElement(inner);

        var element = new HtmlElement(outer)
            .Insert(insert);

        Assert.AreEqual(expected, element.Render());
    }
}