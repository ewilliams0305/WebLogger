using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebLogger.Generators
{
    /// <summary>
    /// Handles command handler attribute syntax declarations.
    /// Reads the namespace, class, and attribute parameters to build a new partial class.
    /// This new partial class will be the target of the new IWebLoggerCommand.
    /// </summary>
    public class CommandHandlerSyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// Stores the information required to build a new command class for each method.
        /// </summary>
        public List<Capture> Captures { get; } = new();

        /// <summary>
        /// Handles the syntax node
        /// </summary>
        /// <param name="syntaxNode"></param>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax{Identifier:{Text: "CommandHandler" }}} attribute)
            {
                return;
            }

            // Read the values from the attribute.
            // First parameter will always be the command name or key
            // Second will always be description
            // Third will always be help information.
            var attributeValues = new List<string>();

            foreach (var attributeArgument in attribute.ArgumentList.Arguments)
            {
                if (attributeArgument.Expression is LiteralExpressionSyntax literal)
                {
                    attributeValues.Add(literal.Token.ValueText);
                }
            }

            try
            {
                var nameSpace = attribute.GetParent<NamespaceDeclarationSyntax>();
                var classDeclaration = attribute.GetParent<ClassDeclarationSyntax>();
                var method = attribute.GetParent<MethodDeclarationSyntax>();
                var key = method.Identifier.Text;

                Captures.Add(new Capture(
                    key,
                    attributeValues[0],
                    attributeValues[1],
                    attributeValues[2],
                    nameSpace,
                    classDeclaration,
                    method));
            }
            catch (Exception e)
            {
                
            }
            
        }

        /// <summary>
        /// Stores the captured data
        /// </summary>
        public class Capture
        {
            public string Key { get; private set; }
            public string Command { get; private set; }
            public string Description { get; private set; }
            public string HelpInfo { get; private set; }
            public NamespaceDeclarationSyntax NamespaceDeclaration { get; private set; }
            public ClassDeclarationSyntax ClassDeclaration { get; private set; }
            public MethodDeclarationSyntax Method { get; private set; }

            public Capture(string key, string command, string description, string help, NamespaceDeclarationSyntax namespaceDeclaration, ClassDeclarationSyntax classDeclaration, MethodDeclarationSyntax method)
            {
                Key = key;
                Command = command.RemoveWhiteSpace().ToUpper();
                Description = description;
                HelpInfo = help;
                NamespaceDeclaration = namespaceDeclaration;
                ClassDeclaration = classDeclaration;
                Method = method;
            }
        }
    }
}