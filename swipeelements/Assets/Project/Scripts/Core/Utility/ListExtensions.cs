using System;
using System.Collections.Generic;

namespace Project.Core.Utility
{
    public static class ListExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> predicate)
        {
            foreach (var item in list)
            {
                predicate(item);
            }
        }
    }
}