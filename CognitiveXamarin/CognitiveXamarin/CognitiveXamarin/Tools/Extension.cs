using System;
using System.Collections.Generic;

namespace CognitiveXamarin.Tools
{
    static partial class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}