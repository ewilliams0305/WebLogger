namespace WebLogger_UnitTests.Factory
{
    [TestClass]
    public class WebLoggerFactoryTests
    {
        [TestMethod]
        public void Factory_Return_DefaultWebLogger_WithDefaultOptions()
        {
            var logger = WebLoggerFactory.CreateWebLogger();

            Assert.IsNotNull(logger);
            Assert.AreEqual(ConstantValues.DefaultHtmlDirectory, logger.HtmlDirectory );
            Assert.AreEqual(false, logger.IsSecured );
            Assert.AreEqual(54321, logger.Port );
        }
        
        [TestMethod]
        public void Factory_Return_DefaultWebLogger_WithCustomPort()
        {
            var logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.WebSocketTcpPort = 9092;
            });

            Assert.IsNotNull(logger);
            Assert.AreEqual(ConstantValues.DefaultHtmlDirectory, logger.HtmlDirectory );
            Assert.AreEqual(false, logger.IsSecured );
            Assert.AreEqual(9092, logger.Port );
        }

        [TestMethod]
        public void Factory_Return_DefaultWebLogger_WithCustomDirectory()
        {
            var logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.DestinationWebpageDirectory = "/temp/html";
            });

            Assert.IsNotNull(logger);
            Assert.AreEqual("/temp/html", logger.HtmlDirectory );
            Assert.AreEqual(false, logger.IsSecured );
            Assert.AreEqual(54321, logger.Port );
        }

        [TestMethod]
        public void Factory_Return_DefaultWebLogger_WithSecurity()
        {
            var logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.Secured = true;
            });

            Assert.IsNotNull(logger);
            Assert.AreEqual(ConstantValues.DefaultHtmlDirectory, logger.HtmlDirectory);
            Assert.AreEqual(true, logger.IsSecured);
            Assert.AreEqual(54321, logger.Port);
        }
    }
}
