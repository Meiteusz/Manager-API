using Manager.Services.DTO_s.ViewModels;

namespace Manager.API.Utilities
{
    public static class Responses
    {
        public static ResponseViewModel ApplicationError()
            => new ResponseViewModel()
            {
                Success = false,
                Message = "Ocorreu um erro inesperado!",
                Data = null
            };

        public static ResponseViewModel DomainErrorMessage(string message)
            => new ResponseViewModel()
            {
                Success = false,
                Message = message,
                Data = null
            };

        public static ResponseViewModel DomainErrorMessage(string message, IReadOnlyCollection<string> errors)
            => new ResponseViewModel()
            {
                Success = false,
                Message = message,
                Data = errors
            };

        public static ResponseViewModel UnauthorizedErrorMessage()
            => new ResponseViewModel()
            {
                Success = false,
                Message = "Login ou senha estão incorretos!",
                Data = null
            };

        public static ResponseViewModel OkResponse(string message, dynamic? data = null)
            => new ResponseViewModel()
            {
                Success = true,
                Message = message,
                Data = data
            };
    }
}
