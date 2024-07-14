using System.Net.Sockets;

namespace WebLogger_UnitTests.Logger
{
    [TestClass]
    public class WebLoggerTests
    {

        [TestMethod]
        public void WebLogger_Starts_WebSocketServer()
        {
            var logger = WebLoggerFactory.CreateWebLogger();

            logger.Start();
            Assert.IsTrue(logger.IsRunning);

            var client = new TcpClient();
            client.Connect("127.0.0.1", 54321);
            Assert.IsTrue(client.Connected);

            client.Close();
            logger.Stop();
            logger.Dispose();
        }

        [TestMethod]
        public void WebLogger_StartsAndStops_WebSocketServer()
        {
            var logger = WebLoggerFactory.CreateWebLogger();
            logger.Start();
            Assert.IsTrue(logger.IsRunning);

            var client = new TcpClient();
            client.Connect("127.0.0.1", 54321);
            Assert.IsTrue(client.Connected);

            client.Close();
            logger.Stop();
            logger.Dispose();

            Assert.IsFalse(client.Connected);
            Assert.IsFalse(logger.IsRunning);

        }

        [TestMethod]
        public void WebLogger_Dispose_StopsWebSocketServer()
        {
            var logger = WebLoggerFactory.CreateWebLogger();

            logger.Start();
            Assert.IsTrue(logger.IsRunning);

            var client = new TcpClient();
            client.Connect("127.0.0.1", 54321);
            Assert.IsTrue(client.Connected);

            client.Close();
            logger.Dispose();

            Assert.IsFalse(logger.IsRunning);
        }

        [TestMethod]
        public void WebLogger_ExtractsEmbeddedWebpage_WhenNoPageIsFound()
        {
            var logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.DestinationWebpageDirectory = ConstantValues.DefaultHtmlDirectory;
            });

            if(Directory.Exists(ConstantValues.DefaultHtmlDirectory))
                Directory.Delete(ConstantValues.DefaultHtmlDirectory, true);

            Assert.IsFalse(Directory.Exists(ConstantValues.DefaultHtmlDirectory));

            logger.Start();

            Assert.IsTrue(File.Exists(Path.Combine(ConstantValues.DefaultHtmlDirectory, "index.html")));
            Assert.IsTrue(File.Exists(Path.Combine(ConstantValues.DefaultHtmlDirectory, "console.js")));
            Assert.IsTrue(File.Exists(Path.Combine(ConstantValues.DefaultHtmlDirectory, "style.css")));

            var info = new FileInfo(Path.Combine(ConstantValues.DefaultHtmlDirectory, "index.html"));

            var dif= info.CreationTime - DateTime.Now;

            Assert.IsTrue(dif.Seconds < 3);
        }

        //[TestMethod]
        //public void WebLogger_DoesNotExtractEmbeddedWebpage_WhenPageIsFound()
        //{
        //    var logger = WebLoggerFactory.CreateWebLogger(options =>
        //    {
        //        options.DestinationWebpageDirectory = ConstantValues.DefaultHtmlDirectory;
        //    });

           
        //    logger.Start();
        //    Path.Combine(ConstantValues.DefaultHtmlDirectory, "console.js");

        //    Assert.IsTrue(Directory.Exists(ConstantValues.DefaultHtmlDirectory));

        //    Assert.IsTrue(File.Exists(Path.Combine(ConstantValues.DefaultHtmlDirectory, "index.html")));
        //    Assert.IsTrue(File.Exists(Path.Combine(ConstantValues.DefaultHtmlDirectory, "console.js")));
        //    Assert.IsTrue(File.Exists(Path.Combine(ConstantValues.DefaultHtmlDirectory, "style.css")));

        //    var info = new FileInfo(Path.Combine(ConstantValues.DefaultHtmlDirectory, "index.html"));

        //    var dif= DateTime.Now - info.CreationTime;

        //    Assert.IsTrue(dif.Milliseconds > 20);
        //}
    }
}
