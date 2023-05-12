using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        // Метод для получения списка продуктов из дерева категорий продуктов
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            var products = new List<Product>();
            TraverseProductCategories(root, pc => products.AddRange(pc.Products));
            return products;
        }

        // Метод для получения списка задач, у которых нет подзадач, из дерева задач
        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            var jobs = new List<Job>();
            // Обход дерева задач
            TraverseJobs(root, j =>
            {
                if (j.Subjobs.Count == 0)
                    jobs.Add(j);
            });
            return jobs;
        }

        // Метод для получения списка значений в листьях бинарного дерева
        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            var values = new List<T>();
            // Обход бинарного дерева
            TraverseBinaryTree(root, node =>
            {
                if (node.Left == null && node.Right == null)
                    values.Add(node.Value);
            });
            return values;
        }

        // Метод для обхода дерева категорий продуктов
        private static void TraverseProductCategories(ProductCategory category, Action<ProductCategory> action)
        {
            action(category);
            foreach (var subcategory in category.Categories)
                TraverseProductCategories(subcategory, action);
        }

        // Метод для обхода дерева задач
        private static void TraverseJobs(Job job, Action<Job> action)
        {
            action(job);
            foreach (var subjob in job.Subjobs)
                TraverseJobs(subjob, action);
        }

        // Метод для обхода бинарного дерева
        private static void TraverseBinaryTree<T>(BinaryTree<T> node, Action<BinaryTree<T>> action)
        {
            action(node);
            // Рекурсивно обходим левое поддерево
            if (node.Left != null)
                TraverseBinaryTree(node.Left, action);
            // Рекурсивно обходим правое поддерево
            if (node.Right != null)
                TraverseBinaryTree(node.Right, action);
        }
    }
}