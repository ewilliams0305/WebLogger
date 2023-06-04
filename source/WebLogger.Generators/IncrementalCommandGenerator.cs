using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            var pipeline = context.SyntaxProvider
                .CreateSyntaxProvider(SyntacticPredicate, SemanticTransform)
                ;
        }

        private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource(Constants.CommandHandlerAttributeFile, Constants.CommandHandlerAttributeValue);
        }

        private static bool SyntacticPredicate(SyntaxNode node, CancellationToken cancellation)
        {
            if (node is not MethodDeclarationSyntax { AttributeLists: { Count: > 0 } } candidate) 
                return false;

            var parentClass = candidate.GetParent<ClassDeclarationSyntax>();

            return parentClass != null
                   && parentClass.Modifiers.Any(SyntaxKind.PartialKeyword)
                   && !parentClass.Modifiers.Any(SyntaxKind.StaticKeyword);
        }

        private static INamedTypeSymbol SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellation)
        {
            Debug.Assert(context.Node is ClassDeclarationSyntax);

            var candidate = Unsafe.As<ClassDeclarationSyntax>(context.Node);

            ISymbol symbol = context.SemanticModel.GetDeclaredSymbol(candidate);

            if (symbol is INamedTypeSymbol type)
            {
                INamedTypeSymbol iWebLoggerCommand =
                    context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandInterface);

                INamedTypeSymbol commandHandlerAttribute =
                    context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.CommandHandlerAttributeName);

                if (!type.Interfaces.Any(@interface =>
                        @interface.OriginalDefinition.Equals(iWebLoggerCommand, SymbolEqualityComparer.Default)))
                {
                    return type;
                }
            }

            return null;
        }

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

        private static PartialClassContext TransformType(INamedTypeSymbol type)
        {
            string @namespace = type.ContainingNamespace.IsGlobalNamespace
                ? null
                : type.ContainingNamespace.ToDisplayString();

            string name = type.Name;

            bool isReference = type.IsReferenceType;

            string targetType = isReference
                ? $"{type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}"
                : type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            return new PartialClassContext(@namespace, name, isReference, targetType, new List<string>());
        }


        internal class PartialClassContext
        {
            public string NameSpace { get; }
            public string Name { get; }
            public bool IsReferenceType { get; }
            public string TargetType { get; }
            public IEnumerable<string> PropertyValues { get; }

            public PartialClassContext(
                string nameSpace, string name, bool isReferenceType, string targetType, IEnumerable<string> propertyValues)
            {
                NameSpace = nameSpace;
                Name = name;
                IsReferenceType = isReferenceType;
                TargetType = targetType;
                PropertyValues = propertyValues;
            }
        }
    }
}
