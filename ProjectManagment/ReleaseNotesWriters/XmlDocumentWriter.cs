using ProjectManagment.Migrations;
using ProjectManagment.Models;
using System.Xml;

namespace ProjectManagment.ReleaseNotesWriters
{
    public class XmlDocumentWriter : DocumentWriter
    {
        private XmlWriter xmlWriter;

        public override MemoryStream Write(IEnumerable<ReleaseNotesIssues> issues)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Replace,
                ConformanceLevel = ConformanceLevel.Document,
            };

            var memoryStream = new MemoryStream();

            using (xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                this.WriteHeader();

                this.WriteItems(issues);

                this.WriteFooter();
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private void WriteItems(IEnumerable<ReleaseNotesIssues> issues)
        {
            this.WriteWhatSFixed(issues);
        }

        private void WriteHeader()
        {
            this.xmlWriter.WriteStartDocument();
            this.xmlWriter.WriteStartElement("releaseNote");
            this.xmlWriter.WriteStartElement("items");
        }

        private void WriteFooter()
        {
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteEndElement();

            this.xmlWriter.WriteEndDocument();
        }

        void WriteWhatSFixed(IEnumerable<ReleaseNotesIssues> issues)
        {
            foreach (var group in issues.OrderBy(i => i.AreaName).GroupBy(i => i.AreaName))
            {
                this.xmlWriter.WriteStartElement("releaseNoteItem");
                this.xmlWriter.WriteAttributeString("Title", group.Key);

                this.xmlWriter.WriteStartElement("fixedItems");
                foreach (var item in group)
                {
                    this.xmlWriter.WriteStartElement("add");
                    this.xmlWriter.WriteCData(item.Title);
                    this.xmlWriter.WriteEndElement();
                }

                this.xmlWriter.WriteEndElement();
                this.xmlWriter.WriteEndElement();
            }
        }
    }
}
