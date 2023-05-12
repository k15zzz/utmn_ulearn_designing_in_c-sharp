using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Reflection.Randomness
{
    public class Generator<T> where T : new()
    {
        // Словарь генераторов случайных значений для свойств класса T
        private readonly Dictionary<string, Func<Random, object>> _generators;

        public Generator()
        {
            // Получаем все свойства класса T, помеченные атрибутом FromDistributionAttribute
            _generators = typeof(T).GetProperties()
                .Where(prop => prop.GetCustomAttributes(typeof(FromDistributionAttribute), true).Length > 0)
                .ToDictionary(prop => prop.Name, prop =>
                {
                    // Получаем атрибут FromDistributionAttribute для свойства
                    var attribute = (FromDistributionAttribute)prop.GetCustomAttributes(typeof(FromDistributionAttribute), true)[0];
                    // Создаем экземпляр класса распределения с указанными аргументами
                    var distribution = (IContinuousDistribution)Activator.CreateInstance(attribute.DistributionType, attribute.Arguments);
                    // Создаем функцию, которая генерирует случайное значение для свойства
                    return new Func<Random, object>(rnd => distribution.Generate(rnd));
                });
        }

        // Метод, который генерирует случайный экземпляр класса T
        public T Generate(Random rnd)
        {
            // Создаем новый экземпляр класса T
            var instance = new T();
            // Заполняем свойства класса T случайными значениями
            foreach (var generator in _generators)
            {
                var value = generator.Value(rnd);
                instance.GetType().GetProperty(generator.Key).SetValue(instance, value);
            }
            // Возвращаем созданный экземпляр класса T
            return instance;
        }
    }

    // Атрибут, который указывает на тип распределения и его параметры для свойства класса
    [AttributeUsage(AttributeTargets.Property)]
    public class FromDistributionAttribute : Attribute
    {
        public Type DistributionType { get; }
        public object[] Arguments { get; }

        public FromDistributionAttribute(Type distributionType, params object[] arguments)
        {
            // Проверяем, что тип распределения является действительным типом распределения
            if (!typeof(IContinuousDistribution).IsAssignableFrom(distributionType))
            {
                throw new ArgumentException($"Type {distributionType.Name} is not a valid distribution type");
            }
            // Проверяем, что конструктор для типа распределения с указанными аргументами существует
            var constructor = distributionType.GetConstructor(arguments.Select(arg => arg.GetType()).ToArray());
            if (constructor == null)
            {
                throw new ArgumentException($"Constructor for type {distributionType.Name} with specified arguments not found");
            }
            // Сохраняем тип распределения и его аргументы
            DistributionType = distributionType;
            Arguments = arguments;
        }
    }
}