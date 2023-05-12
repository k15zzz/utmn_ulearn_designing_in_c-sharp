using System;

namespace Memory.API
{
    public class APIObject : IDisposable
    {
        // Идентификатор ресурса, выделенного внешним API
        private readonly int id;

        // Флаг, указывающий, был ли объект уже освобожден
        private bool disposed = false;

        // Конструктор класса APIObject
        public APIObject(int id)
        {
            this.id = id;
            // Выделение ресурса через внешний API
            MagicAPI.Allocate(id);
        }

        // Метод Dispose, реализующий интерфейс IDisposable
        public void Dispose()
        {
            // Если объект еще не был освобожден
            if (!disposed)
            {
                // Освобождение ресурса через внешний API
                MagicAPI.Free(id);
                // Установка флага, указывающего, что объект был освобожден
                disposed = true;
                // Отключение деструктора объекта
                GC.SuppressFinalize(this); 
            }
        }

        // Деструктор класса APIObject
        ~APIObject()
        {
            // Если объект еще не был освобожден
            if (!disposed)
            {
                // Освобождение ресурса через внешний API
                MagicAPI.Free(id);
            }
        }
    }
}