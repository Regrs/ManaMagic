using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using ZwellTech.SuperNintendo.Drawing;

#nullable enable

namespace ZwellTech.Xml
{
    public static class XmlHelper
    {
        public const string IndexAttributeName = "index";
        public const string ValueAttributeName = "value";

        // ---------------------------------------------------------------------------------

        public readonly struct XmlDocumentNode : IDisposable
        {
            private readonly XmlWriter writer;

            public XmlDocumentNode(XmlWriter writer)
            {
                this.writer = writer;
                writer.WriteStartDocument();
            }

            public void Dispose()
            {
                writer.WriteEndDocument();
            }
        }
        public readonly struct XmlElementNode : IDisposable
        {
            private readonly XmlWriter writer;

            public XmlElementNode(XmlWriter writer, string elementName)
            {
                this.writer = writer;
                writer.WriteStartElement(elementName);
            }

            public void Dispose()
            {
                writer.WriteEndElement();
            }
        }

        public static XmlDocumentNode CreateDocumentNode(this XmlWriter writer)
        {
            return new XmlDocumentNode(writer);
        }

        public static XmlElementNode CreateElementtNode(this XmlWriter writer, string elementName)
        {
            return new XmlElementNode(writer, elementName);
        }

        // ---------------------------------------------------------------------------------

        public static XmlNode GetElementByTagName(XmlDocument document, string tagName)
        {
            XmlNode? node = document.GetElementsByTagName(tagName)[0];
            if (node == null)
            {
                ThrowHelper.ThrowInvalidOperationException($"Element '{tagName}' not found");
            }

            return node;
        }

        public static XmlNode GetElementByTagName(XmlNode node, string tagName)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == tagName)
                {
                    return childNode;
                }
            }
            return (XmlNode)ThrowHelper.ThrowInvalidOperationException($"Element '{tagName}' not found"); ;
        }

        public static bool TryGetElementByTagName(XmlNode searchNode, string tagName, [NotNullWhen(true)] out XmlNode? outNode)
        {
            foreach (XmlNode childNode in searchNode.ChildNodes)
            {
                if (childNode.Name == tagName)
                {
                    outNode = childNode;
                    return true;
                }
            }
            outNode = null;
            return false;
        }

        public static string ReadAttribute(XmlNode node, string attribute)
        {
            if (node.Attributes == null)
            {
                ThrowHelper.ThrowInvalidOperationException($"Attribute '{attribute}' not found");
            }

            XmlAttribute? xmlAttribute = node.Attributes[attribute];
            if (xmlAttribute == null)
            {
                ThrowHelper.ThrowInvalidOperationException($"Attribute '{attribute}' not found");
            }

            return xmlAttribute.Value;
        }

        public static bool TryReadAttribute(XmlNode node, string attribute, out string value)
        {
            value = string.Empty;
            if (node.Attributes != null)
            {
                XmlAttribute? xmlAttribute = node.Attributes[attribute];
                if (xmlAttribute != null)
                {
                    value = xmlAttribute.Value;
                    return true;
                }
            }


            return false;
        }

        public static byte ReadAttributeAsByte(XmlNode node, string attribute)
        {
            return Convert.ToByte(XmlHelper.ReadAttribute(node, attribute), CultureInfo.InvariantCulture);
        }

        public static ushort ReadAttributeAsUInt16(XmlNode node, string attribute)
        {
            return Convert.ToUInt16(XmlHelper.ReadAttribute(node, attribute), CultureInfo.InvariantCulture);
        }

        public static Rgb555Color ReadAttributeAsRgb555Color(XmlNode node, string attribute)
        {
            return Rgb555Color.FromRgb(XmlHelper.ReadAttributeAsUInt16(node, attribute));
        }
    }
}
