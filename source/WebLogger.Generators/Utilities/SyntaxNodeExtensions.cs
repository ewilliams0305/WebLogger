using System;
using Microsoft.CodeAnalysis;

namespace WebLogger.Generators
{
    public static class SyntaxNodeExtensions
    {
        public static T GetParent<T>(this SyntaxNode node)
        {
            var parent = node.Parent;

            while (true)
            {
                if (parent == null)
                {
                    
                    throw new Exception();
                }

                if (parent is T t)
                {
                    return t;
                }

                parent = parent.Parent;
            }
        }
    }
}