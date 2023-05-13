// Импортируем необходимые пространства имен
using System;
using System.Collections.Generic;
using System.Linq;

// Объявляем пространство имен Delegates.TreeTraversal
namespace Delegates.TreeTraversal
{
    // Объявляем статический класс Traversal
    public static class Traversal
    {
        // Метод GetProducts принимает корневую категорию продуктов и возвращает список всех продуктов, находящихся в этой категории и ее подкатегориях
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            var products = new List<Product>();
            // Вызываем метод TraverseProductCategories, передавая ему корневую категорию продуктов и делегат, который добавляет все продукты из категории в список products
            TraverseProductCategories(root, pc => products.AddRange(pc.Products));
            return products;
        }

        // Метод GetEndJobs принимает корневую задачу и возвращает список всех задач, у которых нет подзадач
        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            var jobs = new List<Job>();
            // Вызываем метод TraverseJobs, передавая ему корневую задачу и делегат, который проверяет, есть ли у задачи подзадачи, и если нет, то добавляет задачу в список jobs
            TraverseJobs(root, j =>
            {
                if (j.Subjobs.Count == 0)
                    jobs.Add(j);
            });
            return jobs;
        }

        // Метод GetBinaryTreeValues принимает корневой узел бинарного дерева и возвращает список всех значений в листьях этого дерева
        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            var values = new List<T>();
            // Вызываем метод TraverseBinaryTree, передавая ему корневой узел бинарного дерева и делегат, который проверяет, является ли узел листом (то есть не имеет дочерних узлов), и если да, то добавляет его значение в список values
            TraverseBinaryTree(root, node =>
            {
                if (node.Left == null && node.Right == null)
                    values.Add(node.Value);
            });
            return values;
        }

        // Метод TraverseProductCategories рекурсивно обходит дерево категорий продуктов, вызывая делегат action для каждой категории
        private static void TraverseProductCategories(ProductCategory category, Action<ProductCategory> action)
        {
            action(category);
            foreach (var subcategory in category.Categories)
                TraverseProductCategories(subcategory, action);
        }

        // Метод TraverseJobs рекурсивно обходит дерево задач, вызывая делегат action для каждой задачи
        private static void TraverseJobs(Job job, Action<Job> action)
        {
            action(job);
            foreach (var subjob in job.Subjobs)
                TraverseJobs(subjob, action);
        }

        // Метод TraverseBinaryTree рекурсивно обходит бинарное дерево, вызывая делегат action для каждого узла
        private static void TraverseBinaryTree<T>(BinaryTree<T> node, Action<BinaryTree<T>> action)
        {
            action(node);
            if (node.Left != null)
                TraverseBinaryTree(node.Left, action);
            if (node.Right != null)
                TraverseBinaryTree(node.Right, action);
        }
    }
}