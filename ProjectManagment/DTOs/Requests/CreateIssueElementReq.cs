namespace ProjectManagment.DTOs.Requests
{
    public class CreateIssueElementReq
    {
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
    }
}
