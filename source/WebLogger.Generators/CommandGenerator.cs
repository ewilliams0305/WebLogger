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
    public class CommandGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
            context.RegisterForSyntaxNotifications(() => new CommandHandlerSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {

            if (context.SyntaxReceiver is not CommandHandlerSyntaxReceiver receiver) 
                return;


            foreach (var commandHandler in receiver.Captures)
            {
                var command = CreateCommand(commandHandler);

                context.AddSource(
                    $"{commandHandler.ClassDeclaration.Identifier.Text}.g.cs", 
                    command.GetText(Encoding.UTF8));
            }
        }

        private ClassDeclarationSyntax AddInterfaceImplementation(ClassDeclarationSyntax classDeclaration)
        {
            var newClassDeclaration = classDeclaration.WithBaseList(
                SyntaxFactory.BaseList(
                    SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                        SyntaxFactory.SimpleBaseType(
                            SyntaxFactory.IdentifierName("IWebLoggerCommand")))));

            return newClassDeclaration;
        }

        private CompilationUnitSyntax CreateCommand(CommandHandlerSyntaxReceiver.Capture capture )
        {
            return SyntaxFactory.CompilationUnit()
                .WithMembers(
                    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                        capture.NamespaceDeclaration
                            .WithMembers(
                                SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                                    AddInterfaceImplementation(capture.ClassDeclaration)
                                        .WithMembers(
                                            SyntaxFactory.List<MemberDeclarationSyntax>(
                                                new MemberDeclarationSyntax[]
                                                {
                                                    SyntaxFactory.PropertyDeclaration(
                                                            SyntaxFactory.PredefinedType(
                                                                SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                                            SyntaxFactory.Identifier("Command"))
                                                        .WithModifiers(
                                                            SyntaxFactory.TokenList(
                                                                SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                                                        .WithExpressionBody(
                                                            SyntaxFactory.ArrowExpressionClause(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    SyntaxFactory.Literal(capture.Command))))
                                                        .WithSemicolonToken(
                                                            SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                                    SyntaxFactory.PropertyDeclaration(
                                                            SyntaxFactory.PredefinedType(
                                                                SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                                            SyntaxFactory.Identifier("Description"))
                                                        .WithModifiers(
                                                            SyntaxFactory.TokenList(
                                                                SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                                                        .WithExpressionBody(
                                                            SyntaxFactory.ArrowExpressionClause(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    SyntaxFactory.Literal(capture.Description))))
                                                        .WithSemicolonToken(
                                                            SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                                    SyntaxFactory.PropertyDeclaration(
                                                            SyntaxFactory.PredefinedType(
                                                                SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                                            SyntaxFactory.Identifier("Help"))
                                                        .WithModifiers(
                                                            SyntaxFactory.TokenList(
                                                                SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                                                        .WithExpressionBody(
                                                            SyntaxFactory.ArrowExpressionClause(
                                                                SyntaxFactory.LiteralExpression(
                                                                    SyntaxKind.StringLiteralExpression,
                                                                    SyntaxFactory.Literal(capture.HelpInfo))))
                                                        .WithSemicolonToken(
                                                            SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                                    SyntaxFactory.PropertyDeclaration(
                                                            SyntaxFactory.GenericName(
                                                                    SyntaxFactory.Identifier("Func"))
                                                                .WithTypeArgumentList(
                                                                    SyntaxFactory.TypeArgumentList(
                                                                        SyntaxFactory.SeparatedList<TypeSyntax>(
                                                                            new SyntaxNodeOrToken[]
                                                                            {
                                                                                SyntaxFactory.PredefinedType(
                                                                                    SyntaxFactory.Token(SyntaxKind
                                                                                        .StringKeyword)),
                                                                                SyntaxFactory.Token(SyntaxKind
                                                                                    .CommaToken),
                                                                                SyntaxFactory.GenericName(
                                                                                        SyntaxFactory
                                                                                            .Identifier("List"))
                                                                                    .WithTypeArgumentList(
                                                                                        SyntaxFactory.TypeArgumentList(
                                                                                            SyntaxFactory
                                                                                                .SingletonSeparatedList<
                                                                                                    TypeSyntax>(
                                                                                                    SyntaxFactory
                                                                                                        .PredefinedType(
                                                                                                            SyntaxFactory
                                                                                                                .Token(
                                                                                                                    SyntaxKind
                                                                                                                        .StringKeyword))))),
                                                                                SyntaxFactory.Token(SyntaxKind
                                                                                    .CommaToken),
                                                                                SyntaxFactory.IdentifierName(
                                                                                    "ICommandResponse")
                                                                            }))),
                                                            SyntaxFactory.Identifier("CommandHandler"))
                                                        .WithModifiers(
                                                            SyntaxFactory.TokenList(
                                                                SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                                                        .WithExpressionBody(
                                                            SyntaxFactory.ArrowExpressionClause(
                                                                SyntaxFactory.IdentifierName("ExecuteCommand")))
                                                        .WithSemicolonToken(
                                                            SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                                    SyntaxFactory.MethodDeclaration(
                                                            SyntaxFactory.IdentifierName("ICommandResponse"),
                                                            SyntaxFactory.Identifier("ExecuteCommand"))
                                                        .WithModifiers(
                                                            SyntaxFactory.TokenList(
                                                                SyntaxFactory.Token(SyntaxKind.ProtectedKeyword)))
                                                        .WithParameterList(
                                                            SyntaxFactory.ParameterList(
                                                                SyntaxFactory.SeparatedList<ParameterSyntax>(
                                                                    new SyntaxNodeOrToken[]
                                                                    {
                                                                        SyntaxFactory.Parameter(
                                                                                SyntaxFactory.Identifier("command"))
                                                                            .WithType(
                                                                                SyntaxFactory.PredefinedType(
                                                                                    SyntaxFactory.Token(SyntaxKind
                                                                                        .StringKeyword))),
                                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                        SyntaxFactory.Parameter(
                                                                                SyntaxFactory.Identifier("args"))
                                                                            .WithType(
                                                                                SyntaxFactory.GenericName(
                                                                                        SyntaxFactory
                                                                                            .Identifier("List"))
                                                                                    .WithTypeArgumentList(
                                                                                        SyntaxFactory.TypeArgumentList(
                                                                                            SyntaxFactory
                                                                                                .SingletonSeparatedList<
                                                                                                    TypeSyntax>(
                                                                                                    SyntaxFactory
                                                                                                        .PredefinedType(
                                                                                                            SyntaxFactory
                                                                                                                .Token(
                                                                                                                    SyntaxKind
                                                                                                                        .StringKeyword))))))
                                                                    })))
                                                        .WithBody(
                                                            SyntaxFactory.Block(
                                                                SyntaxFactory.SingletonList<StatementSyntax>(
                                                                    SyntaxFactory.TryStatement(
                                                                            SyntaxFactory
                                                                                .SingletonList<CatchClauseSyntax>(
                                                                                    SyntaxFactory.CatchClause()
                                                                                        .WithDeclaration(
                                                                                            SyntaxFactory
                                                                                                .CatchDeclaration(
                                                                                                    SyntaxFactory
                                                                                                        .IdentifierName(
                                                                                                            "Exception"))
                                                                                                .WithIdentifier(
                                                                                                    SyntaxFactory
                                                                                                        .Identifier(
                                                                                                            "e")))
                                                                                        .WithBlock(
                                                                                            SyntaxFactory.Block(
                                                                                                SyntaxFactory
                                                                                                    .SingletonList<
                                                                                                        StatementSyntax>(
                                                                                                        SyntaxFactory
                                                                                                            .ReturnStatement(
                                                                                                                SyntaxFactory
                                                                                                                    .InvocationExpression(
                                                                                                                        SyntaxFactory
                                                                                                                            .MemberAccessExpression(
                                                                                                                                SyntaxKind
                                                                                                                                    .SimpleMemberAccessExpression,
                                                                                                                                SyntaxFactory
                                                                                                                                    .IdentifierName(
                                                                                                                                        "CommandResponse"),
                                                                                                                                SyntaxFactory
                                                                                                                                    .IdentifierName(
                                                                                                                                        "Error")))
                                                                                                                    .WithArgumentList(
                                                                                                                        SyntaxFactory
                                                                                                                            .ArgumentList(
                                                                                                                                SyntaxFactory
                                                                                                                                    .SeparatedList
                                                                                                                                        <ArgumentSyntax>(
                                                                                                                                            new
                                                                                                                                                SyntaxNodeOrToken
                                                                                                                                                []
                                                                                                                                                {
                                                                                                                                                    SyntaxFactory
                                                                                                                                                        .Argument(
                                                                                                                                                            SyntaxFactory
                                                                                                                                                                .ThisExpression()),
                                                                                                                                                    SyntaxFactory
                                                                                                                                                        .Token(
                                                                                                                                                            SyntaxKind
                                                                                                                                                                .CommaToken),
                                                                                                                                                    SyntaxFactory
                                                                                                                                                        .Argument(
                                                                                                                                                            SyntaxFactory
                                                                                                                                                                .IdentifierName(
                                                                                                                                                                    "e"))
                                                                                                                                                })))))))))
                                                                        .WithBlock(
                                                                            SyntaxFactory.Block(
                                                                                SyntaxFactory
                                                                                    .SingletonList<StatementSyntax>(
                                                                                        SyntaxFactory.ReturnStatement(
                                                                                            SyntaxFactory
                                                                                                .InvocationExpression(
                                                                                                    SyntaxFactory
                                                                                                        .IdentifierName(
                                                                                                            capture.Key))
                                                                                                .WithArgumentList(
                                                                                                    SyntaxFactory
                                                                                                        .ArgumentList(
                                                                                                            SyntaxFactory
                                                                                                                .SeparatedList
                                                                                                                    <ArgumentSyntax>(
                                                                                                                        new
                                                                                                                            SyntaxNodeOrToken[]
                                                                                                                            {
                                                                                                                                SyntaxFactory.Argument(
                                                                                                                                    SyntaxFactory.MemberAccessExpression(
                                                                                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                                                                                        SyntaxFactory.ThisExpression(),
                                                                                                                                        SyntaxFactory.IdentifierName("Command"))),
                                                                                                                                SyntaxFactory
                                                                                                                                    .Token(
                                                                                                                                        SyntaxKind
                                                                                                                                            .CommaToken),
                                                                                                                                SyntaxFactory
                                                                                                                                    .Argument(
                                                                                                                                        SyntaxFactory
                                                                                                                                            .IdentifierName(
                                                                                                                                                "args"))
                                                                                                                            }))))))))))
                                                }))))))
                .NormalizeWhitespace();
        }
    }
}
