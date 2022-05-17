using AutoMapper;
using Manager.API.Utilities;
using Manager.Core.Exceptions;
using Manager.Services.DTO_s;
using Manager.Services.DTO_s.ViewModels;
using Manager.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper) 
        {
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpPost]
        [Route("/api/v1/users/create")]
        [Authorize] //This in real aplication > Register User: [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel userViewModel)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(userViewModel);
                var userCreated = await _userService.Create(userDto);

                return Ok(Responses.OkResponse("Usuário registrado com sucesso!", userCreated));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/getById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var userDto = await _userService.GetById(id);

                if (userDto != null)
                    return Ok(Responses.OkResponse("Usuário obtido com sucesso!", userDto));

                return Ok(Responses.OkResponse("Nenhum usuário foi encontrado com este Id!"));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/getAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var usersDtoList = await _userService.GetAll();

                return Ok(Responses.OkResponse("Usuários obtidos com sucesso!", usersDtoList));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/get-by-email")]
        [Authorize]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var userDto = await _userService.GetByEmail(email);

                if (userDto != null)
                    return Ok(Responses.OkResponse("Usuário obtido com sucesso!", userDto));

                return Ok(Responses.OkResponse("Nenhum usuário foi encontrado com o email informado!"));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/search-by-email")]
        [Authorize]
        public async Task<IActionResult> SearchByEmail(string email)
        {
            try
            {
                var usersDtoList = await _userService.SearchByEmail(email);

                if (usersDtoList.Count < 1)
                    return Ok(Responses.OkResponse("Não há nenhum usuário com este email"));

                return Ok(Responses.OkResponse("Usuários obtidos com sucesso!", usersDtoList));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/search-by-name")]
        [Authorize]
        public async Task<IActionResult> SearchByName(string name)
        {
            try
            {
                var usersDtoList = await _userService.SearchByName(name);

                if (usersDtoList.Count < 1)
                    return Ok(Responses.OkResponse("Não há nenhum usuário com este nome"));

                return Ok(Responses.OkResponse("Usuários obtidos com sucesso!", usersDtoList));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }

        [HttpPut]
        [Route("/api/v1/users/update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateUserViewModel userViewModel)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(userViewModel);

                var userUpdated = await _userService.Update(userDto);

                return Ok(Responses.OkResponse("Usuário atualizado com sucesso!", userUpdated));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }

        [HttpDelete]
        [Route("/api/v1/users/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _userService.Remove(id);

                return Ok(Responses.OkResponse("Usuário deletado com sucesso!"));
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationError());
            }
        }
    }
}
