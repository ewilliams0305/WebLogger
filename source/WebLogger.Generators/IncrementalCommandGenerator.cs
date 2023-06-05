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
                .Where(static ((INamedTypeSymbol, IMethodSymbol)? type) => type.HasValue)
                .Select(static ((INamedTypeSymbol, IMethodSymbol)? type, CancellationToken _) => TransformType(type!.Value));
            //TODO: Add WithComparer;

            context.RegisterSourceOutput(provider, Execute);
        }

        private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource(Constants.CommandHandlerAttributeFile, Constants.CommandHandlerAttributeValue);
            context.AddSource(Constants.WebLoggerCommandAttributeFile, Constants.WebLoggerCommandAttributeValue);
        }

        private static bool SyntacticPredicate(SyntaxNode node, CancellationToken cancellation)
        {
            if (node is not MethodDeclarationSyntax { AttributeLists: { Count: > 0 } } candidate) 
                return false;

            return true;

            //var parentClass = candidate.GetParent<ClassDeclarationSyntax>();

            //return parentClass != null
            //       && parentClass.Modifiers.Any(SyntaxKind.PartialKeyword)
            //       && !parentClass.Modifiers.Any(SyntaxKind.StaticKeyword);
        }

        private static (INamedTypeSymbol, IMethodSymbol)? SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellation)
        {
            Debug.Assert(context.Node is MethodDeclarationSyntax);

            var methodDeclaration = Unsafe.As<MethodDeclarationSyntax>(context.Node);
            var candidate = methodDeclaration.GetParent<ClassDeclarationSyntax>();

            ISymbol methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
            ISymbol typeSymbol = context.SemanticModel.GetDeclaredSymbol(candidate);

            //if (typeSymbol is IMethodSymbol method)
            //{
            //    //TODO: Might not really need to evaluate this as it already passed the 
            //    INamedTypeSymbol commandHandlerAttribute =
            //        context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandHandlerAttributeName);

            //    if (method.)
            //    {
            //        return type;
            //    }
            //}

            if (typeSymbol is INamedTypeSymbol type)
            {
                INamedTypeSymbol iWebLoggerCommand =
                    context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandInterface);

                if (!type.Interfaces.Any(@interface =>
                        @interface.OriginalDefinition.Equals(iWebLoggerCommand, SymbolEqualityComparer.Default)))
                {
                    if (methodSymbol is IMethodSymbol method)
                    {
                        //TODO: Grab the parameters from the TryGet method below and return a list of the values.
                        
                        
                        return (type, method);
                    }
                }
            }

            return null;
        }
        //private static (INamedTypeSymbol, IEnumerable<string>) SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellation)
        //{
        //    Debug.Assert(context.Node is ClassDeclarationSyntax);

        //    var candidate = Unsafe.As<ClassDeclarationSyntax>(context.Node);

        //    ISymbol symbol = context.SemanticModel.GetDeclaredSymbol(candidate);

        //    if (symbol is INamedTypeSymbol type)
        //    {
        //        INamedTypeSymbol iWebLoggerCommand =
        //            context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandInterface);

        //        //TODO: Might not really need to evaluate this as it already passed the 
        //        INamedTypeSymbol commandHandlerAttribute =
        //            context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandHandlerAttributeName);

        //        if (!type.Interfaces.Any(@interface =>
        //                @interface.OriginalDefinition.Equals(iWebLoggerCommand, SymbolEqualityComparer.Default)))
        //        {
        //            return type;
        //        }
        //    }

        //    return null;
        //}

        private static bool TryGetAttributeInfo(
            MethodDeclarationSyntax candidate, 
            INamedTypeSymbol target,
            SemanticModel semanticModel)
        {
            foreach (var candidateAttributeList in candidate.AttributeLists)
            {
                foreach (var attribute in candidateAttributeList.Attributes)
                {
                    SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(attribute);
                    ISymbol symbol = symbolInfo.Symbol;

                    if (symbol is not null
                        && SymbolEqualityComparer.Default.Equals(symbol.ContainingSymbol, target)
                        && attribute.ArgumentList is { Arguments: { Count: 3 } } argumentList)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private static bool TryGetAttributeValues(ClassDeclarationSyntax candidate, INamedTypeSymbol target, SemanticModel semanticModel, out IEnumerable<string> values)
        {
            var methods = candidate.GetChildrenOfType<MethodDeclarationSyntax>();

            foreach (var methodDeclarationSyntax in methods)
            {
                //TODO: Might not really need to evaluate this as it already passed the 
                INamedTypeSymbol commandHandlerAttribute =
                    semanticModel.Compilation.GetTypeByMetadataName(Constants.CommandHandlerAttributeName);

            }

            foreach (var candidateAttributeList in candidate.AttributeLists)
            {
                foreach (var attribute in candidateAttributeList.Attributes)
                {
                    SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(attribute);
                    ISymbol symbol = symbolInfo.Symbol;

                    if (symbol is not null
                        && SymbolEqualityComparer.Default.Equals(symbol.ContainingSymbol, target)
                        && attribute.ArgumentList is { Arguments: { Count: 3 } } argumentList)
                    {
                        var parameters = new List<string>();

                        foreach (var argument in argumentList.Arguments)
                        {
                            if (argument.Expression is LiteralExpressionSyntax literal)
                            {
                                parameters.Add(literal.Token.ValueText);
                            }
                        }

                        values = parameters;
                        return true;
                    }
                }
            }
            values = new List<string>();
            return false;
        }

        private static PartialClassContext TransformType((INamedTypeSymbol PartialType, IMethodSymbol Handler) type)
        {
            var @namespace = type.PartialType.ContainingNamespace.IsGlobalNamespace
                ? null
                : type.PartialType.ContainingNamespace.ToDisplayString();

            var name = type.PartialType.Name;

            var targetType = type.PartialType.IsReferenceType
                ? $"{type.PartialType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}?"
                : type.PartialType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            var methodTarget = type.Handler.Name;

            return new PartialClassContext(@namespace, name, targetType, methodTarget, new List<string>());
        }


        internal class PartialClassContext
        {
            public string Namespace { get; }
            public string Name { get; }
            public string TargetType { get; }
            public string MethodTarget { get; }
            public IEnumerable<string> PropertyValues { get; }

            public PartialClassContext(
                string @namespace, string name, string targetType, string methodTarget, IEnumerable<string> propertyValues)
            {
                Namespace = @namespace;
                Name = name;
                TargetType = targetType;
                MethodTarget = methodTarget;
                PropertyValues = propertyValues;
            }
        }

        private static void Execute(SourceProductionContext context, PartialClassContext subject)
        {

            var @namespace = $"namespace {subject.Namespace ?? string.Empty}";
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

        public string Command => ""ReplaceMeCommand"";
        public string Description => ""ReplaceMeDescription"";
        public string Help => ""ReplaceMeHelp"";
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
            string qualifiedName = subject.Namespace is null ? subject.Name : $"{subject.Namespace}.{subject.Name}";
            context.AddSource($"{qualifiedName}.WebLoggerCommand.g.cs", code);
        }
    }
}
