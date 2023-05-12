using System;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        private int numerator; // числитель
        private int denominator; // знаменатель

        // Конструктор класса
        public Rational(int numerator, int denominator = 1)
        {
            // Если знаменатель равен нулю, то число не определено
            if (denominator == 0)
            {
                IsNan = true;
                return;
            }

            // Находим наибольший общий делитель числителя и знаменателя
            int gcd = Gcd(numerator, denominator);

            // Сокращаем дробь на этот НОД
            this.numerator = numerator / gcd;
            this.denominator = denominator / gcd;

            // Если знаменатель отрицательный, то меняем знак числителя и знаменателя
            if (this.denominator < 0)
            {
                this.numerator = -this.numerator;
                this.denominator = -this.denominator;
            }
        }

        // Свойство, которое возвращает true, если число не определено
        public bool IsNan { get; }

        // Свойство, которое возвращает числитель
        public int Numerator => numerator;

        // Свойство, которое возвращает знаменатель
        public int Denominator => denominator;

        // Перегруженный оператор сложения
        public static Rational operator +(Rational a, Rational b)
        {
            // Если хотя бы одно число не определено, то результат тоже не определен
            if (a.IsNan || b.IsNan)
            {
                return new Rational(0, 0);
            }

            // Находим наименьшее общее кратное знаменателей
            int lcm = Lcm(a.denominator, b.denominator);

            // Складываем числители с учетом общего знаменателя
            int num = a.numerator * (lcm / a.denominator) + b.numerator * (lcm / b.denominator);

            // Возвращаем новое рациональное число
            return new Rational(num, lcm);
        }

        // Перегруженный оператор вычитания
        public static Rational operator -(Rational a, Rational b)
        {
            // Если хотя бы одно число не определено, то результат тоже не определен
            if (a.IsNan || b.IsNan)
            {
                return new Rational(0, 0);
            }

            // Находим наименьшее общее кратное знаменателей
            int lcm = Lcm(a.denominator, b.denominator);

            // Вычитаем числители с учетом общего знаменателя
            int num = a.numerator * (lcm / a.denominator) - b.numerator * (lcm / b.denominator);

            // Возвращаем новое рациональное число
            return new Rational(num, lcm);
        }

        // Перегруженный оператор умножения
        public static Rational operator *(Rational a, Rational b)
        {
            // Если хотя бы одно число не определено, то результат тоже не определен
            if (a.IsNan || b.IsNan)
            {
                return new Rational(0, 0);
            }

            // Умножаем числитель первого числа на числитель второго числа,
            // а знаменатель первого числа на знаменатель второго числа
            int num = a.numerator * b.numerator;
            int den = a.denominator * b.denominator;

            // Возвращаем новое рациональное число
            return new Rational(num, den);
        }

        // Перегруженный оператор деления
        public static Rational operator /(Rational a, Rational b)
        {
            // Если хотя бы одно число не определено или знаменатель второго числа равен нулю, то результат тоже не определен
            if (a.IsNan || b.IsNan || b.numerator == 0)
            {
                return new Rational(0, 0);
            }

            // Делим числитель первого числа на знаменатель второго числа,
            // а знаменатель первого числа на числитель второго числа
            int num = a.numerator * b.denominator;
            int den = a.denominator * b.numerator;

            // Возвращаем новое рациональное число
            return new Rational(num, den);
        }

        // Перегруженный оператор явного преобразования рационального числа в целое число
        public static explicit operator int(Rational r)
        {
            // Если знаменатель не равен 1, то число не является целым
            if (r.denominator != 1)
            {
                throw new Exception("Cannot convert non-integer rational number to integer");
            }

            // Возвращаем числитель
            return r.numerator;
        }

        // Перегруженный оператор неявного преобразования целого числа в рациональное число
        public static implicit operator Rational(int i)
        {
            // Создаем новое рациональное число с числителем i и знаменателем 1
            return new Rational(i);
        }

        // Перегруженный оператор неявного преобразования рационального числа в десятичное число
        public static implicit operator double(Rational r)
        {
            // Делим числитель на знаменатель и возвращаем результат
            return (double)r.numerator / r.denominator;
        }

        // Метод для нахождения наибольшего общего делителя
        private static int Gcd(int a, int b)
        {
            // Берем модули чисел
            a = Math.Abs(a);
            b = Math.Abs(b);

            // Используем алгоритм Евклида для нахождения НОД
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            // Возвращаем НОД
            return a;
        }

        // Метод для нахождения наименьшего общего кратного
        private static int Lcm(int a, int b)
        {
            // Находим произведение чисел и делим его на НОД
            return Math.Abs(a * b) / Gcd(a, b);
        }
    }
}