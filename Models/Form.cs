using System;

namespace Sigma.Models
{
    public partial class Form
    {
        public Form(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
        public Form(int id, string email, string password, int? recoverCode) : this(id, email, password)
        {
            RecoverCode = recoverCode;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? RecoverCode { get; set; }
    }
}