using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using System.Diagnostics;

namespace WebLogger.Generators
{
    /// <summary>
    /// Creates an IWebLogger command from a partial class with a method tagged with the CommandHandler attribute
    /// </summary>
    [Generator]
    public class IncrementalCommandGenerator : IIncrementalGenerator
    {
        

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(PostInitializationCallback);
        }

        private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource(Constants.CommandHandlerAttributeFile, Constants.CommandHandlerAttributeValue);
        }
    }
}
