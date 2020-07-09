using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
    /// <summary>
    /// A node is one of the core elements in the OpenStreetMap data model. It consists of a single point in space defined by its latitude, longitude and node id.
    /// </summary>
    [XmlType]
    public class OsmNode : OsmDataPrimitive
    {
        /// <summary>
        /// Latitude coordinate in degrees (North of equator is positive) using the standard WGS84 projection. Some applications may not accept latitudes above/below <a href="http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames#X_and_Y">±85 degrees</a> for some projections.
        /// </summary>
        [XmlAttribute("lat")]
        public decimal LatitudeWgs84 { get; set; }

        /// <summary>
        /// Longitude coordinate in degrees (East of Greenwich is positive) using the standard WGS84 projection. Note that the geographic poles will be exactly at latitude ±90 degrees but in that case the longitude will be set to an arbitrary value within this range. 
        /// </summary>
        [XmlAttribute("lon")]
        public decimal LongitideWgs84 { get; set; }

        /// <summary>
        /// See the <a href="http://wiki.openstreetmap.org/wiki/Map_Features">Map Features</a> wiki page for tagging guidelines.
        /// </summary>
        [XmlElement("tag")]
        public OsmTag[] Tags { get; set; }
    }
}
