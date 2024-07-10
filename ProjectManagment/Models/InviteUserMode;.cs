namespace ProjectManagment.Models
{
    public class InviteUserModel : CreateProjectElementModel
    {
        public Guid InviteId { get; set; }
        public bool isInvited { get; set; } = false;
        public string? ErrorMessage {  get; set; }
    }
}
