using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Код с использованием событий значительно короче и проще для понимания, чем классическая реализация шаблона Наблюдатель. 
 * Вместо создания отдельных интерфейсов и классов для наблюдателей и наблюдаемых объектов, 
 * мы можем использовать встроенный механизм событий в C#. 
 * Кроме того, код становится более типобезопасным, 
 * так как мы можем использовать обобщенный тип StackEventData для передачи данных о событии.
 */
namespace Delegates.Observers
{
    // Класс StackOperationsLogger используется для логирования операций со стеком.
    public class StackOperationsLogger
    {
        // Создаем объект типа Observer для обработки событий.
        private readonly Observer observer = new Observer();

        // Метод SubscribeOn<T> используется для подписки на событие StackEvent у объекта типа ObservableStack<T>.
        public void SubscribeOn<T>(ObservableStack<T> stack)
        {
            stack.StackEvent += observer.HandleEvent;
        }

        // Метод GetLog() возвращает лог, который был записан в объекте типа Observer.
        public string GetLog()
        {
            return observer.Log.ToString();
        }
    }

    // Класс Observer используется для обработки событий, которые происходят со стеком.
    public class Observer
    {
        // Создаем объект типа StringBuilder для записи лога.
        public StringBuilder Log = new StringBuilder();

        // Метод HandleEvent используется для записи информации о событии в объект типа StringBuilder.
        public void HandleEvent(object sender, StackEventData<object> eventData)
        {
            Log.Append(eventData.ToString());
        }
    }

    // Класс ObservableStack реализует стек с помощью списка и имеет два метода: Push и Pop.
    public class ObservableStack
    {
        // Определяем событие StackEvent, которое генерируется при каждом вызове методов Push и Pop.
        public event EventHandler> StackEvent;

        // Создаем список для хранения элементов стека.
        List<T> data = new List<T>();

        // Метод Push добавляет элемент в стек и генерирует событие StackEvent.
        public void Push(T obj)
        {
            data.Add(obj);
            StackEvent?.Invoke(this, new StackEventData<object> { IsPushed = true, Value = obj });
        }

        // Метод Pop удаляет элемент из стека и генерирует событие StackEvent.
        public T Pop()
        {
            if (data.Count == 0)
                throw new InvalidOperationException();
            var result = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            StackEvent?.Invoke(this, new StackEventData<object> { IsPushed = false, Value = result });
            return result;
        }
    }
}