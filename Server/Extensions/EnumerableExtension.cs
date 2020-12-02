using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UtilityBox.App.Server.Extensions
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var item in array)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(var item in enumeration)
                action(item);
        }
        
        public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
                await func(value);
        }
    }
}