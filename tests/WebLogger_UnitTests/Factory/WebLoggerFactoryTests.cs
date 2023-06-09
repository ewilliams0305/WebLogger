using System.Drawing;
using System.Text;
using WebLogger.Render;

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

        [TestMethod]
        public void Factory_Return_DefaultWebLogger_WithCommandCollection()
        {
            var commands = new List<IWebLoggerCommand>()
            {
                new WebLoggerCommand(
                    (cmd, args) => CommandResponse.Success("EXAMPLE", $"{cmd} Received"),
                    "EXAMPLE",
                    "Simple example of console command",
                    "Parameter: NA"),

                new WebLoggerCommand(
                    (cmd, args) => CommandResponse.Success("TEST", $"{cmd} Received"),
                    "TEST",
                    "Simple example of console command",
                    "Parameter: NA")
            };

            var logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.Secured = false;
                options.Commands = commands;
            });

            
            Assert.IsNotNull(logger);

            var example = logger.GetHelpInfo("EXAMPLE");
            var test = logger.GetHelpInfo("TEST");

            Assert.AreEqual("Simple example of console command | Parameter: NA", example.Response);
            Assert.AreEqual("Simple example of console command | Parameter: NA", test.Response);
        }

        [TestMethod]
        public void Factory_WithColorOptions_ChangesColors()
        {
            var logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.Colors.ProvideColors(
                    verbose: Color.AliceBlue,
                    information: Color.Aqua,
                    warning: Color.Bisque,
                    error: Color.Azure);
            });

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
}
