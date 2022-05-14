using Manager.Core.Exceptions;
using Manager.Domain.Validators;
using System.ComponentModel.DataAnnotations;

namespace Manager.Domain.Entities
{
    public class User : Base
    {
        [Required]
        [StringLength(80)]
        public string Name { get; private set; }

        [Required]
        [StringLength(180)]
        public string Email { get; private set; }

        [Required]
        [StringLength(50)]
        public string Password { get; private set; }

        public User(string name, string email, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            _errors = new List<string>();

            Validate();
        }

        //EF
        protected User() { }

        public void ChangeName(string name)
        {
            Name = name;
            Validate();
        }

        public void ChangeEmail(string email)
        {
            Email = email;
            Validate();
        }

        public void ChangePassword(string password)
        {
            Password = password;
            Validate();
        }

        public override bool Validate()
        {
            var validator = new UserValidator();
            var validation = validator.Validate(this);

            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    _errors.Add(error.ErrorMessage);

                throw new DomainException("Alguns campos estão inválidos, por favor corrija-os! ", _errors);
            }

            return true;
        }
    }
}
