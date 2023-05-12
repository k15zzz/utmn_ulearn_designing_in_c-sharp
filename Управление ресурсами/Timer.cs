using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        // Объект StringWriter, используемый для записи отчета о времени выполнения
        private readonly StringWriter _stringWriter;

        // Имя таймера
        protected readonly string _nameTimer;

        // Уровень вложенности таймера
        protected readonly int _levelTimer;

        // Объект Stopwatch, используемый для измерения времени выполнения
        protected readonly Stopwatch _stopwatch = new Stopwatch();

        // Коллекция дочерних таймеров
        protected readonly ICollection<Timer> _childsTimer = new List<Timer>();

        // Конструктор класса Timer
        protected Timer(string name, int level = 0, StringWriter writer = null)
        {
            _stringWriter = writer;
            _nameTimer = name;
            _levelTimer = level;

            // Запускаем таймер
            _stopwatch.Start();
        }

        // Метод StartChildTimer создает новый дочерний таймер и добавляет его в коллекцию дочерних таймеров
        public Timer StartChildTimer(string name)
        {
            var child = new Timer(name, _levelTimer + 1);

            _childsTimer.Add(child);

            return child;
        }

        // Статический метод Start создает новый таймер и возвращает его
        public static Timer Start(StringWriter writer, string name = "*")
        {
            return new Timer(name, 0, writer);
        }

        // Статический метод FormatReportLine форматирует строку отчета о времени выполнения
        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }

        // Метод Dispose останавливает таймер и записывает отчет о времени выполнения
        public virtual void Dispose()
        {
            _stopwatch.Stop();

            // Если уровень вложенности таймера равен 0, то записываем отчет о времени выполнения
            if (_levelTimer == 0)
            {
                WriteReport(_stringWriter);
            }
        }

        // Метод WriteReport записывает отчет о времени выполнения
        protected void WriteReport(StringWriter stringWriter)
        {
            // Форматируем строку отчета о времени выполнения
            var reportLine = Timer.FormatReportLine(_nameTimer,
                _levelTimer, _stopwatch.ElapsedMilliseconds);
            stringWriter.Write(reportLine);

            // Если у таймера есть дочерние таймеры, то записываем отчет о времени выполнения каждого из них
            if (!_childsTimer.Any())
                return;

            foreach (var child in _childsTimer)
                child.WriteReport(stringWriter);

            // Вычисляем время выполнения таймера без учета времени выполнения дочерних таймеров
            var childTime = _childsTimer.Sum(c => c._stopwatch.ElapsedMilliseconds);
            var totalTime = _stopwatch.ElapsedMilliseconds - childTime;

            // Форматируем строку отчета о времени выполнения таймера без учета времени выполнения дочерних таймеров
            reportLine = Timer.FormatReportLine("Rest", _levelTimer + 1, totalTime);
            stringWriter.Write(reportLine);
        }
    }
}