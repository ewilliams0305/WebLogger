using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLogger_UnitTests.Response
{
    [TestClass]
    public class CommandResponseTests
    {
        [TestMethod]
        public void Success_CreatesSuccessResult_FromCommand()
        {
            var response = CommandResponse.Success(new DoWorkCommand(), "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual("DO", response.Command);
            Assert.AreEqual("Result", response.Response);
            Assert.AreEqual(CommandResult.Success, response.Status);
        }
        
        [TestMethod]
        public void Success_CreatesSuccessResult_FromString()
        {
            var response = CommandResponse.Success("DO", "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual("DO", response.Command);
            Assert.AreEqual("Result", response.Response);
            Assert.AreEqual(CommandResult.Success, response.Status);
        }
        
        [TestMethod]
        public void Failure_CreatesFailureResult_FromCommand()
        {
            var response = CommandResponse.Failure(new DoWorkCommand(), "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual("DO", response.Command);
            Assert.AreEqual("Result", response.Response);
            Assert.AreEqual(CommandResult.Failure, response.Status);
        }
        
        [TestMethod]
        public void Failure_CreatesFailureResult_FromString()
        {
            var response = CommandResponse.Failure("DO", "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual("DO", response.Command);
            Assert.AreEqual("Result", response.Response);
            Assert.AreEqual(CommandResult.Failure, response.Status);
        }
        
        [TestMethod]
        public void Error_CreatesErrorResult_FromCommand()
        {
            var response = CommandResponse.Error(new DoWorkCommand(), "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual("DO", response.Command);
            Assert.AreEqual("Result", response.Response);
            Assert.AreEqual(CommandResult.InternalError, response.Status);
        }
        
        [TestMethod]
        public void Error_CreatesErrorResult_FromString()
        {
            var response = CommandResponse.Error("DO", "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual("DO", response.Command);
            Assert.AreEqual("Result", response.Response);
            Assert.AreEqual(CommandResult.InternalError, response.Status);
        }

        [TestMethod]
        public void Error_CreatesErrorResult_FromException()
        {
            var response = CommandResponse.Error(new DoWorkCommand(), new Exception("Exception Message"));

            Assert.IsNotNull(response);
            Assert.AreEqual("DO", response.Command);
            Assert.AreEqual("Exception Message", response.Response);
            Assert.AreEqual(CommandResult.InternalError, response.Status);
        }
        
        [TestMethod]
        public void Success_CreatesSuccessResult_ImplicateOperator_ConvertsToSuccess()
        {
            var response = CommandResponse.Success(new DoWorkCommand(), "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual(CommandResult.Success, response);
        }

        [TestMethod]
        public void Failure_CreatesFailureResult_ImplicateOperator_ConvertsToFail()
        {
            var response = CommandResponse.Failure(new DoWorkCommand(), "Result");

            Assert.IsNotNull(response);
            Assert.AreEqual(CommandResult.Failure, response);
        }

        [TestMethod]
        public void Error_CreateErrorResult_ImplicateOperator_ConvertsToInternalError()
        {
            var response = CommandResponse.Error(new DoWorkCommand(), new Exception());

            Assert.IsNotNull(response);
            Assert.AreEqual(CommandResult.InternalError, response);
        }
    }
}
