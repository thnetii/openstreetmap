using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
    /// <summary>
    /// A way is an ordered list of nodes which normally also has at least one tag or is included within a Relation. A way can have between 2 and 2,000 nodes, although it's possible that faulty ways with zero or a single node exist. A way can be open or closed. A closed way is one whose last node on the way is also the first on that way. A closed way may be interpreted either as a closed polyline, or an area, or both.
    /// </summary>
    [XmlType]
    public class OsmWay : OsmDataPrimitive
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlElement("nd")]
        public OsmWayNodeReference[] Nodes { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// See the <a href="http://wiki.openstreetmap.org/wiki/Map_Features">Map Features</a> wiki page for tagging guidelines.
        /// </summary>
        [XmlElement("tag")]
        public OsmTag[] Tags { get; set; }
    }
}
