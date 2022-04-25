using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Core.Extensions
{
    public static class ExtensionMethods
    {
        public static T[] ToArray<T>(this ICollection<T> collection, Int32 index = 0)
        {
            lock (collection)
            {
                var arr = new T[collection.Count - index];
                collection.CopyTo(arr, index);
                return arr;
            }
        }
    }
}
