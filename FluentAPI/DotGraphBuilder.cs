using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace FluentApi.Graph
{
    /*
     * В данном коде определены классы и методы для работы с графами в формате DOT с помощью Fluent API.

        Класс DotGraphBuilder представляет собой построитель графа в формате DOT. Он содержит методы для добавления узлов и ребер в граф, а также метод для построения графа в формате DOT.

        Класс Builder является базовым классом для узлов и ребер графа. Он содержит методы для добавления узла и ребра в граф, а также метод для построения графа в формате DOT.

        Классы NodeBuilder и EdgeBuilder наследуются от класса Builder и представляют собой построители узлов и ребер графа соответственно. Они содержат методы для добавления атрибутов к узлам и ребрам графа, таких как цвет, метка, размер шрифта, форма и вес.

        Статический класс BuildeStatic содержит методы расширения для класса Builder, которые позволяют добавлять атрибуты к узлам и ребрам графа. Эти методы используют обобщенный тип TObj, который ограничен классом Builder , чтобы гарантировать, что они могут использоваться только с объектами, которые являются построителями узлов или ребер графа.
     */

    // Перечисление для определения формы узла
    public enum NodeShape
    {
        Box,
        Ellipse
    }

    // Класс для построения графа в формате DOT
    public class DotGraphBuilder
    {
        private readonly Graph graph;

        private DotGraphBuilder(Graph graph)
        {
            this.graph = graph;
        }

        // Метод для создания направленного графа
        public static DotGraphBuilder DirectedGraph(string name) => new DotGraphBuilder(new Graph(name, true, false));

        // Метод для создания ненаправленного графа
        public static DotGraphBuilder UndirectedGraph(string name) => new DotGraphBuilder(new Graph(name, false, false));

        // Метод для добавления узла в граф
        public NodeBuilder AddNode(string name)
        {
            graph.AddNode(name);
            return new NodeBuilder(this, graph.Nodes.Last().Attributes);
        }

        // Метод для добавления ребра в граф
        public EdgeBuilder AddEdge(string from, string to)
        {
            graph.AddEdge(from, to);
            return new EdgeBuilder(this, graph.Edges.Last().Attributes);
        }

        // Метод для построения графа в формате DOT
        public string Build() => graph.ToDotFormat();
    }

    // Статический класс для добавления атрибутов к узлам и ребрам графа
    public static class BuildeStatic
    {
        // Метод для добавления цвета к узлу или ребру
        public static TObj Color<TObj>(this TObj obj, string color)
            where TObj : Builder
        {
            obj._attributes["color"] = color;
            return obj;
        }

        // Метод для добавления метки к узлу или ребру
        public static TObj Label<TObj>(this TObj obj, string label)
            where TObj : Builder
        {
            obj._attributes["label"] = $"{label.Replace("\"", "\\\"")}";
            return obj;
        }

        // Метод для добавления размера шрифта к узлу или ребру
        public static TObj FontSize<TObj>(this TObj obj, int size)
                where TObj : Builder
        {
            obj._attributes["fontsize"] = size.ToString();
            return obj;
        }

        // Метод для добавления формы узла
        public static TObj Shape<TObj>(this TObj obj, NodeShape shape)
        where TObj : Builder
        {
            obj._attributes["shape"] = shape.ToString().ToLower();
            return obj;
        }

        // Метод для добавления веса ребра
        public static TObj Weight<TObj>(this TObj obj, double weight)
            where TObj : Builder
        {
            obj._attributes["weight"] = weight.ToString();
            return obj;
        }

        // Метод для добавления нескольких атрибутов к узлу или ребру
        public static TObj With<TObj>(this TObj obj, Action<TObj> action)
            where TObj : Builder
        {
            action(obj);
            return obj;
        }
    }

    // Базовый класс для узлов и ребер графа
    public class Builder
    {
        internal DotGraphBuilder _graphBuilder;
        internal Dictionary<string, string> _attributes = new Dictionary<string, string>();

        internal Builder(DotGraphBuilder builder, Dictionary<string, string> attributes)
        {
            _graphBuilder = builder;
            _attributes = attributes;
        }

        // Метод для добавления узла в граф
        internal NodeBuilder AddNode(string name)
        {
            return _graphBuilder.AddNode(name);
        }

        // Метод для добавления ребра в граф
        internal EdgeBuilder AddEdge(string from, string to)
        {
            return _graphBuilder.AddEdge(from, to);
        }

        // Метод для построения графа в формате DOT
        internal string Build()
        {
            return _graphBuilder.Build();
        }
    }

    // Класс для построения узла графа
    public class NodeBuilder : Builder
    {
        public NodeBuilder(DotGraphBuilder builder, Dictionary<string, string> nodeAttributes) : base(builder, nodeAttributes) { }
    }

    //Класс для построения ребра графа
    public class EdgeBuilder : Builder
    {
        public EdgeBuilder(DotGraphBuilder builder, Dictionary<string, string> nodeAttributes) : base(builder, nodeAttributes) { }
    }
}
