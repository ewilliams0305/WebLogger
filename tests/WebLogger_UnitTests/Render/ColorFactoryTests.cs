using System.Drawing;
using System.Text;
using WebLogger.Render;

namespace WebLogger_UnitTests.Render;

[TestClass]
public class ColorFactoryTests
{
    [TestMethod]
    [DataRow("#000000", 0, 0, 0)]
    [DataRow("#FF0000", 255, 0, 0)]
    [DataRow("#FFFF00", 255, 255, 0)]
    [DataRow("#FFFFFF", 255, 255, 255)]
    [DataRow("#FF00FF", 255, 0, 255)]
    [DataRow("#0000FF", 0, 0, 255)]
    public void ProvideColors_ChangesVerboseColor(string color, int r, int g, int b)
    {
        ColorFactory.Instance.ProvideColors(verbose: Color.FromArgb(r, g, b));
        var result = ColorFactory.Instance.GetColor(Severity.Verbose);

        var builder = new StringBuilder();

        builder.RenderColor(result);
        Assert.AreEqual(color, builder.ToString());
    }
    
    [TestMethod]
    [DataRow("#000000", 0, 0, 0)]
    [DataRow("#FF0000", 255, 0, 0)]
    [DataRow("#FFFF00", 255, 255, 0)]
    [DataRow("#FFFFFF", 255, 255, 255)]
    [DataRow("#FF00FF", 255, 0, 255)]
    [DataRow("#0000FF", 0, 0, 255)]
    public void ProvideColors_ChangesInformationColor(string color, int r, int g, int b)
    {
        ColorFactory.Instance.ProvideColors(information: Color.FromArgb(r, g, b));
        var result = ColorFactory.Instance.GetColor(Severity.Information);


        var builder = new StringBuilder();

        builder.RenderColor(result);
        Assert.AreEqual(color, builder.ToString());
    }

    [TestMethod]
    [DataRow("#000000", 0, 0, 0)]
    [DataRow("#FF0000", 255, 0, 0)]
    [DataRow("#FFFF00", 255, 255, 0)]
    [DataRow("#FFFFFF", 255, 255, 255)]
    [DataRow("#FF00FF", 255, 0, 255)]
    [DataRow("#0000FF", 0, 0, 255)]
    public void ProvideColors_ChangesWarningColor(string color, int r, int g, int b)
    {
        ColorFactory.Instance.ProvideColors(warning: Color.FromArgb(r, g, b));
        var result = ColorFactory.Instance.GetColor(Severity.Warning);


        var builder = new StringBuilder();

        builder.RenderColor(result);
        Assert.AreEqual(color, builder.ToString());
    }

    [TestMethod]
    [DataRow("#000000", 0, 0, 0)]
    [DataRow("#FF0000", 255, 0, 0)]
    [DataRow("#FFFF00", 255, 255, 0)]
    [DataRow("#FFFFFF", 255, 255, 255)]
    [DataRow("#FF00FF", 255, 0, 255)]
    [DataRow("#0000FF", 0, 0, 255)]
    public void ProvideColors_ChangesErrorColor(string color, int r, int g, int b)
    {
        ColorFactory.Instance.ProvideColors(error: Color.FromArgb(r, g, b));
        var result = ColorFactory.Instance.GetColor(Severity.Error);


        var builder = new StringBuilder();

        builder.RenderColor(result);
        Assert.AreEqual(color, builder.ToString());
    }

    [TestMethod]
    public void ProvideColors_ChangesAllColor()
    {
        ColorFactory.Instance.ProvideColors(
            verbose: Color.AliceBlue, 
            information: Color.Aqua, 
            warning: Color.Bisque, 
            error: Color.Azure);

        var verbose = ColorFactory.Instance.GetColor(Severity.Verbose);
        var information = ColorFactory.Instance.GetColor(Severity.Information);
        var warning = ColorFactory.Instance.GetColor(Severity.Warning);
        var error = ColorFactory.Instance.GetColor(Severity.Error);

        Assert.AreEqual(Color.AliceBlue, verbose);
        Assert.AreEqual(Color.Aqua, information);
        Assert.AreEqual(Color.Bisque, warning);
        Assert.AreEqual(Color.Azure, error);
    }
}