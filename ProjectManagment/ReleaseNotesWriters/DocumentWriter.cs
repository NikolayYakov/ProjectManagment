using ProjectManagment.Migrations;
using ProjectManagment.Models;

namespace ProjectManagment.ReleaseNotesWriters
{
    public abstract class DocumentWriter
    {
        public abstract MemoryStream Write(IEnumerable<ReleaseNotesIssues> issues);
    }
}
