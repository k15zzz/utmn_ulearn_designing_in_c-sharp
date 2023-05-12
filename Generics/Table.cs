using System;
using System.Collections.Generic;
using System.Linq;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
        // Словарь, который хранит значения таблицы. Ключом первого уровня является строка, а значением - словарь, который хранит значения для каждого столбца в этой строке.
        private readonly Dictionary<TRow, Dictionary<TColumn, TValue>> _table = new Dictionary<TRow, Dictionary<TColumn, TValue>>();
        // Свойство, которое возвращает экземпляр класса TableOpenIndex, который предоставляет доступ к таблице с автоматическим созданием нужных строк и столбцов при обращении к таблице по соответствующим индексам.
        public TableOpenIndex<TRow, TColumn, TValue> Open => new TableOpenIndex<TRow, TColumn, TValue>(this);
        // Свойство, которое возвращает экземпляр класса TableExistedIndex, который предоставляет доступ к таблице, которая требует создания столбцов и строк заранее и выбрасывает исключение при доступе к несуществующим столбцам или строкам
        public TableExistedIndex<TRow, TColumn, TValue> Existed => new TableExistedIndex<TRow, TColumn, TValue>(this);
        // Коллекция всех строк в таблице.
        public IEnumerable<TRow> Rows => _table.Keys;
        // Коллекция всех столбцов в таблице.
        public IEnumerable<TColumn> Columns
        {
            get
            {
                // Используем HashSet для удаления дубликатов столбцов.
                var columns = new HashSet<TColumn>();
                foreach (var row in _table.Values)
                {
                    foreach (var column in row.Keys)
                    {
                        columns.Add(column);
                    }
                }
                return columns;
            }
        }
        // Метод, который добавляет новую строку в таблицу, если она еще не существует.
        public void AddRow(TRow row)
        {
            if (!_table.ContainsKey(row))
            {
                _table[row] = new Dictionary<TColumn, TValue>();
            }
        }
        // Метод, который добавляет новый столбец в таблицу для каждой строки, если он еще не существует.
        public void AddColumn(TColumn column)
        {
            foreach (var row in _table.Values)
            {
                if (!row.ContainsKey(column))
                {
                    row[column] = default(TValue);
                }
            }
        }
        // Индексатор, который позволяет получать и устанавливать значения в таблице.
        public TValue this[TRow row, TColumn column]
        {
            get
            {
                if (_table.TryGetValue(row, out var rowValues) && rowValues.TryGetValue(column, out var value))
                {
                    return value;
                }
                return default(TValue);
            }
            set
            {
                if (!_table.TryGetValue(row, out var rowValues))
                {
                    rowValues = new Dictionary<TColumn, TValue>();
                    _table[row] = rowValues;
                }
                rowValues[column] = value;
            }
        }
        // Класс, который предоставляет доступ к таблице, которая требует создания столбцов и строк заранее и выбрасывает исключение при доступе к несуществующим столбцам или строкам.
        public class TableExistedIndex<TRow, TColumn, TValue>
        {
            private readonly Table<TRow, TColumn, TValue> _table;

            public TableExistedIndex(Table<TRow, TColumn, TValue> table)
            {
                _table = table;
            }
            // Индексатор, который позволяет получать и устанавливать значения в таблице.
            public TValue this[TRow row, TColumn column]
            {
                get
                {
                    if (!_table.Rows.Contains(row))
                    {
                        throw new ArgumentException($"Row {row} does not exist in the table.");
                    }
                    if (!_table.Columns.Contains(column))
                    {
                        throw new ArgumentException($"Column {column} does not exist in the table.");
                    }
                    return _table[row, column];
                }
                set
                {
                    if (!_table.Rows.Contains(row))
                    {
                        throw new ArgumentException($"Row {row} does not exist in the table.");
                    }
                    if (!_table.Columns.Contains(column))
                    {
                        throw new ArgumentException($"Column {column} does not exist in the table.");
                    }
                    _table[row, column] = value;
                }
            }
        }
        // Класс, который предоставляет доступ к таблице с автоматическим созданием нужных строк и столбцов при обращении к таблице по соответствующим индексам.
        public class TableOpenIndex<TRow, TColumn, TValue>
        {
            private readonly Table<TRow, TColumn, TValue> _table;

            public TableOpenIndex(Table<TRow, TColumn, TValue> table)
            {
                _table = table;
            }
            // Индексатор, который позволяет получать и устанавливать значения в таблице.
            public TValue this[TRow row, TColumn column]
            {
                get
                {
                    return _table[row, column];
                }
                set
                {
                    _table.AddRow(row);
                    _table.AddColumn(column);
                    _table[row, column] = value;
                }
            }
        }
    }
}