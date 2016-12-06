using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
    /// <summary>
    /// A member of a relation can optionally have a role which describes the part that a particular feature plays within a relation.
    /// </summary>
    [XmlType]
    public class OsmRelationMember
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlAttribute("type")]
        public OsmDataPrimitiveType MemberType { get; set; }

        [XmlAttribute("ref")]
        public ulong ReferenceId { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// A role is an optional textual field describing the function of a member of the relation. For example, in North America, <c>east</c> indicates that a way would be <a href="http://wiki.openstreetmap.org/wiki/Highway_Directions_In_The_United_States">posted as <strong>East</strong></a> on the directional plate of a route numbering shield. Or, <a href="http://wiki.openstreetmap.org/wiki/Relation:multipolygon">multipolygon relation</a>, <c>inner</c> and <c>outer</c> are used to specify whether a way forms the inner or outer part of that polygon.
        /// </summary>
        [XmlAttribute("role")]
        public string Role { get; set; }
    }
}