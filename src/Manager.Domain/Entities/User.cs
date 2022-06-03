using Manager.Core.Exceptions;
using Manager.Domain.Interfaces;
using Manager.Domain.Validators;
using System.ComponentModel.DataAnnotations;

namespace Manager.Domain.Entities
{
    public class User : Base, IAggregateRoot
    {
        [Required]
        [StringLength(80)]
        public string Name { get; private set; }

        [Required]
        [StringLength(180)]
        public string Email { get; private set; }

        [Required]
        [StringLength(100)]
        public string Password { get; private set; }

        public User(string name, string email, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            _errors = new List<string>();
        }

        //EF
        protected User() { }

        public void UpdatePassword(string newPassword)
            => Password = newPassword;

        public void Validate()
            => base.Validate<UserValidator, User>(new UserValidator(), this);
    }
}
