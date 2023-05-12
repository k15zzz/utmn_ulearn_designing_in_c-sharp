using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    // Интерфейс для форматирования отчета
    public interface IReportFormatter
    {
        // Метод для формирования заголовка отчета
        string MakeCaption(string caption);

        // Метод для начала списка
        string BeginList();

        // Метод для формирования элемента списка
        string MakeItem(string valueType, string entry);

        // Метод для окончания списка
        string EndList();
    }

    // Интерфейс для вычисления статистики
    public interface IStatisticsCalculator
    {
        // Метод для вычисления статистики
        object MakeStatistics(IEnumerable<double> data);
    }

    // Класс для форматирования отчета в HTML
    public class HtmlReportFormatter : IReportFormatter
    {
        // Метод для формирования заголовка отчета в HTML
        public string MakeCaption(string caption)
        {
            return $"<h1>{caption}</h1>";
        }

        // Метод для начала списка в HTML
        public string BeginList()
        {
            return "<ul>";
        }

        // Метод для формирования элемента списка в HTML
        public string MakeItem(string valueType, string entry)
        {
            return $"<li><b>{valueType}</b>: {entry}";
        }

        // Метод для окончания списка в HTML
        public string EndList()
        {
            return "</ul>";
        }
    }

    // Класс для форматирования отчета в Markdown
    public class MarkdownReportFormatter : IReportFormatter
    {
        // Метод для формирования заголовка отчета в Markdown
        public string MakeCaption(string caption)
        {
            return $"## {caption}\n\n";
        }

        // Метод для начала списка в Markdown
        public string BeginList()
        {
            return "";
        }

        // Метод для формирования элемента списка в Markdown
        public string MakeItem(string valueType, string entry)
        {
            return $" * **{valueType}**: {entry}\n\n";
        }

        // Метод для окончания списка в Markdown
        public string EndList()
        {
            return "";
        }
    }

    // Класс для вычисления среднего и стандартного отклонения
    public class MeanAndStdCalculator : IStatisticsCalculator
    {
        // Метод для вычисления среднего и стандартного отклонения
        public object MakeStatistics(IEnumerable<double> _data)
        {
            // Копируем данные в список
            var data = _data.ToList();

            // Вычисляем среднее значение
            var mean = data.Average();

            // Вычисляем стандартное отклонение
            var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

            // Возвращаем объект MeanAndStd, содержащий среднее значение и стандартное отклонение
            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        }
    }

    // Класс для вычисления медианы
    public class MedianCalculator : IStatisticsCalculator
    {
        // Метод для вычисления медианы
        public object MakeStatistics(IEnumerable<double> data)
        {
            // Копируем данные в список и сортируем его
            var list = data.OrderBy(z => z).ToList();

            // Если количество элементов четное, то медиана равна среднему значению двух средних элементов
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

            // Если количество элементов нечетное, то медиана равна среднему значению среднего элемента
            return list[list.Count / 2];
        }
    }

    // Класс для формирования отчета
    public class ReportMaker
    {
        private readonly IReportFormatter _formatter;
        private readonly IStatisticsCalculator _calculator;

        public ReportMaker(IReportFormatter formatter, IStatisticsCalculator calculator)
        {
            _formatter = formatter;
            _calculator = calculator;
        }

        // Метод для формирования отчета
        public string MakeReport(IEnumerable<Measurement> measurements)
        {
            // Копируем данные в список
            var data = measurements.ToList();

            // Создаем объект StringBuilder для формирования отчета
            var result = new StringBuilder();

            // Добавляем заголовок отчета
            result.Append(_formatter.MakeCaption(Caption));

            // Начинаем список
            result.Append(_formatter.BeginList());

            // Добавляем элемент списка для температуры
            result.Append(_formatter.MakeItem("Temperature", _calculator.MakeStatistics(data.Select(z => z.Temperature)).ToString()));

            // Добавляем элемент списка для влажности
            result.Append(_formatter.MakeItem("Humidity", _calculator.MakeStatistics(data.Select(z => z.Humidity)).ToString()));

            // Заканчиваем список
            result.Append(_formatter.EndList());

            // Возвращаем отчет в виде строки
            return result.ToString();
        }

        // Свойство для задания заголовка отчета
        public string Caption { get; set; }
    }

    // Класс-помощник для формирования отчетов
    public static class ReportMakerHelper
    {
        // Метод для формирования отчета о среднем и стандартном отклонении в HTML
        public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
        {
            // Создаем объект ReportMaker с HtmlReportFormatter и MeanAndStdCalculator
            var htmlReportMaker = new ReportMaker(new HtmlReportFormatter(), new MeanAndStdCalculator());

            // Задаем заголовок отчета
            htmlReportMaker.Caption = "Mean and Std";

            // Формируем отчет и возвращаем его в виде строки
            return htmlReportMaker.MakeReport(data);
        }

        // Метод для формирования отчета о медиане в Markdown
        public static string MedianMarkdownReport(IEnumerable<Measurement> data)
        {
            // Создаем объект ReportMaker с MarkdownReportFormatter и MedianCalculator
            var markdownReportMaker = new ReportMaker(new MarkdownReportFormatter(), new MedianCalculator());

            // Задаем заголовок отчета
            markdownReportMaker.Caption = "Median";

            // Формируем отчет и возвращаем его в виде строки
            return markdownReportMaker.MakeReport(data);
        }

        // Метод для формирования отчета о среднем и стандартном отклонении в Markdown
        public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> data)
        {
            // Создаем объект ReportMaker с MarkdownReportFormatter и MeanAndStdCalculator
            var markdownReportMaker = new ReportMaker(new MarkdownReportFormatter(), new MeanAndStdCalculator());

            // Задаем заголовок отчета
            markdownReportMaker.Caption = "Mean and Std";

            // Формируем отчет и возвращаем его в виде строки
            return markdownReportMaker.MakeReport(data);
        }

        // Метод для формирования отчета о медиане в HTML
        public static string MedianHtmlReport(IEnumerable<Measurement> data)
        {
            // Создаем объект ReportMaker с HtmlReportFormatter и MedianCalculator
            var htmlReportMaker = new ReportMaker(new HtmlReportFormatter(), new MedianCalculator());

            // Задаем заголовок отчета
            htmlReportMaker.Caption = "Median";

            // Формируем отчет и возвращаем его в виде строки
            return htmlReportMaker.MakeReport(data);
        }
    }
}