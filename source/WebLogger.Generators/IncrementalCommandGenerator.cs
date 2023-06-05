using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Reflection.Metadata;

namespace WebLogger.Generators
{
    /// <summary>
    /// Creates an IWebLogger command from a partial class with a method tagged with the CommandHandler attribute
    /// </summary>
    [Generator(LanguageNames.CSharp)]
    public class IncrementalCommandGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(PostInitializationCallback);

            IncrementalValuesProvider<PartialClassContext> provider = context.SyntaxProvider
                .CreateSyntaxProvider(SyntacticPredicate, SemanticTransform)
                .Where(static ((INamedTypeSymbol, IMethodSymbol, List<string>)? type) => type.HasValue)
                .Select(static ((INamedTypeSymbol, IMethodSymbol, List<string>)? type, CancellationToken _) => TransformType(type!.Value));
            //TODO: Add WithComparer;

            context.RegisterSourceOutput(provider, Execute);
        }

        private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource(Constants.CommandHandlerAttributeFile, Constants.CommandHandlerAttributeValue);
            context.AddSource(Constants.WebLoggerCommandAttributeFile, Constants.WebLoggerCommandAttributeValue);
        }

        /// <summary>
        /// Ensure the node is a method and the node has at least 1 attribute
        /// </summary>
        private static bool SyntacticPredicate(SyntaxNode node, CancellationToken cancellation)
        {
            if (node is not MethodDeclarationSyntax { AttributeLists: { Count: > 0 } } candidate) 
                return false;

            return true;
        }

        /// <summary>
        /// Validate the method, class declaration, and capture metadata
        /// </summary>
        private static (INamedTypeSymbol, IMethodSymbol, List<string>)? SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellation)
        {
            Debug.Assert(context.Node is MethodDeclarationSyntax);

            var methodDeclaration = Unsafe.As<MethodDeclarationSyntax>(context.Node);
            ISymbol methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);

            if (methodSymbol is not IMethodSymbol method)
            {
                return null;
            }

            var candidate = methodDeclaration.GetParent<ClassDeclarationSyntax>();

            if (!ValidateCandidateModifiers(candidate)) 
                return null;

            INamedTypeSymbol commandHandlerAttribute = context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandHandlerAttributeName);

            List<string> parameters = new List<string>();

            foreach (var methodDeclarationAttributeList in methodDeclaration.AttributeLists)
            {
                foreach (var attribute in methodDeclarationAttributeList.Attributes)
                {
                    SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(attribute);
                    ISymbol attSymbol = symbolInfo.Symbol;

                    if (attSymbol is null)
                        return null;

                    if (!SymbolEqualityComparer.Default.Equals(attSymbol.ContainingSymbol, commandHandlerAttribute))
                        return null;

                    if (attribute.ArgumentList != null)
                    {
                        foreach (var argument in attribute.ArgumentList.Arguments)
                        {
                            if (argument.Expression is LiteralExpressionSyntax literal)
                            {
                                parameters.Add(literal.Token.ValueText);
                            }
                        }
                    }
                }
            }

            ISymbol typeSymbol = context.SemanticModel.GetDeclaredSymbol(candidate);

            if (typeSymbol is INamedTypeSymbol type)
            {
                INamedTypeSymbol iWebLoggerCommand =
                    context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandInterface);

                if (!type.Interfaces.Any(@interface =>
                        @interface.OriginalDefinition.Equals(iWebLoggerCommand, SymbolEqualityComparer.Default)))
                {
                    return (type, method, parameters);
                }
            }

            return null;
        }

        private static bool ValidateCandidateModifiers(ClassDeclarationSyntax candidate)
        {
            if (candidate == null)
                return false;

            if(!candidate.Modifiers.Any(SyntaxKind.PartialKeyword))
               return false;

            if(candidate.Modifiers.Any(SyntaxKind.StaticKeyword))
                return false;

            return true;
        }

        /// <summary>
        /// Creates the partial class capture from the provided type, method, and args
        /// </summary>
        private static PartialClassContext TransformType((INamedTypeSymbol PartialType, IMethodSymbol Handler, List<string> Values) type)
        {
            var @namespace = type.PartialType.ContainingNamespace.IsGlobalNamespace
                ? null
                : type.PartialType.ContainingNamespace.ToDisplayString();

            var name = type.PartialType.Name;

            var targetType = type.PartialType.IsReferenceType
                ? $"{type.PartialType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}?"
                : type.PartialType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            var methodTarget = type.Handler.Name;

            return new PartialClassContext(@namespace, name, targetType, methodTarget, type.Values);
        }

        /// <summary>
        /// Stores the gathered information from the partial class.
        /// </summary>
        internal class PartialClassContext
        {
            public string Namespace { get; }
            public string Name { get; }
            public string TargetType { get; }
            public string MethodTarget { get; }
            public List<string> PropertyValues { get; }

            public PartialClassContext(
                string @namespace, string name, string targetType, string methodTarget, List<string> propertyValues)
            {
                Namespace = @namespace;
                Name = name;
                TargetType = targetType;
                MethodTarget = methodTarget;
                PropertyValues = propertyValues;
            }
        }

        /// <summary>
        /// Writes the gathered information to the source code.
        /// </summary>
        /// <param name="context">Production</param>
        /// <param name="subject">Partial class gathered information</param>
        private static void Execute(SourceProductionContext context, PartialClassContext subject)
        {

            var @namespace = $"namespace {subject.Namespace ?? string.Empty}";
            var command = subject.PropertyValues[0] is not null ? subject.PropertyValues[0] : "ERROR" ;
            var description = subject.PropertyValues[0] is not null ? subject.PropertyValues[1] : "ERROR" ;
            var help = subject.PropertyValues[0] is not null ? subject.PropertyValues[2] : "ERROR" ;
            var code = $@"// <auto-generated/>
{@namespace}
{{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""WebLogger"", ""1.1.3"")]
    partial class {subject.Name} : global::WebLogger.IWebLoggerCommand
    {{
        //Code generated by reading the following:
        //Namespace : {@namespace}
        //Name: {subject.Name}
        //TargetType: {subject.TargetType}
        //MethodName: {subject.MethodTarget}

        public string Command => ""{command.RemoveWhiteSpace().ToUpper()}"";
        public string Description => ""{description}"";
        public string Help => ""{help}"";

        public Func<string, List<string>, ICommandResponse> CommandHandler => ExecuteCommand_Generated;

        protected ICommandResponse ExecuteCommand_Generated(string command, List<string> args)
        {{
            try
            {{
                return {subject.MethodTarget}(command, args);
            }}
            catch (Exception e)
            {{
                return CommandResponse.Error(this, e);
            }}
        }}
    }}
}}
";
            var qualifiedName = subject.Namespace is null ? subject.Name : $"{subject.Namespace}.{subject.Name}";
            context.AddSource($"{qualifiedName}.WebLoggerCommand.g.cs", code);
        }
    }
}
