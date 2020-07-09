using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
    /// <summary>
    /// Enumeration to differentiate between different OSM Data Primitives.
    /// </summary>
    public enum OsmDataPrimitiveType
    {
        /// <summary>
        /// The Type of the OSM Data Primitive is not known.
        /// </summary>
        [XmlEnum("")]
        Unknown = 0,

        /// <summary>
        /// A <a href="http://wiki.openstreetmap.org/wiki/Node">node</a> represents a specific point on the earth's surface defined by its latitude and longitude.
        /// </summary>
        [XmlEnum("node")]
        Node,

        /// <summary>
        /// A <a href="http://wiki.openstreetmap.org/wiki/Way">way</a> is an ordered list of between 2 and 2,000 nodes that define a <a href="http://en.wikipedia.org/wiki/Polygonal_chain">polyline</a>. Ways are used to represent linear features such as rivers and roads.
        /// </summary>
        [XmlEnum("way")]
        Way,

        /// <summary>
        /// A <a href="http://wiki.openstreetmap.org/wiki/Relation">relation</a> is a multi-purpose data structure that documents a relationship between two or more data <a href="http://wiki.openstreetmap.org/wiki/Element">elements</a> (nodes, ways, and/or other relations).
        /// </summary>
        [XmlEnum("relation")]
        Relation
    }
}
