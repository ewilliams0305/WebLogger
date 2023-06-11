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

}