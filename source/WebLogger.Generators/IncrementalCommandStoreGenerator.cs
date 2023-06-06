using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace WebLogger.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class IncrementalCommandStoreGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(PostInitializationCallback);

            IncrementalValuesProvider<TargetClassCaptureContext> provider = context.SyntaxProvider
                .CreateSyntaxProvider(SyntacticPredicate, SemanticTransform)
                .Where(static ((INamedTypeSymbol, Dictionary<IMethodSymbol, List<string>>)? type) => type.HasValue)
                .Select(static ((INamedTypeSymbol, Dictionary<IMethodSymbol, List<string>>)? type, CancellationToken _) => TransformType(type!.Value))
                .WithComparer(TargetClassCaptureContextComparer.Instance);

            context.RegisterSourceOutput(provider, Execute);
        }

        private static void PostInitializationCallback(IncrementalGeneratorPostInitializationContext context)
        {

            context.AddSource(Constants.CommandStoreAttributeFile, Constants.CommandStoreAttributeValue);
            context.AddSource(Constants.TargetCommandAttributeFile, Constants.TargetCommandAttributeValue);
            context.AddSource(Constants.StoredCommandsInterfaceExtensionsFile, Constants.StoredCommandsInterfaceExtensionsValue);
            context.AddSource(Constants.StoredCommandsInterfaceFile, Constants.StoredCommandsInterfaceValue);
        }

        private static bool SyntacticPredicate(SyntaxNode node, CancellationToken cancellation)
        {
            if (node is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier: { Text: "CommandStore" } } } attribute)
            {
                return false;
            }

            return true;
        }

        private static (INamedTypeSymbol, Dictionary<IMethodSymbol, List<string>>)? SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellation)
        {
            Debug.Assert(context.Node is AttributeSyntax);
            
            var header = Unsafe.As<AttributeSyntax>(context.Node);
            var candidate = header.GetParent<ClassDeclarationSyntax>();

            // 1. Ensure the class is partial but not static
            if (!ValidateCandidateModifiers(candidate))
                return null;

            // 2. Ensure the class does not already implement the command store and ensure the attribute is applied
            ISymbol typeSymbol = context.SemanticModel.GetDeclaredSymbol(candidate);

            if (typeSymbol is not INamedTypeSymbol type) 
                return null;

            INamedTypeSymbol commandStoreInterface =
                context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.StoredCommandsInterfaceName);

            if (type.Interfaces.Any(@interface =>
                    @interface.OriginalDefinition.Equals(commandStoreInterface, SymbolEqualityComparer.Default)))
            {
                return null;
            }

            // 3. Capture the methods and attribute arguments.
            if (!CaptureMethodValues(context, candidate, out var methods)) 
                return null;

            return (type, methods);
        }

        private static bool CaptureMethodValues(
            GeneratorSyntaxContext context, 
            ClassDeclarationSyntax candidate,
            out Dictionary<IMethodSymbol, List<string>> methods)
        {

            methods = new Dictionary<IMethodSymbol, List<string>>();

            foreach (var methodDeclaration in candidate.GetChildrenOfType<MethodDeclarationSyntax>())
            {
                ISymbol methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);

                if (methodSymbol is not IMethodSymbol method)
                {
                    continue;
                }

                INamedTypeSymbol targetMethodAttribute =
                    context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.TargetCommandAttributeName);

                List<string> parameters = new();

                foreach (var methodDeclarationAttributeList in methodDeclaration.AttributeLists)
                {
                    foreach (var attribute in methodDeclarationAttributeList.Attributes)
                    {
                        SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(attribute);
                        ISymbol attSymbol = symbolInfo.Symbol;

                        if (attSymbol is null)
                            return true;

                        if (SymbolEqualityComparer.Default.Equals(attSymbol.ContainingSymbol, targetMethodAttribute))
                        {
                            if (attribute.ArgumentList == null) continue;

                            foreach (var argument in attribute.ArgumentList.Arguments)
                            {
                                if (argument.Expression is LiteralExpressionSyntax literal)
                                {
                                    parameters.Add(literal.Token.ValueText);
                                }
                            }

                            methods.Add(method, new List<string>(parameters));
                        }
                    }
                }
            }

            return methods.Count > 0;
        }


        private static bool ValidateCandidateModifiers(ClassDeclarationSyntax candidate)
        {
            if (candidate == null)
                return false;

            if (!candidate.Modifiers.Any(SyntaxKind.PartialKeyword))
                return false;

            if (candidate.Modifiers.Any(SyntaxKind.StaticKeyword))
                return false;

            return true;
        }

        /// <summary>
        /// Creates the partial class capture from the provided type, method, and args
        /// </summary>
        private static TargetClassCaptureContext TransformType((INamedTypeSymbol PartialType, Dictionary<IMethodSymbol, List<string>> Methods) type)
        {
            var @namespace = type.PartialType.ContainingNamespace.IsGlobalNamespace
                ? null
                : type.PartialType.ContainingNamespace.ToDisplayString();

            var name = type.PartialType.Name;

            var targetMethods = new List<TargetMethodCaptureContext>();

            foreach (var method in type.Methods)
            {
                var methodContext = new TargetMethodCaptureContext(method.Key.Name, method.Value);
                targetMethods.Add(methodContext);
            }

            return new TargetClassCaptureContext(@namespace, name, targetMethods);
        }


        #region LOCAL TYPES

        internal class TargetMethodCaptureContext : IComparable<TargetMethodCaptureContext>, IEquatable<TargetMethodCaptureContext>
        {
            public string MethodTarget { get; }
            public List<string> PropertyValues { get; }
            public TargetMethodCaptureContext(string methodTarget, List<string> propertyValues)
            {
                MethodTarget = methodTarget;
                PropertyValues = propertyValues;
            }

            public int CompareTo(TargetMethodCaptureContext other)
            {
                if (other.MethodTarget != MethodTarget) return -1;

                if (other.PropertyValues.Count != PropertyValues.Count) return -1;

                if (other.PropertyValues[0] != PropertyValues[0]) return -1;
                if (other.PropertyValues[1] != PropertyValues[1]) return -1;
                if (other.PropertyValues[2] != PropertyValues[2]) return -1;

                return 0;
            }

            public bool Equals(TargetMethodCaptureContext other)
            {
                if (other == null) return false;

                if (other.MethodTarget != MethodTarget) return false;

                if (other.PropertyValues.Count != PropertyValues.Count) return false;

                if(other.PropertyValues[0] == null || other.PropertyValues[1] == null || other.PropertyValues[2] == null)
                    return false;

                if (other.PropertyValues[0] != PropertyValues[0]) return false;
                if (other.PropertyValues[1] != PropertyValues[1]) return false;
                if (other.PropertyValues[2] != PropertyValues[2]) return false;

                return true;
            }
        }

        /// <summary>
        /// Stores the gathered information from the partial class.
        /// </summary>
        internal class TargetClassCaptureContext : IComparable<TargetClassCaptureContext>, IEquatable<TargetClassCaptureContext>
        {
            public string Namespace { get; }
            public string Name { get; }
            public List<TargetMethodCaptureContext> TargetMethods { get; }

            public TargetClassCaptureContext(
                string @namespace,
                string name,
                List<TargetMethodCaptureContext> targetMethods)
            {
                Namespace = @namespace;
                Name = name;
                TargetMethods = targetMethods;
            }

            public int CompareTo(TargetClassCaptureContext other)
            {
                if (other.Namespace != Namespace) return -1;

                if (other.Name != Name) return -1;

                if (other.TargetMethods == null) return -1;

                int i = 0;
                foreach (var method in other.TargetMethods)
                {
                    if (TargetMethods[i] == null)
                        return -1;

                    if (!TargetMethods[i].Equals(method))
                        return -1;

                    i++;
                }

                return 0;
            }

            public bool Equals(TargetClassCaptureContext other)
            {
                if (other == null) return false;

                if (other.Namespace != Namespace) return false;

                if(other.Name != Name) return false;
                if (other.TargetMethods == null) return false;

                for (int i = 0; i < TargetMethods.Count; i++)
                {
                    if (!TargetMethods[i].Equals(other.TargetMethods[i])) return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Comparision to prevent extraneous generation. 
        /// </summary>
        internal class TargetClassCaptureContextComparer : IEqualityComparer<TargetClassCaptureContext>
        {
            private static readonly Lazy<TargetClassCaptureContextComparer> _lazy = new(() => new TargetClassCaptureContextComparer());
            public static TargetClassCaptureContextComparer Instance => _lazy.Value;

            public bool Equals(TargetClassCaptureContext x, TargetClassCaptureContext y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;

                return x.Equals(y);
            }

            public int GetHashCode(TargetClassCaptureContext obj)
            {
                unchecked
                {
                    var hashCode = (obj.Namespace != null ? obj.Namespace.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Name != null ? obj.Name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.TargetMethods != null ? obj.TargetMethods.GetHashCode() : 0);

                    return hashCode;
                }
            }
        }

        #endregion

        /// <summary>
        /// Writes the gathered information to the source code.
        /// </summary>
        /// <param name="context">Production</param>
        /// <param name="subject">Partial class gathered information</param>
        private static void Execute(SourceProductionContext context, TargetClassCaptureContext subject)
        {
            var @namespace = $"namespace {subject.Namespace ?? string.Empty}";

            var methods = new StringBuilder();
            var declarations = new StringBuilder();

            foreach (var method in subject.TargetMethods)
            {
                declarations.Append($@"
        private ICommandResponse {method.MethodTarget}_Generated(string command, List<string> args)
        {{
            try
            {{
                return {method.MethodTarget}(command, args);
            }}
            catch (Exception e)
            {{
                return CommandResponse.Error(command, e.Message);
            }}
        }}"
                );
            }

            foreach (var method in subject.TargetMethods)
            {
                methods.Append("                new WebLoggerCommand(").Append(method.MethodTarget).Append("_Generated, ");

                int i = 1;
                foreach (var property in method.PropertyValues)
                {
                    methods.Append("\"").Append(i == 1? property.RemoveWhiteSpace().ToUpper() : property).Append("\"");

                    if (i != 3)
                    {
                        methods.Append(",");
                    }
                    i++;
                }
                methods.AppendLine("),");
            }
            var code = $@"// <auto-generated/> @{DateTime.UtcNow}
{@namespace}
{{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""WebLogger"", ""1.1.4"")]
    partial class {subject.Name} : global::{Constants.StoredCommandsInterfaceName}
    {{
        //Code generated by reading the following:
        //Namespace : {@namespace}
        //Name: {subject.Name}

        private List<IWebLoggerCommand> _commands;

{declarations}

        public IEnumerable<IWebLoggerCommand> GetStoredCommands()
        {{
            _commands = new List<IWebLoggerCommand>()
            {{

{methods}
            }};
            return _commands;
        }}
    }}
}}
";
            var qualifiedName = subject.Namespace is null ? subject.Name : $"{subject.Namespace}.{subject.Name}";
            context.AddSource($"{qualifiedName}.g.cs", code);
        }
    }
}