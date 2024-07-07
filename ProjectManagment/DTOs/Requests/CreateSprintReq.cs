namespace ProjectManagment.DTOs.Requests
{
    public class CreateSprintReq : CreateIssueElementReq
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
