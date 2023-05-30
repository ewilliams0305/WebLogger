using System.Reflection;
using WebLogger.Utilities;

namespace WebLogger_UnitTests.Utilities
{
    [TestClass]
    public class CommandDiscoveryTests
    {
        [TestMethod]
        public void CommandDiscovery_FindsAllCommands_InAssembly()
        {
            var logger = new MockedLogger();
            logger.DiscoverCommands(Assembly.GetExecutingAssembly());
            logger.Start();

            var doResult = logger.ExecuteCommand("DO", out var doResponse);
            var otherResult = logger.ExecuteCommand("DO", out var otherResponse);
            
            Assert.IsTrue(doResult);
            Assert.IsNotNull(doResponse);
            Assert.AreEqual(doResponse, "The work was done");

            Assert.IsTrue(otherResult);
            Assert.IsNotNull(otherResponse);
            Assert.AreEqual(otherResponse, "The work was done");
        }
        
        [TestMethod]
        public void CommandDiscovery_RejectsInvalidCommands_InAssembly()
        {
            var logger = new MockedLogger();
            logger.DiscoverCommands(Assembly.GetExecutingAssembly());
            logger.Start();

            var invalid = logger.ExecuteCommand("INVALID", out var invalidResult);

            Assert.IsFalse(invalid);
            Assert.IsTrue(string.IsNullOrEmpty(invalidResult));
        }


        [TestMethod]
        public void CommandDiscovery_Throw_ArgumentNullException()
        {
            var logger = new MockedLogger();

            try
            {
                logger.DiscoverCommands(null);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentNullException));
            }
        }
    }
}