using System;
using System.Collections;
using System.Collections.Generic;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        // Объявляем свойства Left, Right и Value, которые возвращают соответствующие значения узла root
        private Node<T> root = null;
        public Node<T> Left { get => root.Left; }
        public Node<T> Right { get => root.Right; }
        public T Value { get => root.Value; }

        // Объявляем метод Add, который добавляет новый узел в дерево
        public void Add(T value)
        {
            // Если корневой узел пустой, то создаем новый узел и делаем его корневым
            if (root == null)
                root = new Node<T>(value);
            // Иначе вызываем метод AddTo для добавления нового узла в дерево
            else
                AddTo(root, value);
        }

        // Объявляем метод AddTo, который добавляет новый узел в дерево, начиная с заданного узла
        private void AddTo(Node<T> node, T value)
        {
            // Если значение нового узла меньше значения текущего узла, то добавляем его в левое поддерево
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                    node.Left = new Node<T>(value);
                else
                    AddTo(node.Left, value);
            }
            // Если значение нового узла больше значения текущего узла, то добавляем его в правое поддерево
            else if (value.CompareTo(node.Value) > 0)
            {
                if (node.Right == null)
                    node.Right = new Node<T>(value);
                else
                    AddTo(node.Right, value);
            }
            // Если значение нового узла равно значению текущего узла, то добавляем его в левое поддерево
            else 
            {
                if (node.Left == null)
                    node.Left = new Node<T>(value);
                else
                    AddTo(node.Left, value);
            }
        }

        // Реализуем метод GetEnumerator, который возвращает перечислитель для обхода дерева в порядке возрастания значений
        public IEnumerator<T> GetEnumerator()
        {
            if (root != null)
                foreach (T value in TraverseInOrder(root))
                    yield return value;
        }

        // Реализуем метод IEnumerable.GetEnumerator, который возвращает перечислитель для обхода дерева в порядке возрастания значений
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Объявляем метод TraverseInOrder, который обходит дерево в порядке возрастания значений и возвращает значения узлов
        private IEnumerable<T> TraverseInOrder(Node<T> node)
        {
            if (node.Left != null)
                foreach (T value in TraverseInOrder(node.Left))
                    yield return value;

            yield return node.Value;

            if (node.Right != null)
                foreach (T value in TraverseInOrder(node.Right))
                    yield return value;
        }


    // Объявляем вложенный класс Node, который представляет узел дерева и принимает тип TNode, который должен реализовывать интерфейс IComparable<TNode>
    public class Node<TNode> where TNode : IComparable<TNode>
    {
        // Объявляем свойства Value, Left и Right для узла
        public TNode Value { get; private set; }
        public Node<TNode> Left { get; set; }
        public Node<TNode> Right { get; set; }

            public Node(TNode value)
            {
                Value = value;
                Left = null;
                Right = null;
            }
        }
    }

    // Объявляем статический класс BinaryTree, который содержит метод Create для создания нового дерева
    static public class BinaryTree
    {
        // Объявляем статический метод Create, который принимает массив значений и возвращает новое дерево
        static public BinaryTree<T> Create<T>(params T[] values) where T : IComparable<T>
        {
            // Создаем новое дерево
            var tree = new BinaryTree<T>();
            // Добавляем каждое значение из массива в дерево
            foreach (T value in values)
                tree.Add(value);
            // Возвращаем новое дерево
            return tree;
        }
    }
}