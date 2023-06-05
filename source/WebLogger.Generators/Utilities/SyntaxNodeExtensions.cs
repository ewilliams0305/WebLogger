using System;
using System.Collections.Generic;
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
        public static IEnumerable<T> GetChildrenOfType<T>(this SyntaxNode node)
        {
            var children = node.ChildNodes();
            var childrenOfType = new List<T>();

            foreach (var syntaxNode in children)
            {
                if (syntaxNode is T t)
                {
                    childrenOfType.Add(t);
                }
            }

            return childrenOfType;
        }
    }
}