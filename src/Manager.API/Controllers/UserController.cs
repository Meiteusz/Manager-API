using AutoMapper;
using Manager.API.ViewModels;
using Manager.Core.Exceptions;
using Manager.Services.DTO_s;
using Manager.Services.Services.Interfaces;
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
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel userViewModel)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(userViewModel);

                var userCreated = await _userService.Create(userDto);

                return Ok(new ResponseViewModel()
                {
                    Success = true,
                    Message = "Usuário criado com sucesso!",
                    Data = userCreated
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(new ResponseViewModel
                {
                    Message = "Algum erro aconteceu"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }
        }

        [HttpGet]
        [Route("/api/v1/users/getById")]
        public async Task<IActionResult> GetById()
        {
            try
            {

            }
            catch (DomainException ex)
            {

            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }
        }

        [HttpGet]
        [Route("/api/v1/users/getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

            }
            catch (DomainException ex)
            {

            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }
        }

        [HttpPut]
        [Route("/api/v1/users/update")]
        public async Task<IActionResult> Update()
        {
            try
            {

            }
            catch (DomainException ex)
            {

            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }
        }

        [HttpDelete]
        [Route("/api/v1/users/delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {

            }
            catch (DomainException ex)
            {

            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }
        }
    }
}
