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

    public class StackOperationsLogger
    {
        private readonly Observer observer = new Observer();
        public void SubscribeOn<T>(ObservableStack<T> stack)
        {
            stack.StackEvent += observer.HandleEvent;
        }

        public string GetLog()
        {
            return observer.Log.ToString();
        }
    }

    public class Observer
    {
        public StringBuilder Log = new StringBuilder();

        public void HandleEvent(object sender, StackEventData<object> eventData)
        {
            Log.Append(eventData.ToString());
        }
    }

    public class ObservableStack<T>
    {
        public event EventHandler<StackEventData<object>> StackEvent;

        List<T> data = new List<T>();

        public void Push(T obj)
        {
            data.Add(obj);
            StackEvent?.Invoke(this, new StackEventData<object> { IsPushed = true, Value = obj });
        }

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