using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
            try { return RefNodeId == Convert.ToUInt64(obj, CultureInfo.InvariantCulture); }
            catch (InvalidCastException) { return Equals(obj as OsmWayNodeReference); }
        }

        public bool Equals(OsmWayNodeReference other)
        {
            switch (other)
            {
                case null:
                    return false;
                case OsmWayNodeReference _ when ReferenceEquals(this, other):
                case OsmWayNodeReference _ when RefNodeId == other.RefNodeId:
                    return true;
                default:
                    return false;
            }
        }

        public bool Equals(ulong other) => other == (ulong)this;

        [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = nameof(AsUInt64))]
        public static explicit operator ulong(OsmWayNodeReference @ref)
        {
            if (@ref is null)
                return 0UL;
            return @ref.RefNodeId;
        }

        public ulong AsUInt64() => RefNodeId;

        public static bool operator ==(OsmWayNodeReference x, OsmWayNodeReference y)
        {
            if (x is null)
                return y is null;
            return x.Equals(y);
        }

        public static bool operator !=(OsmWayNodeReference x, OsmWayNodeReference y)
        {
            if (x is null)
                return y is OsmWayNodeReference;
            return !x.Equals(y);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}