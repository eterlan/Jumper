using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
    public static class CollectionsEx
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
                action(obj);
            return source;
        }
    }
}