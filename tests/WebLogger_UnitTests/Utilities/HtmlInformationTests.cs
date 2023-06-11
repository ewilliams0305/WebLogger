using System.Reflection;
using System.Text;
using WebLogger.Utilities;

namespace WebLogger_UnitTests.Utilities;

[TestClass]
public class HtmlInformationTests
{
    [TestMethod]
    public void VersionsAreEqual_ReturnsTrue_WhenVersionsAreEqual()
    {
        var version1 = new Version(1, 2, 3);
        var version2 = new Version(1, 2, 3);

        Assert.IsTrue(HtmlInformation.VersionsAreEqual(version1, version2));
    }
        
    [TestMethod]
    public void VersionsAreEqual_ReturnsFalse_WhenMajorVersionsAreNotEqual()
    {
        var version1 = new Version(1, 2, 3);
        var version2 = new Version(2, 2, 3);

        Assert.IsFalse(HtmlInformation.VersionsAreEqual(version1, version2));
    }

    [TestMethod]
    public void VersionsAreEqual_ReturnsFalse_WhenMinorVersionsAreNotEqual()
    {
        var version1 = new Version(1, 3, 3);
        var version2 = new Version(1, 2, 3);

        Assert.IsFalse(HtmlInformation.VersionsAreEqual(version1, version2));
    }

    [TestMethod]
    public void VersionsAreEqual_ReturnsFalse_WhenBuildVersionsAreNotEqual()
    {
        var version1 = new Version(1, 2, 3);
        var version2 = new Version(1, 2, 1);

        Assert.IsFalse(HtmlInformation.VersionsAreEqual(version1, version2));
    }

    [TestMethod]
    public void VerifyRunningVersionIsSameAsLoadedVersion_ReturnsTrue_WhenVersionsMatch()
    {
        var path = Path.Combine(ConstantValues.DefaultHtmlDirectory, ConstantValues.HtmlInfo);

        if(File.Exists(path))
            File.Delete(path);

        HtmlInformation.CreateInfoFile(ConstantValues.DefaultHtmlDirectory);
        Assert.IsTrue(HtmlInformation.VerifyRunningVersionIsSameAsLoadedVersion(ConstantValues.DefaultHtmlDirectory));
    }

    [TestMethod]
    public void VerifyRunningVersionIsSameAsLoadedVersion_ReturnsFalse_WhenVersionsDontMatch()
    {
        var path = Path.Combine(ConstantValues.DefaultHtmlDirectory, ConstantValues.HtmlInfo);

        if (!Directory.Exists(ConstantValues.DefaultHtmlDirectory))
            Directory.CreateDirectory(ConstantValues.DefaultHtmlDirectory);

        using (var writer = new FileStream(path, FileMode.Create))
        {
            var builder = new StringBuilder("#VERSION:1.0.0");

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());

            writer.Write(bytes, 0, bytes.Length);
        }

        Assert.IsFalse(HtmlInformation.VerifyRunningVersionIsSameAsLoadedVersion(ConstantValues.DefaultHtmlDirectory));
    }

}