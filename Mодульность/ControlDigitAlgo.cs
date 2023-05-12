using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    // Создаем статический класс Extensions для расширения функциональности типа long
    public static class Extensions
    {
        // Метод для получения цифр числа в прямом порядке
        public static IEnumerable<int> GetDigits(this long number)
        {
            return number.GetDigitsInReverseOrder().Reverse();
        }

        // Метод для получения цифр числа в обратном порядке
        public static IEnumerable<int> GetDigitsInReverseOrder(this long number)
        {
            do
            {
                yield return (int)(number % 10);
                number /= 10;
            }
            while (number > 0);
        }

        // Метод для суммирования элементов последовательности с помощью селектора
        public static int SumWithSelector<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            return source.Select(selector).Sum();
        }

        // Перегрузка метода SumWithSelector для последовательности целых чисел
        public static int SumWithSelector(this IEnumerable<int> source, Func<int, int> selector)
        {
            return source.Select(selector).Sum();
        }
    }

    // Создаем статический класс ControlDigitAlgo для реализации алгоритмов контрольных цифр
    public static class ControlDigitAlgo
    {
        // Метод для вычисления контрольной цифры с помощью заданных множителей
        private static int CalculateControlDigit(long number, int[] factors)
        {
            int sum = number.GetDigits().SumWithSelector(digit => factors.Next() * digit);
            return (10 - sum % 10) % 10;
        }

        // Метод для получения множителей для алгоритма ISBN-10
        private static int[] GetIsbn10Factors()
        {
            return new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        }

        // Метод для получения множителей для алгоритма Luhn
        private static int[] GetLuhnFactors()
        {
            return new[] { 2, 1 }.Repeat(numberOfRepetitions: 100);
        }

        // Метод для вычисления контрольной цифры UPC
        public static int Upc(long number)
        {
            int[] factors = { 3, 1 };
            return CalculateControlDigit(number, factors);
        }

        // Метод для вычисления контрольной цифры ISBN-10
        public static int Isbn10(long number)
        {
            int[] factors = GetIsbn10Factors();
            int sum = number.GetDigits().Take(9).SumWithSelector(digit => factors.Next() * digit);
            int controlDigit = CalculateControlDigit(sum, factors);
            return controlDigit == 0 ? '0' : (char)('0' + (10 - controlDigit));
        }

        // Метод для вычисления контрольной цифры Luhn
        public static int Luhn(long number)
        {
            int[] factors = GetLuhnFactors();
            int sum = number.GetDigits().SumWithSelector(digit => factors.Next() * digit);
            return (10 - sum % 10) % 10;
        }

        // Вспомогательный метод для получения следующего множителя из массива множителей
        private static int _index;
        private static int Next(this int[] array)
        {
            int index = array.Length - 1 - _index;
            _index++;
            return array[index];
        }

        // Вспомогательный метод для повторения элементов массива заданное количество раз
        private static int[] Repeat(this int[] array, int numberOfRepetitions)
        {
            return Enumerable.Range(0, numberOfRepetitions).SelectMany(_ => array).ToArray();
        }
    }
}