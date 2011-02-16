using Microsoft.Xna.Framework.Content.Pipeline;
using SGDeContent.DataTypes;
using System.Xml;
using System.IO;

namespace SGDeContent
{
    [LocalizedContentImporter(new string[] { ".svg", ".svgz" }, "SVGImporter", DefaultProcessor = "SVGProcessor")]
    public class SVGImport : ContentImporter<Content>
    {
        public override Content Import(string filename, ContentImporterContext context)
        {
            Stream iSt = new FileStream(filename, FileMode.Open);
            if (Path.GetExtension(filename).ToLower().Equals(".svgz"))
            {
                iSt = new System.IO.Compression.GZipStream(iSt, System.IO.Compression.CompressionMode.Decompress);
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(iSt);
            iSt.Close();
            return new Content(doc, ContentTypes.SVG, double.Parse(doc.DocumentElement.Attributes["version"].Value));
        }
    }
}
