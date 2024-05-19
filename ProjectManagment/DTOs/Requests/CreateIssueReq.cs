

namespace ProjectManagment.DTOs.Requests
{
    public class CreateIssueReq
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid ProjectId { get; set; }
        public string OwnerId { get; set; }
        public Guid MilestoneId { get; set; }
        public Guid StatusId { get; set; }
        public Guid AreaId { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
