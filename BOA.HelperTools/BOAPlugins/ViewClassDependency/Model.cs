﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BOA.Common.Helpers;
using Mono.Cecil;

namespace BOAPlugins.ViewClassDependency
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class Node
    {
        #region Static Fields
        static string ns = "http://schemas.microsoft.com/vs/2009/dgml";
        #endregion

        #region Fields
        readonly TypeDefinition _typeDefinition;
        #endregion

        #region Constructors
        public Node(string value = null)
        {
            Id    = Guid.NewGuid();
            Value = value;
        }

        public Node(FieldReference fieldReference, TypeDefinition typeDefinition)
        {
            _typeDefinition = typeDefinition;
            FieldReference  = fieldReference;
            Id              = Guid.NewGuid();
            Value           = fieldReference.Name;
        }

        public Node(MethodReference methodDefinition, TypeDefinition typeDefinition)
        {
            _typeDefinition = typeDefinition;
            MethodReference = methodDefinition;

            Id = Guid.NewGuid();
            if (IsProperty)
            {
                Value = methodDefinition.Name.RemoveFromStart("set_").RemoveFromStart("get_");
            }
            else
            {
                Value = methodDefinition.Name;
            }

            if (IsBaseMethod)
            {
                Value = "base." + Value;
            }
        }
        #endregion

        #region Public Properties
        public FieldReference FieldReference { get; }
        public Guid           Id             { get; }

        public bool IsField => FieldReference != null;

        public bool IsProperty
        {
            get
            {
                if (MethodDefinition == null)
                {
                    return false;
                }

                return MethodDefinition.IsSetter || MethodDefinition.IsGetter;
            }
        }

        public MethodDefinition MethodDefinition => MethodReference?.Resolve();

        public MethodReference MethodReference { get; }
        public string          Value           { get; }
        #endregion

        #region Properties
        bool IsBaseMethod
        {
            get
            {
                if (MethodDefinition != null)
                {
                    return _typeDefinition != MethodDefinition.DeclaringType;
                }

                return _typeDefinition == FieldReference?.DeclaringType;
            }
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return node != null && node.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public XElement ToDgml()
        {
            var element = new XElement(XName.Get("Node", ns), new XAttribute("Label", Value), new XAttribute("Id", Id));

            if (IsProperty)
            {
                element.Add(new XAttribute("StrokeDashArray", "5,5"));
                element.Add(new XAttribute("Background", "#f2f4f7"));
            }

            if (IsField)
            {
                element.Add(new XAttribute("StrokeDashArray", "5,5"));
                element.Add(new XAttribute("Background", "#90B9F5"));
            }

            return element;
        }
        #endregion
    }

    [DebuggerDisplay("{Source.Value} -{VertexType}-> {Target.Value}")]
    public class Vertex
    {
        #region Static Fields
        static string ns = "http://schemas.microsoft.com/vs/2009/dgml";
        #endregion

        #region Constructors
        public Vertex(Node source, Node target, VertexType vertexType = VertexType.None)
        {
            Source     = source;
            Target     = target;
            VertexType = vertexType;
        }
        #endregion

        #region Public Properties
        public Node Source { get; set; }

        public Node Target { get; set; }

        public VertexType VertexType { get; set; }
        #endregion

        #region Public Methods
        public XElement ToDgml()
        {
            var element = new XElement(XName.Get("Link", ns), new XAttribute("Source", Source.Id), new XAttribute("Target", Target.Id));

            if (VertexType == VertexType.ReadProperty)
            {
                element.Add(new XAttribute("StrokeDashArray", "5,5"));
            }

            return element;
        }
        #endregion
    }

    public enum VertexType
    {
        None,
        ReadProperty,
        True,
        False
    }

    static class Extensions
    {
        #region Public Methods
        public static IEnumerable<Node> ConnectedNodes(this BinaryDecisionTree bdt)
        {
            return bdt.Vertices.SelectMany(v => new[]
                      {
                          v.Source,
                          v.Target
                      })
                      .Distinct()
                      .ToList();
        }
        #endregion
    }

    public class BinaryDecisionTree
    {
        #region Constructors
        static BinaryDecisionTree()
        {
            EntryNode = new Node("Entry");
        }

        public BinaryDecisionTree(string value) : this()
        {
            var node = new Node(value);
            Add(node);
            Add(new Vertex(EntryNode, node));
        }

        internal BinaryDecisionTree()
        {
            Nodes    = new List<Node>();
            Vertices = new List<Vertex>();
        }
        #endregion

        #region Public Properties
        public static Node EntryNode { get; }

        public List<Node> Nodes { get; }

        public List<Vertex> Vertices { get; }
        #endregion

        #region Public Methods
        public void Add(params Node[] node)
        {
            Nodes.AddRange(node);
        }

        public void Add(params Vertex[] vertex)
        {
            Vertices.AddRange(vertex);
        }

        public void Remove(Node node)
        {
            Nodes.Remove(node);
        }

        public void Remove(Vertex vertex)
        {
            Vertices.Remove(vertex);
        }
        #endregion
    }

    public static class DgmlHelper
    {
        #region Static Fields
        static string ns = "http://schemas.microsoft.com/vs/2009/dgml";
        #endregion

        #region Public Methods
        public static XElement ToDgml(this BinaryDecisionTree bdt)
        {
            var nodes =
                from n in bdt.ConnectedNodes()
                select n.ToDgml();
            var links =
                from v in bdt.Vertices
                select v.ToDgml();
            return CreateGraph(nodes, links);
        }
        #endregion

        #region Methods
        static XElement CreateGraph(IEnumerable<XElement> nodes, IEnumerable<XElement> links)
        {
            var xElement = new XElement(XName.Get("DirectedGraph", ns));
            xElement.SetAttributeValue("GraphDirection", "LeftToRight");
            xElement.SetAttributeValue("Layout", "Sugiyama");

            var xElement2 = new XElement(XName.Get("Nodes", ns));
            var xElement3 = new XElement(XName.Get("Links", ns));
            xElement2.Add(nodes.Cast<object>().ToArray());
            xElement3.Add(links.Cast<object>().ToArray());
            xElement.Add(xElement2);
            xElement.Add(xElement3);

            xElement.SetDirectionLeftToRight();

            return xElement;
        }

        public static void SetDirectionLeftToRight( string graphFilePath)
        {
            var xElement = XElement.Parse(File.ReadAllText(graphFilePath));
            xElement.SetDirectionLeftToRight();

            xElement.Save(graphFilePath);
        }

        static void SetDirectionLeftToRight(this XElement xElement)
        {
            XmlHelper.AddAttribute(xElement, "DirectedGraph", "Layout", "Sugiyama");
            XmlHelper.AddAttribute(xElement, "DirectedGraph", "GraphDirection", "LeftToRight");

            XmlHelper.AddAttribute(xElement, "Properties/Property[Id]=GraphDirection", "DataType", "Microsoft.VisualStudio.Diagrams.Layout.LayoutOrientation");
            XmlHelper.AddAttribute(xElement, "Properties/Property[Id]=Layout", "DataType", "System.String");
        }
        #endregion
    }
}