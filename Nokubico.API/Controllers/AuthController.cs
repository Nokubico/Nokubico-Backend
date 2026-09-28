using Microsoft.AspNetCore.Mvc;
using Nokubico.API.Models;
using Nokubico.Domain.Account;

namespace Nokubico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticate _authenticate;

        public AuthController(IAuthenticate authenticate)
        {
            _authenticate = authenticate;
        }

        [HttpPost("register")]
        public ActionResult<UserToken> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_authenticate.UserExists(model.Email))
            {
                return BadRequest("Este email já possui um cadastro.");
            }

            var user = _authenticate.Register(model.Name, model.Email, model.Password);
            if (user == null)
            {
                return BadRequest("Erro ao criar a conta.");
            }

            var token = _authenticate.GenerateToken(user.Id, user.Email, user.Role);
            return Ok(new UserToken { Token = token, Role = user.Role, Email = user.Email });
        }

        [HttpPost("login")]
        public ActionResult<UserToken> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_authenticate.Authenticate(model.Email, model.Password))
            {
                return Unauthorized("Credenciais inválidas.");
            }

            var user = _authenticate.GetUserByEmail(model.Email);
            if (user == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            var token = _authenticate.GenerateToken(user.Id, user.Email, user.Role);
            return Ok(new UserToken { Token = token, Role = user.Role, Email = user.Email });
        }
    }
}