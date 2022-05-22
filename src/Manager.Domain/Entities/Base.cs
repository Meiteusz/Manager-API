using System.ComponentModel.DataAnnotations;

namespace Manager.Domain.Entities
{
    public abstract class Base
    {
        [Key]
        [DataType("BIGINT")]
        public long Id { get; set; }


        protected List<string> _errors;
        public IReadOnlyCollection<string> Errors => _errors;


        public abstract void Validate();
    }
}
