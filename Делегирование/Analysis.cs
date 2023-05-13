using System;
using System.Collections.Generic;
using System.Linq;

// Определяем класс Analysis, содержащий два метод для анализа данных
namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        // Метод FindMaxPeriodIndex принимает список дат и возвращает индекс пары с наибольшим периодом между датами
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            // Преобразуем список дат в последовательность пар дат и добавляем индекс каждой пары
            var pairs = data.Pairs().Select((p, i) => new { Pair = p, Index = i });
            // Используем метод MaxBy для поиска пары с наибольшим периодом между датами и возвращаем ее индекс
            return pairs.MaxBy(p => (p.Pair.Item2 - p.Pair.Item1).TotalSeconds).Index;
        }

        // Метод FindAverageRelativeDifference принимает список чисел и возвращает среднее значение относительных разностей между парами чисел
        public static double FindAverageRelativeDifference(params double[] data)
        {
            // Преобразуем список чисел в последовательность пар чисел и вычисляем относительную разность между каждой парой
            var pairs = data.Pairs().Select(p => (p.Item2 - p.Item1) / p.Item1);
            // Используем метод Average для вычисления среднего значения относительных разностей
            return pairs.Average();
        }
    }
    // Определяем класс EnumerableExtensions, содержащий методы расширения для класса IEnumerable
    public static class EnumerableExtensions
    {
        // Метод Pairs принимает последовательность элементов и возвращает последовательность кортежей, каждый из которых содержит два соседних элемента исходной последовательности
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> source)
        {
            // Используем итератор для последовательного перебора элементов исходной последовательности
            using (var iterator = source.GetEnumerator())
            {
                // Если исходная последовательность пуста, то возвращаем пустую последовательность кортежей
                if (!iterator.MoveNext())
                    yield break;

                // Иначе сохраняем первый элемент и перебираем оставшиеся элементы, создавая кортежи из соседних элементов
                var previous = iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return Tuple.Create(previous, iterator.Current);
                    previous = iterator.Current;
                }
            }
        }

        // Метод MaxBy принимает последовательность элементов и функцию, которая преобразует каждый элемент в ключ, по которому будет производиться сравнение. Метод возвращает элемент последовательности, у которого значение ключа максимально.
        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            // Используем метод Aggregate для последовательного сравнения элементов исходной последовательности и выбора элемента с максимальным значением ключа
            return source.Aggregate((a, b) => Comparer<TKey>.Default.Compare(keySelector(a), keySelector(b)) > 0 ? a : b);
        }
    }

    // Определяем класс AnalysisExtensions, содержащий метод расширения для класса IEnumerable<int>
    public static class AnalysisExtensions
    {
        // Метод MaxIndex принимает последовательность целых чисел и возвращает индекс максимального элемента
        public static int MaxIndex(this IEnumerable<int> source)
        {
            // Если последовательность пуста, то выбрасываем исключение
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            int maxIndex = -1;
            int maxValue = int.MinValue;
            int currentIndex = 0;

            // Перебираем элементы последовательности, сохраняя индекс и значение максимального элемента
            foreach (int value in source)
            {
                if (value > maxValue)
                {
                    maxValue = value;
                    maxIndex = currentIndex;
                }

                currentIndex++;
            }

            // Если максимальный элемент не найден, то выбрасываем исключение
            if (maxIndex == -1)
                throw new InvalidOperationException("Sequence was empty");

            // Возвращаем индекс максимального элемента
            return maxIndex;
        }
    }
}