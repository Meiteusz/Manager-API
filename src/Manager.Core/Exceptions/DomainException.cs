namespace Manager.Core.Exceptions
{
    public class DomainException : Exception
    {
        public readonly List<string> Errors;


        public DomainException() { }

        public DomainException(string message, List<string> errors) : base(message)
        {
            Errors = errors;
        }

        public DomainException(string message) : base(message) { }

        public DomainException(string message, Exception innerException) : base(message, innerException) { }

        public static void ThrowDomainExceptionInvalidId(long inputId) 
            => throw new DomainException(string.Format("Não existe nenhum usuário com o Id informado!\nId Informado: {0}", inputId));
    }
}
