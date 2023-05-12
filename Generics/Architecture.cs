using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots
{
    // Определяем интерфейс IRobotAI с обобщенным параметром TCommand, который должен реализовывать интерфейс IMoveCommand
    // и методом GetCommand, который возвращает объект типа TCommand
    public interface IRobotAI<out TCommand> where TCommand : IMoveCommand
    {
        TCommand GetCommand();
    }

    // Определяем абстрактный класс RobotAI с обобщенным параметром TCommand, который должен реализовывать интерфейс IMoveCommand
    // и методом GetCommand, который возвращает объект типа TCommand
    public abstract class RobotAI<TCommand> : IRobotAI<TCommand> where TCommand : IMoveCommand
    {
        public abstract TCommand GetCommand();
    }

    // Определяем класс ShooterAI, который наследуется от абстрактного класса RobotAI с обобщенным параметром IShooterMoveCommand
    // и реализует метод GetCommand, который возвращает объект типа IShooterMoveCommand
    public class ShooterAI : RobotAI<IShooterMoveCommand>
    {
        int counter = 1;

        public override IShooterMoveCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    // Определяем класс BuilderAI, который наследуется от абстрактного класса RobotAI с обобщенным параметром IMoveCommand
    // и реализует метод GetCommand, который возвращает объект типа IMoveCommand
    public class BuilderAI : RobotAI<IMoveCommand>
    {
        int counter = 1;

        public override IMoveCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    // Определяем интерфейс IDevice с обобщенным параметром TCommand, который должен реализовывать интерфейс IMoveCommand
    // и методом ExecuteCommand, который принимает объект типа TCommand и возвращает строку
    public interface IDevice<in TCommand> where TCommand : IMoveCommand
    {
        string ExecuteCommand(TCommand command);
    }

    // Определяем абстрактный класс Device с обобщенным параметром TCommand, который должен реализовывать интерфейс IMoveCommand
    // и методом ExecuteCommand, который принимает объект типа TCommand и возвращает строку
    public abstract class Device<TCommand> : IDevice<TCommand> where TCommand : IMoveCommand
    {
        public abstract string ExecuteCommand(TCommand command);
    }

    // Определяем класс Mover, который наследуется от абстрактного класса Device с обобщенным параметром IMoveCommand
    // и реализует метод ExecuteCommand, который принимает объект типа IMoveCommand и возвращает строку
    public class Mover : Device<IMoveCommand>
    {
        public override string ExecuteCommand(IMoveCommand command)
        {
            if (command == null) throw new ArgumentException();

            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    // Определяем класс ShooterMover, который наследуется от абстрактного класса Device с обобщенным параметром IShooterMoveCommand
    // и реализует метод ExecuteCommand, который принимает объект типа IShooterMoveCommand и возвращает строку
    public class ShooterMover : Device<IShooterMoveCommand>
    {
        public override string ExecuteCommand(IShooterMoveCommand command)
        {
            if (command == null) throw new ArgumentException();

            var hide = command.ShouldHide ? "YES" : "NO";

            return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
        }
    }

    // Определяем статический класс Robot с обобщенным параметром TCommand, который должен реализовывать интерфейс IMoveCommand
    // и методом Create, который принимает объекты типа IRobotAI и IDevice и возвращает объект типа Robot
    public static class Robot
    {
        public static Robot<TCommand> Create<TCommand>(IRobotAI<TCommand> ai, IDevice<TCommand> executor) where TCommand : IMoveCommand
        {
            return new Robot<TCommand>(ai, executor);
        }
    }

    // Определяем класс Robot с обобщенным параметром TCommand, который должен реализовывать интерфейс IMoveCommand
    // и имеет два поля: ai типа IRobotAI и device типа IDevice
    // Класс также имеет конструктор, который принимает объекты типа IRobotAI и IDevice
    // и метод Start, который принимает целочисленный параметр steps и возвращает перечисление строк
    // В методе Start происходит вызов метода GetCommand у объекта ai, который возвращает объект типа TCommand,
    // затем вызывается метод ExecuteCommand у объекта device, который принимает полученный объект типа TCommand
    // и возвращает строку. Эта строка добавляется в перечисление строк, которое возвращается из метода Start
    public class Robot<TCommand> where TCommand : IMoveCommand
    {
        private readonly IRobotAI<TCommand> ai;
        private readonly IDevice<TCommand> device;

        public Robot(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                yield return device.ExecuteCommand(command);
            }
        }
    }
}