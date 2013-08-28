using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns true if there are enough items in the IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="number">The number of items to check for.</param>
        /// <returns>True if there are enough items, false otherwise.</returns>
        public static bool AtLeast<T>(this IEnumerable<T> source, int number)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var collection = source as ICollection<T>;
            if (collection != null)
                return collection.Count >= number;

            var collection2 = source as ICollection;
            if (collection2 != null)
                return collection2.Count >= number;

            int count = 0;
            using (IEnumerator<T> data = source.GetEnumerator())
                while (count < number && data.MoveNext())
                {
                    count++;
                }
            return count == number;
        }

        /// <summary>
        /// Returns true if there are enough items in the IEnumerable&lt;T&gt; to satisfy the condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="number">The number of items to check for.</param>
        /// <param name="predicate">The condition to apply to the items.</param>
        /// <returns>True if there are enough items, false otherwise.</returns>
        public static bool AtLeast<T>(this IEnumerable<T> source, int number, Func<T, bool> condition)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (condition == null)
                throw new ArgumentNullException("condition");

            int count = 0;
            using (IEnumerator<T> data = source.GetEnumerator())
                while (count < number && data.MoveNext())
                {
                    if (condition(data.Current))
                        count++;
                }
            return count == number;
        }

        /// <summary>
        /// Returns true if there are no more than a set number of items in the IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="number">The number of items that must exist.</param>
        /// <returns>True if there are no more than the number of items, false otherwise.</returns>
        public static bool AtMost<T>(this IEnumerable<T> source, int number)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var collection = source as ICollection<T>;
            if (collection != null)
                return collection.Count <= number;

            var collection2 = source as ICollection;
            if (collection2 != null)
                return collection2.Count <= number;

            int count = 0;
            using (IEnumerator<T> data = source.GetEnumerator())
                while (count <= number && data.MoveNext())
                {
                    count++;
                }
            return count <= number;
        }

        /// <summary>
        /// Returns true if there are no more than a set number of items in the IEnumerable&lt;T&gt; that satisfy a given condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="number">The number of items that must exist.</param>
        /// <param name="predicate">The condition to apply to the items.</param>
        /// <returns>True if there are no more than the number of items, false otherwise.</returns>
        public static bool AtMost<T>(this IEnumerable<T> source, int number, Func<T, bool> condition)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (condition == null)
                throw new ArgumentNullException("condition");

            int count = 0;
            using (IEnumerator<T> data = source.GetEnumerator())
                while (count <= number && data.MoveNext())
                {
                    if (condition(data.Current))
                        count++;
                }
            return count <= number;
        }

        /// <summary>
        /// Clumps items into same size lots.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="size">The maximum size of the clumps to make.</param>
        /// <returns>A list of list of items, where each list of items is no bigger than the size given.</returns>
        public static IEnumerable<IEnumerable<T>> Clump<T>(this IEnumerable<T> source, int size)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (size < 1)
                throw new ArgumentOutOfRangeException("size", "size is less than 1.");

            return ClumpIterator<T>(source, size);
        }

        /// <summary>
        /// Creates a list by repeating another list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The list to repeat.</param>
        /// <returns>A circular list of items.</returns>
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return CycleIterator<T>(source);
        }

        /// <summary>
        /// Lazily invokes an action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="action">Action to invoke for each element.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return DoIterator<TSource>(source, action);
        }

        /// <summary>
        /// Lazily invokes an action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="action">Action to invoke for each element.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return DoIterator<TSource>(source, action);
        }

        /// <summary>
        /// Returns an empty list if the source is null.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>An empty sequence if the source is null, otherwise the source sequence.</returns>
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                return Enumerable.Empty<TSource>();

            return source;
        }

        /// <summary>
        /// Compares two sequences and returns true if they have the same elements, ignoring order.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="first">The first sequence to compare.</param>
        /// <param name="second">The second sequence to compare.</param>
        /// <returns>True if the sequences contain the same elements.</returns>
        public static bool EnumerableEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "first is null.");
            if (second == null)
                throw new ArgumentNullException("second", "second is null.");

            return EnumerableEqual<TSource>(first, second, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Compares two sequences and returns true if they have the same elements, ignoring order.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="first">The first sequence to compare.</param>
        /// <param name="second">The second sequence to compare.</param>
        /// <param name="comparer">Comparer used for the sequence elements.</param>
        /// <returns>True if the sequences contain the same elements.</returns>
        public static bool EnumerableEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("first", "first is null.");
            if (second == null)
                throw new ArgumentNullException("second", "second is null.");
            if (comparer == null)
                throw new ArgumentNullException("comparer", "comparer is null.");

            var firstStorage = new List<TSource>();
            var secondStorage = new List<TSource>();

            bool firstRunning = true;
            bool secondRunning = true;

            using (var firstEnum = first.GetEnumerator())
            using (var secondEnum = second.GetEnumerator())
            {
                while (firstRunning && secondRunning)
                {
                    firstRunning = firstRunning && firstEnum.MoveNext();

                    if (firstRunning && !secondStorage.Remove(firstEnum.Current, comparer))
                        firstStorage.Add(firstEnum.Current);

                    secondRunning = secondRunning && secondEnum.MoveNext();

                    if (secondRunning && !firstStorage.Remove(secondEnum.Current, comparer))
                        secondStorage.Add(secondEnum.Current);
                }
            }

            return !firstRunning && !secondRunning && firstStorage.Count == 0 && secondStorage.Count == 0;
        }

        /// <summary>
        /// Invokes an action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="action">Action to invoke for each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            using (var data = source.GetEnumerator())
                while (data.MoveNext())
                {
                    action(data.Current);
                }
        }

        /// <summary>
        /// Invokes an action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="action">Action to invoke for each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            int index = 0;
            using (var data = source.GetEnumerator())
                while (data.MoveNext())
                {
                    action(data.Current, index);
                    index++;
                }
        }

        /// <summary>
        /// Creates a list by applying a function to the last item in the list to generate the next item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start">The first item in the list.</param>
        /// <param name="count">The number of items to return.</param>
        /// <param name="iterator">The delegate to generate the next item from.</param>
        /// <returns>A list of generated items.</returns>
        public static IEnumerable<T> Iterate<T>(T start, int count, Func<T, T> iterator)
        {
            if (start == null)
                throw new ArgumentNullException("source");
            if (iterator == null)
                throw new ArgumentNullException("iterator");

            return IterateIterator<T>(start, count, iterator);
        }

        public static TSource OnlyOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            ICollection<TSource> list = source as ICollection<TSource>;
            TSource[] array = source as TSource[];

            if (list != null)
            {
                switch (list.Count)
                {
                    case 0:
                        return default(TSource);

                    case 1:
                        return list.First();
                }
            }
            else if (array != null)
            {
                switch (array.Length)
                {
                    case 0:
                        return default(TSource);

                    case 1:
                        return array[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        return default(TSource);
                    }
                    TSource current = enumerator.Current;
                    if (!enumerator.MoveNext())
                    {
                        return current;
                    }
                }
            }
            return default(TSource);
        }

        public static TSource OnlyOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            TSource local = default(TSource);
            long num = 0L;
            foreach (TSource local2 in source)
            {
                if (predicate(local2))
                {
                    local = local2;
                    num += 1L;
                }
            }
            long num2 = num;
            if ((num2 <= 1L) && (num2 >= 0L))
            {
                switch (((int)num2))
                {
                    case 0:
                        return default(TSource);

                    case 1:
                        return local;
                }
            }
            return default(TSource);
        }

        public static IEnumerable<T> RepeatItems<T>(this IEnumerable<T> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 1)
                throw new ArgumentOutOfRangeException("count", "count is less than 1.");

            return RepeatItemsIterator<T>(source, count);
        }

        /// <summary>
        /// Creates a sequence by applying a delegate to pairs of items from the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">The source list of items.</param>
        /// <param name="zipFunction">The delegate to combine items.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Scan<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> combine)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (combine == null)
                throw new ArgumentNullException("combine");

            return ScanIterator<TSource, TResult>(source, combine);
        }

        /// <summary>
        /// Creates a sequence by applying a delegate to the next item, and the previous delegate result.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">The source sequence of items.</param>
        /// <param name="combine">The delegate to combine items.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Scanl<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> combine)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (combine == null)
                throw new ArgumentNullException("combine");

            return ScanlIterator<TSource>(source, combine);
        }

        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
        {
            return new HashSet<TSource>(source);
        }

        public static ObservableCollection<TSource> ToObservable<TSource>(this IEnumerable<TSource> source)
        {
            return new ObservableCollection<TSource>(source);
        }

        /// <summary>
        /// Creates a list by combining three other lists into one.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source1">One of the lists to zip.</param>
        /// <param name="source2">One of the lists to zip.</param>
        /// <param name="source3">One of the lists to zip.</param>
        /// <param name="combine">The delegate used to combine the items.</param>
        /// <returns>A new list with the combined items.</returns>
        public static IEnumerable<TResult> Zip<T1, T2, T3, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, Func<T1, T2, T3, TResult> combine)
        {
            if (source1 == null)
                throw new ArgumentNullException("source1");
            if (source2 == null)
                throw new ArgumentNullException("source2");
            if (source3 == null)
                throw new ArgumentNullException("source3");
            if (combine == null)
                throw new ArgumentNullException("combine");

            return ZipIterator<T1, T2, T3, TResult>(source1, source2, source3, combine);
        }

        /// <summary>
        /// Creates a list by combining four other lists into one
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source1">One of the lists to zip.</param>
        /// <param name="source2">One of the lists to zip.</param>
        /// <param name="source3">One of the lists to zip.</param>
        /// <param name="source4">One of the lists to zip.</param>
        /// <param name="combine">The delegate used to combine the items.</param>
        /// <returns>A new list with the combined items.</returns>
        public static IEnumerable<TResult> Zip<T1, T2, T3, T4, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4, Func<T1, T2, T3, T4, TResult> combine)
        {
            if (source1 == null)
                throw new ArgumentNullException("source1");
            if (source2 == null)
                throw new ArgumentNullException("source2");
            if (source3 == null)
                throw new ArgumentNullException("source3");
            if (source4 == null)
                throw new ArgumentNullException("source4");
            if (combine == null)
                throw new ArgumentNullException("combine");

            return ZipIterator<T1, T2, T3, T4, TResult>(source1, source2, source3, source4, combine);
        }

        /// <summary>
        /// Creates a list by combining five other lists into one
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source1">One of the lists to zip.</param>
        /// <param name="source2">One of the lists to zip.</param>
        /// <param name="source3">One of the lists to zip.</param>
        /// <param name="source4">One of the lists to zip.</param>
        /// <param name="source5">One of the lists to zip.</param>
        /// <param name="combine">The delegate used to combine the items.</param>
        /// <returns>A new list with the combined items.</returns>
        public static IEnumerable<TResult> Zip<T1, T2, T3, T4, T5, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4, IEnumerable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> combine)
        {
            if (source1 == null)
                throw new ArgumentNullException("source1");
            if (source2 == null)
                throw new ArgumentNullException("source2");
            if (source3 == null)
                throw new ArgumentNullException("source3");
            if (source4 == null)
                throw new ArgumentNullException("source4");
            if (source5 == null)
                throw new ArgumentNullException("source5");
            if (combine == null)
                throw new ArgumentNullException("combine");

            return ZipIterator<T1, T2, T3, T4, T5, TResult>(source1, source2, source3, source4, source5, combine);
        }

        public static IEnumerable<TResult> Zip<T1, T2, T3, T4, T5, T6, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4, IEnumerable<T5> source5, IEnumerable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> combine)
        {
            if (source1 == null)
                throw new ArgumentNullException("source1");
            if (source2 == null)
                throw new ArgumentNullException("source2");
            if (source3 == null)
                throw new ArgumentNullException("source3");
            if (source4 == null)
                throw new ArgumentNullException("source4");
            if (source5 == null)
                throw new ArgumentNullException("source5");
            if (source6 == null)
                throw new ArgumentNullException("source6");
            if (combine == null)
                throw new ArgumentNullException("combine");

            return ZipIterator<T1, T2, T3, T4, T5, T6, TResult>(source1, source2, source3, source4, source5, source6, combine);
        }

        private static IEnumerable<IEnumerable<TSource>> ClumpIterator<TSource>(IEnumerable<TSource> source, int size)
        {
            TSource[] items = new TSource[size];
            int count = 0;
            foreach (var item in source)
            {
                items[count] = item;
                count++;

                if (count == size)
                {
                    yield return items;
                    items = new TSource[size];
                    count = 0;
                }
            }
            if (count > 0)
            {
                if (count == size)
                    yield return items;
                else
                {
                    TSource[] tempItems = new TSource[count];
                    Array.Copy(items, tempItems, count);
                    yield return tempItems;
                }
            }
        }

        private static IEnumerable<T> CycleIterator<T>(IEnumerable<T> source)
        {
            while (true)
                using (IEnumerator<T> data = source.GetEnumerator())
                {
                    while (data.MoveNext())
                    {
                        yield return data.Current;
                    }
                }
        }

        private static IEnumerable<TSource> DoIterator<TSource>(IEnumerable<TSource> source, Action<TSource> action)
        {
            using (var data = source.GetEnumerator())
                while (data.MoveNext())
                {
                    action(data.Current);
                    yield return data.Current;
                }
        }

        private static IEnumerable<TSource> DoIterator<TSource>(IEnumerable<TSource> source, Action<TSource, int> action)
        {
            int index = 0;
            using (var data = source.GetEnumerator())
                while (data.MoveNext())
                {
                    action(data.Current, index++);
                    yield return data.Current;
                }
        }

        private static IEnumerable<T> IterateIterator<T>(T start, int count, Func<T, T> iterator)
        {
            int i = 0;
            T result = start;
            while (i < count)
            {
                yield return result;
                result = iterator(result);
                i++;
            }
        }

        private static IEnumerable<T> RepeatItemsIterator<T>(IEnumerable<T> source, int count)
        {
            using (IEnumerator<T> data = source.GetEnumerator())
                while (data.MoveNext())
                {
                    for (int i = 0; i < count; i++)
                    {
                        yield return data.Current;
                    }
                }
        }

        private static IEnumerable<TResult> ScanIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TSource, TResult> combine)
        {
            using (var data = source.GetEnumerator())
                if (data.MoveNext())
                {
                    TSource first = data.Current;

                    while (data.MoveNext())
                    {
                        yield return combine(first, data.Current);
                        first = data.Current;
                    }
                }
        }

        private static IEnumerable<TSource> ScanlIterator<TSource>(IEnumerable<TSource> source, Func<TSource, TSource, TSource> combine)
        {
            using (var data = source.GetEnumerator())
                if (data.MoveNext())
                {
                    TSource first = data.Current;
                    yield return first;

                    while (data.MoveNext())
                    {
                        first = combine(first, data.Current);
                        yield return first;
                    }
                }
        }

        private static IEnumerable<TResult> ZipIterator<T1, T2, T3, TResult>(IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, Func<T1, T2, T3, TResult> combine)
        {
            using (IEnumerator<T1> data1 = source1.GetEnumerator())
            using (IEnumerator<T2> data2 = source2.GetEnumerator())
            using (IEnumerator<T3> data3 = source3.GetEnumerator())
                while (data1.MoveNext() && data2.MoveNext() && data3.MoveNext())
                {
                    yield return combine(data1.Current, data2.Current, data3.Current);
                }
        }

        private static IEnumerable<TResult> ZipIterator<T1, T2, T3, T4, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4, Func<T1, T2, T3, T4, TResult> combine)
        {
            using (IEnumerator<T1> data1 = source1.GetEnumerator())
            using (IEnumerator<T2> data2 = source2.GetEnumerator())
            using (IEnumerator<T3> data3 = source3.GetEnumerator())
            using (IEnumerator<T4> data4 = source4.GetEnumerator())
                while (data1.MoveNext() && data2.MoveNext() && data3.MoveNext() && data4.MoveNext())
                {
                    yield return combine(data1.Current, data2.Current, data3.Current, data4.Current);
                }
        }

        private static IEnumerable<TResult> ZipIterator<T1, T2, T3, T4, T5, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4, IEnumerable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> combine)
        {
            using (IEnumerator<T1> data1 = source1.GetEnumerator())
            using (IEnumerator<T2> data2 = source2.GetEnumerator())
            using (IEnumerator<T3> data3 = source3.GetEnumerator())
            using (IEnumerator<T4> data4 = source4.GetEnumerator())
            using (IEnumerator<T5> data5 = source5.GetEnumerator())
                while (data1.MoveNext() && data2.MoveNext() && data3.MoveNext() && data4.MoveNext() && data5.MoveNext())
                {
                    yield return combine(data1.Current, data2.Current, data3.Current, data4.Current, data5.Current);
                }
        }

        private static IEnumerable<TResult> ZipIterator<T1, T2, T3, T4, T5, T6, TResult>(this IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3, IEnumerable<T4> source4, IEnumerable<T5> source5, IEnumerable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> combine)
        {
            using (IEnumerator<T1> data1 = source1.GetEnumerator())
            using (IEnumerator<T2> data2 = source2.GetEnumerator())
            using (IEnumerator<T3> data3 = source3.GetEnumerator())
            using (IEnumerator<T4> data4 = source4.GetEnumerator())
            using (IEnumerator<T5> data5 = source5.GetEnumerator())
            using (IEnumerator<T6> data6 = source6.GetEnumerator())
                while (data1.MoveNext() && data2.MoveNext() && data3.MoveNext() && data4.MoveNext() && data5.MoveNext() && data6.MoveNext())
                {
                    yield return combine(data1.Current, data2.Current, data3.Current, data4.Current, data5.Current, data6.Current);
                }
        }
    }
}