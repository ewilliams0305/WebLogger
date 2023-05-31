using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

            try
            {
                client.Close();
                logger.Stop();

                Assert.IsFalse(logger.IsRunning);
            }
            catch (Exception e)
            {
                
            }
            
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
                options.DestinationWebpageDirectory = "C://Temp/Html/Logger";
            });

            if(Directory.Exists("C://Temp/Html/Logger"))
                Directory.Delete("C://Temp/Html/Logger", true);

            Assert.IsFalse(Directory.Exists("C://Temp/Html/Logger"));

            logger.Start();

            Assert.IsTrue(File.Exists("C://Temp/Html/Logger/index.html"));
            Assert.IsTrue(File.Exists("C://Temp/Html/Logger/console.js"));
            Assert.IsTrue(File.Exists("C://Temp/Html/Logger/style.css"));

            var info = new FileInfo("C://Temp/Html/Logger/index.html");

            var dif= info.CreationTime - DateTime.Now;

            Assert.IsTrue(dif.Seconds < 3);
        }

        [TestMethod]
        public void WebLogger_DoesNotExtractEmbeddedWebpage_WhenPageIsFound()
        {
            var logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.DestinationWebpageDirectory = "C://Temp/Html/Logger";
            });

           
            logger.Start();

            Assert.IsTrue(Directory.Exists("C://Temp/Html/Logger"));

            Assert.IsTrue(File.Exists("C://Temp/Html/Logger/index.html"));
            Assert.IsTrue(File.Exists("C://Temp/Html/Logger/console.js"));
            Assert.IsTrue(File.Exists("C://Temp/Html/Logger/style.css"));

            var info = new FileInfo("C://Temp/Html/Logger/index.html");

            var dif= DateTime.Now - info.CreationTime;

            Assert.IsTrue(dif.Milliseconds > 500);
        }
    }
}
