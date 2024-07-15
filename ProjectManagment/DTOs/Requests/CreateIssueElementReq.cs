namespace ProjectManagment.DTOs.Requests
{
    public class CreateIssueElementReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Order { get; set; }
    }
}
