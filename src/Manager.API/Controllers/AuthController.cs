using Manager.API.Utilities;
using Manager.Services.DTO_s.ViewModels;
using Manager.Services.Token;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IConfiguration configuration, ITokenGenerator tokenGenerator)
        {
            this._configuration = configuration;
            this._tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        public IActionResult Login([FromBody]LoginViewModel loginViewModel)
        {
            try
            {
                var tokenLogin = _configuration["Jwt:Login"];          //Fixes Login and Password, but in real application > pushed on the database to make login
                var tokenPassword = _configuration["Jwt:Password"];

                if (loginViewModel.Login == tokenLogin && loginViewModel.Password == tokenPassword)
                {
                    return Ok(Responses.OkResponse("Usuário autenticado com sucesso!", new
                    {
                        Token = _tokenGenerator.GenerateToken(),
                        TokenExpires = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:HoursToExpire"]))
                    }));
                }
                else
                {
                    return StatusCode(401, Responses.UnauthorizedErrorMessage());
                }
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }
    }
}
