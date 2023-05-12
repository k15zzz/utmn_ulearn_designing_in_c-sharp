using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            var pairs = data.Pairs().Select((p, i) => new { Pair = p, Index = i });
            return pairs.MaxBy(p => (p.Pair.Item2 - p.Pair.Item1).TotalSeconds).Index;
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            var pairs = data.Pairs().Select(p => (p.Item2 - p.Item1) / p.Item1);
            return pairs.Average();
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> source)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                    yield break;

                var previous = iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return Tuple.Create(previous, iterator.Current);
                    previous = iterator.Current;
                }
            }
        }

        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.Aggregate((a, b) => Comparer<TKey>.Default.Compare(keySelector(a), keySelector(b)) > 0 ? a : b);
        }
    }

    public static class AnalysisExtensions
    {
        public static int MaxIndex(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            int maxIndex = -1;
            int maxValue = int.MinValue;
            int currentIndex = 0;

            foreach (int value in source)
            {
                if (value > maxValue)
                {
                    maxValue = value;
                    maxIndex = currentIndex;
                }

                currentIndex++;
            }

            if (maxIndex == -1)
                throw new InvalidOperationException("Sequence was empty");

            return maxIndex;
        }
    }
}