using System;
using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [XmlType]
    public class OsmWayNodeReference : IEquatable<OsmWayNodeReference>, IEquatable<ulong>
    {
        [XmlAttribute("ref")]
        public ulong RefNodeId { get; set; }

        public override int GetHashCode() => RefNodeId.GetHashCode();

        public override bool Equals(object obj)
        {
            try { return RefNodeId == Convert.ToUInt64(obj); }
            catch (InvalidCastException) { return Equals(obj as OsmWayNodeReference); }
        }

        public bool Equals(OsmWayNodeReference other)
        {
            if (ReferenceEquals(null, other))
                return false;
            else if (ReferenceEquals(this, other))
                return true;
            else
                return RefNodeId == other.RefNodeId;
        }

        public bool Equals(ulong other) => other == (ulong)this;

        public static explicit operator ulong(OsmWayNodeReference @ref)
        {
            if (ReferenceEquals(null, @ref))
                return 0UL;
            return @ref.RefNodeId;
        }

        public static bool operator ==(OsmWayNodeReference x, OsmWayNodeReference y)
        {
            if (ReferenceEquals(null, x))
                return ReferenceEquals(null, y);
            return x.Equals(y);
        }

        public static bool operator !=(OsmWayNodeReference x, OsmWayNodeReference y)
        {
            if (ReferenceEquals(null, x))
                return !ReferenceEquals(null, y);
            return !x.Equals(y);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}