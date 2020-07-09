using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
    /// <summary>
    /// Within the OSM database, we store these attributes for nodes, ways and relations. Your application may not need to make use of all of them, and some third-party extracts produced from OSM data may not reproduce them all. 
    /// </summary>
    /// <remarks>
    /// <para>Original OpenStreetMap Wiki page: <a href="http://wiki.openstreetmap.org/wiki/Elements#Common_attributes">Common Attributes (Elements)</a></para>
    /// </remarks>
    [XmlType]
    public abstract class OsmDataPrimitive
    {
        /// <summary>
        /// Used for identifying the element. Element types have their own ID space, so there could be a node with id=100 and a way with id=100, which are unlikely to be related or geographically near to each other.
        /// </summary>
        [XmlAttribute("id")]
        public ulong Id { get; set; }

        /// <summary>
        /// The display name of the user who last modified the object. A user can change their display name.
        /// </summary>
        [XmlAttribute("user")]
        public string Username { get; set; }

        /// <summary>
        /// The numeric user id. of the user who last modified the object. A user's id. never changes.
        /// </summary>
        [XmlAttribute("uid")]
        public uint UserId { get; set; }

        /// <summary>
        /// Time of the last modification.
        /// </summary>
        [XmlAttribute("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Whether the object is deleted or not in the database, if visible="false" then the object should only be returned by history calls.
        /// </summary>
        [XmlAttribute("visible")]
        [DefaultValue(true)]
        public bool Visible { get; set; }

        /// <summary>
        /// The edit version of the object. Newly created objects start at version 1 and the value is incremented by the server when a client uploads a new version of the object. The server will reject a new version of an object if the version sent by the client does not match the current version of the object in the database.
        /// </summary>
        [XmlAttribute("version")]
        public int Version { get; set; }

        /// <summary>
        /// The <a href="http://wiki.openstreetmap.org/wiki/Changeset">changeset</a> in which the object was created or updated.
        /// </summary>
        [XmlAttribute("changeset")]
        public ulong Changeset { get; set; }

        /// <summary>
        /// Default Base Constructor for common OSM Data Primitive Types
        /// </summary>
        protected OsmDataPrimitive()
        {
            Visible = true;
        }
    }
}
