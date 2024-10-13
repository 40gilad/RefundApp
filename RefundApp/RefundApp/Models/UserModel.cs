namespace RefundApp.Models
{
    public class UserModel
    {
        public string Uid { get; set; }
        public string UName { get; set; }
        public string UPassword { get; set; }

        public UserModel() { }

        public UserModel(string u_id, string u_name, string u_password)
        {
            if (string.IsNullOrWhiteSpace(u_id))
                throw new ArgumentException("User ID cannot be null or empty", nameof(u_id));
            if (string.IsNullOrWhiteSpace(u_password))
                throw new ArgumentException("Password cannot be null or empty", nameof(u_password));
            if (string.IsNullOrWhiteSpace(u_name))
                throw new ArgumentException("User Name cannot be null or empty", nameof(u_name));

            Uid = u_id;
            UName = u_name;
            UPassword = u_password;
        }

        public UserModel(UserModel other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Other UserModel cannot be null");

            Uid = other.Uid;
            UName = other.UName;
            UPassword = other.UPassword;
        }

        public override string ToString()
        {
            return $"{{\nUser Id: {Uid}\nUser Name: {UName}\n}}";
        }
    }
}
