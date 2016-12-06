using System.Xml.Serialization;

namespace THNETII.OpenStreetMap.Osm
{
    /// <summary>
    /// A tag consists of two items, a key and a value. Tags describe specific <a href="http://wiki.openstreetmap.org/wiki/Features">features</a> of map <a href="http://wiki.openstreetmap.org/wiki/Element">elements</a> (<a href="http://wiki.openstreetmap.org/wiki/Nodes">nodes</a>, <a href="http://wiki.openstreetmap.org/wiki/Ways">ways</a>, or <a href="http://wiki.openstreetmap.org/wiki/Relations">relations</a>) or <a href="http://wiki.openstreetmap.org/wiki/Changesets"></a>changesets. Both items are free format text fields, but often represent numeric or other structured items. Conventions are agreed on the meaning and use of tags, which are captured on the OpenStreetMap wiki.
    /// </summary>
    public class OsmTag
    {
        /// <summary>
        /// The key, therefore, is used to describe a topic, category, or type of feature (e.g., <a href="http://wiki.openstreetmap.org/wiki/Highway">highway</a> or <a href="http://wiki.openstreetmap.org/wiki/Name">name</a>). Keys can be qualified with prefixes, infixes, or suffixes (usually, separated with a colon, :), forming super- or sub-categories, or <a href="http://wiki.openstreetmap.org/wiki/Namespace">namespace</a>. Common namespaces are language specification and a <a href="http://wiki.openstreetmap.org/wiki/Date_namespace">date namespace</a> specification for name keys.
        /// </summary>
        [XmlAttribute("k")]
        public string Key { get; set; }

        /// <summary>
        /// The value details the specific form of the key-specified feature. Commonly, values are free form text (e.g., name="Jeff Memorial Highway"), one of a set of distinct values (an enumeration; e.g., highway=motorway), multiple values from an enumeration (separated by a <a href="http://wiki.openstreetmap.org/wiki/Semi-colon_value_separator">semicolon</a>), or a number (integer or decimal), such as a distance.
        /// </summary>
        [XmlAttribute("v")]
        public string Value { get; set; }
    }
}
