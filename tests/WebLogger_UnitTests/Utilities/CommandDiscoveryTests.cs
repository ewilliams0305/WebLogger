using System.Reflection;
using WebLogger.Utilities;
using WebSocketSharp;

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

            var doResult = logger.ExecuteCommand("DO");
            var otherResult = logger.ExecuteCommand("DO");
            
            Assert.IsTrue(doResult.Status == CommandResult.Success);
            Assert.IsNotNull(doResult.Response);
            Assert.AreEqual(doResult.Response, "The work was done");
            
            Assert.IsTrue(otherResult.Status == CommandResult.Success);
            Assert.IsNotNull(otherResult.Response);
            Assert.AreEqual(otherResult.Response, "The work was done");
        }
        
        [TestMethod]
        public void CommandDiscovery_RejectsInvalidCommands_InAssembly()
        {
            var logger = new MockedLogger();
            logger.DiscoverCommands(Assembly.GetExecutingAssembly());
            logger.Start();

            var invalid = logger.ExecuteCommand("INVALID");

            Assert.IsFalse(invalid.Status == CommandResult.Success);
            Assert.IsTrue(string.IsNullOrEmpty(invalid.Response));
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