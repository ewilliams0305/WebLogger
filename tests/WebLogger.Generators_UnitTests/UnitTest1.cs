using Microsoft;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using WebLogger.Generators;
using WebLogger;

namespace WebLogger.Generators_UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            string code = @"
using System;
using WebLogger.Generators;

namespace Tests
{
    public class Empty
    {

    }
}";
        }
    }
}