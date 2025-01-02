using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RefundApp.Models
{

    public class UserModel
    {
        public int Id { get; set; }
        public required string UName { get; set; }

        public required string UEmail { get; set; }

        [DataType(DataType.Password)]
        public required string UPassword { get; set; }

        public UserModel() { }

        public UserModel(string u_name, string e_mail, string u_password)
        {
            if (string.IsNullOrWhiteSpace(u_password))
                throw new ArgumentException("Password cannot be null or empty", nameof(u_password));
            if (string.IsNullOrWhiteSpace(e_mail))
                throw new ArgumentException("Email cannot be null or empty", nameof(u_password));
            if (string.IsNullOrWhiteSpace(u_name))
                throw new ArgumentException("User Name cannot be null or empty", nameof(u_name));

            UName = u_name;
            UEmail = e_mail;
            UPassword = u_password;
        }

        public UserModel(UserModel other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Other UserModel cannot be null");

            UName = other.UName;
            UEmail = other.UEmail;
            UPassword = other.UPassword;
        }

        public override string ToString()
        {
            return $"{{\nUser Mail: {UEmail}\n User Name: {UName}\n}}";
        }
    }
}
