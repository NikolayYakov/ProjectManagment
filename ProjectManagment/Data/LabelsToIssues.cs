using Azure;
using Microsoft.Extensions.Hosting;

namespace ProjectManagment.Data
{
    public class LabelsToIssues
    {
        public Guid Id { get; set; }
        public Guid LabelId { get; set; }
        public Guid IssueId { get; set; }
        public Label Label { get; set; }
        public Issue Issue { get; set; }
        public bool IsRemoved { get; set; } = false;
    }
}
