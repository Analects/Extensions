using System;
using System.Linq;

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException("collection", "collection is null.");
            if (items == null)
                throw new ArgumentNullException("items", "items is null.");

            foreach (var item in items)
                collection.Add(item);
        }

        public static int IndexOf<T>(this IList<T> list, T item, IEqualityComparer<T> comparer)
        {
            if (list == null)
                throw new ArgumentNullException("list", "list is null.");
            if (comparer == null)
                throw new ArgumentNullException("comparer", "comparer is null.");

            for (int i = 0; i < list.Count; i++)
            {
                if (comparer.Equals(item, list[i]))
                    return i;
            }

            return -1;
        }

        public static bool Remove<T>(this IList<T> list, T item, IEqualityComparer<T> comparer)
        {
            if (list == null)
                throw new ArgumentNullException("list", "list is null.");
            if (comparer == null)
                throw new ArgumentNullException("comparer", "comparer is null.");

            var index = list.IndexOf(item, comparer);
            if (index < 0)
                return false;

            list.RemoveAt(index);
            return true;
        }
    }
}