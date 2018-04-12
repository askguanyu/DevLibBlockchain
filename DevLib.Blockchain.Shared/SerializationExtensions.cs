namespace DevLib.Blockchain
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Serialization Extensions.
    /// </summary>
    public static class SerializationExtensions
    {
        /// <summary>
        /// Serializes object to Xml string.
        /// </summary>
        /// <remarks>
        /// The object to be serialized should be decorated with the <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
        /// </remarks>
        /// <param name="source">The object to serialize.</param>
        /// <param name="indent">Whether to write individual elements on new lines and indent.</param>
        /// <param name="omitXmlDeclaration">Whether to write an Xml declaration.</param>
        /// <param name="removeDefaultNamespace">Whether to write default namespace.</param>
        /// <param name="extraTypes">A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <returns>An Xml encoded string representation of the source object.</returns>
        public static string SerializeXmlString(this object source, bool indent = false, bool omitXmlDeclaration = true, bool removeDefaultNamespace = true, Type[] extraTypes = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            XmlSerializer xmlSerializer = (extraTypes == null || extraTypes.Length == 0) ? new XmlSerializer(source.GetType()) : new XmlSerializer(source.GetType(), extraTypes);

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader streamReader = new StreamReader(memoryStream))
            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings() { OmitXmlDeclaration = omitXmlDeclaration, Indent = indent, Encoding = new UTF8Encoding(false), CloseOutput = true }))
            {
                if (removeDefaultNamespace)
                {
                    XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                    xmlns.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(xmlWriter, source, xmlns);
                }
                else
                {
                    xmlSerializer.Serialize(xmlWriter, source);
                }

                xmlWriter.Flush();
                memoryStream.Position = 0;

                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Deserializes Xml string to object.
        /// </summary>
        /// <param name="source">The Xml string to deserialize.</param>
        /// <param name="type">Type of object.</param>
        /// <param name="extraTypes">A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <returns>Instance of object.</returns>
        public static object DeserializeXmlString(this string source, Type type, Type[] extraTypes = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            XmlSerializer xmlSerializer = (extraTypes == null || extraTypes.Length == 0) ? new XmlSerializer(type) : new XmlSerializer(type, extraTypes);

            using (StringReader stringReader = new StringReader(source))
            {
                return xmlSerializer.Deserialize(stringReader);
            }
        }

        /// <summary>
        /// Deserializes Xml string to object.
        /// </summary>
        /// <param name="source">The Xml string to deserialize.</param>
        /// <param name="knownTypes">A <see cref="T:System.Type" /> array of object types to serialize.</param>
        /// <returns>Instance of object.</returns>
        public static object DeserializeXmlString(this string source, Type[] knownTypes)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (knownTypes == null || knownTypes.Length < 1)
            {
                throw new ArgumentException("knownTypes is null or empty.", "knownTypes");
            }

            Type sourceType = null;

            using (StringReader stringReader = new StringReader(source))
            {
                string rootNodeName = XElement.Load(stringReader).Name.LocalName;

                sourceType = knownTypes.FirstOrDefault(p => p.Name.Equals(rootNodeName, StringComparison.OrdinalIgnoreCase));

                if (sourceType == null)
                {
                    throw new InvalidOperationException();
                }
            }

            XmlSerializer xmlSerializer = new XmlSerializer(sourceType, knownTypes);

            using (StringReader stringReader = new StringReader(source))
            {
                return xmlSerializer.Deserialize(stringReader);
            }
        }

        /// <summary>
        /// Deserializes Xml string to object.
        /// </summary>
        /// <typeparam name="T">Type of the returns object.</typeparam>
        /// <param name="source">The Xml string to deserialize.</param>
        /// <param name="extraTypes">A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <returns>Instance of T.</returns>
        public static T DeserializeXmlString<T>(this string source, Type[] extraTypes = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            XmlSerializer xmlSerializer = (extraTypes == null || extraTypes.Length == 0) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), extraTypes);

            using (StringReader stringReader = new StringReader(source))
            {
                return (T)xmlSerializer.Deserialize(stringReader);
            }
        }
    }
}
