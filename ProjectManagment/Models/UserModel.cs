namespace ProjectManagment.Models
{
    public class UserModel
    {
        private string name;

        public UserModel(string userId, string email)
        {
            this.UserId = userId;
            this.Email = email;
            this.Name = email;
        }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get { return name; } set { name = value.Split("@").First(); } }
    }
}
