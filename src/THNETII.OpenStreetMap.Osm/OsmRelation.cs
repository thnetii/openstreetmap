using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
    /// <summary>
    /// A relation is one of the core data elements that consists of one or more tags and also an ordered list of one or more nodes, ways and/or relations as members which is used to define logical or geographic relationships between other elements. A member of a relation can optionally have a role which describes the part that a particular feature plays within a relation.
    /// </summary>
    [XmlType]
    public class OsmRelation : OsmDataPrimitive
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlElement("member")]
        public OsmRelationMember[] Members { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// See the <a href="http://wiki.openstreetmap.org/wiki/Map_Features">Map Features</a> wiki page for tagging guidelines.
        /// </summary>
        [XmlElement("tag")]
        public OsmTag[] Tags { get; set; }
    }
}
