using System;
using System.Linq.Expressions;

namespace Reflection.Differentiation
{
    public static class Algebra
    {
        // Метод Differentiate принимает выражение в виде лямбда-функции и возвращает выражение, представляющее производную этой функции.
        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function)
        {
            // Получаем тело функции и параметр
            var body = function.Body;
            var parameter = function.Parameters[0];
            // Создаем экземпляр класса DifferentiationVisitor и вызываем его метод Visit для тела функции
            var visitor = new DifferentiationVisitor(parameter);
            var derivative = visitor.Visit(body);
            // Возвращаем новую лямбда-функцию, представляющую производную
            return Expression.Lambda<Func<double, double>>(derivative, parameter);
        }

        // Класс DifferentiationVisitor наследуется от ExpressionVisitor и реализует методы для обхода дерева выражения и замены его узлов на соответствующие выражения производной.
        private class DifferentiationVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public DifferentiationVisitor(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            // Метод VisitConstant заменяет константный узел на константу 0.0
            protected override Expression VisitConstant(ConstantExpression node)
            {
                return Expression.Constant(0.0);
            }

            // Метод VisitParameter заменяет параметр на константу 1.0
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return Expression.Constant(1.0);
            }

            // Метод VisitBinary реализует формулы дифференцирования для бинарных операций
            protected override Expression VisitBinary(BinaryExpression node)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        return Expression.Add(Visit(node.Left), Visit(node.Right));
                    case ExpressionType.Subtract:
                        return Expression.Subtract(Visit(node.Left), Visit(node.Right));
                    case ExpressionType.Multiply:
                        return Expression.Add(Expression.Multiply(Visit(node.Left), node.Right), Expression.Multiply(node.Left, Visit(node.Right)));
                    case ExpressionType.Divide:
                        return Expression.Divide(Expression.Subtract(Expression.Multiply(Visit(node.Left), node.Right), Expression.Multiply(node.Left, Visit(node.Right))), Expression.Multiply(node.Right, node.Right));
                    default:
                        throw new ArgumentException($"Unsupported binary operator: {node.NodeType}");
                }
            }

            // Метод VisitMethodCall реализует формулы дифференцирования для вызовов методов Sin и Cos
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                switch (node.Method.Name)
                {
                    case "Sin":
                        return Expression.Multiply(Expression.Call(typeof(Math).GetMethod("Cos", new[] { typeof(double) }), node.Arguments[0]), Visit(node.Arguments[0]));
                    case "Cos":
                        return Expression.Multiply(Expression.Constant(-1.0), Expression.Multiply(Expression.Call(typeof(Math).GetMethod("Sin", new[] { typeof(double) }), node.Arguments[0]), Visit(node.Arguments[0])));
                    default:
                        throw new ArgumentException($"Unknown function: {node.Method.Name}");
                }
            }

            // Метод VisitUnary реализует формулы дифференцирования для унарных операций
            protected override Expression VisitUnary(UnaryExpression node)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Negate:
                    case ExpressionType.Convert:
                        return Expression.Negate(Visit(node.Operand));
                    default:
                        throw new ArgumentException($"Unsupported unary operator: {node.NodeType}");
                }
            }
        }
    }
}