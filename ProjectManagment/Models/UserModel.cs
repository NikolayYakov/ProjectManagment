namespace ProjectManagment.Models
{
    public class UserModel
    {
        private string name;


        public UserModel(string userId, string email, bool isOwner = false)
        {
            this.UserId = userId;
            this.Email = email;
            this.Name = email;
            this.IsOwner = isOwner;

            this.Role = IsOwner ? "Owner" : "Member";
        }

        public bool IsOwner { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get { return name; } set { name = value.Split("@").First(); } }
    }
}
