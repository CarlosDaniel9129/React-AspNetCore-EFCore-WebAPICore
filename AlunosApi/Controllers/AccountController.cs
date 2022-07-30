using AlunosApi.Services;
using AlunosApi.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticate _authenticate;

        public AccountController(IConfiguration configuration, IAuthenticate authenticate)
        {
            _configuration = configuration;
            _authenticate = authenticate;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "As senhas não conferem");
                return BadRequest(ModelState);
            }

            var result = await _authenticate.RegisterUser(model.Email, model.Password);

            if (result)
            {
                return Ok($"Usuário {model.Email} criado com sucesso");
            }
            else
            {
                ModelState.AddModelError("CreateUser", "Registro inválido");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel model)
        {
            var result = await _authenticate.Authenticate(model.Email, model.Password);

            if (result)
            {
                return GenerateToken(model);
            }
            else
            {
                ModelState.AddModelError("LoginUser", "Login inválido");
                return BadRequest(ModelState);
            }
        }

        private ActionResult<UserToken> GenerateToken(LoginModel model)
        {
            var claims = new Claim[]
            {
                new Claim("email", model.Email),
                new Claim("meuToken", "token carlos"),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // adiciona um identificador unico usando a struct GUID
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])); // esta Key deve ser um hash, gerado de alguma maneira

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // assinatura digital que será assinada no token

            var expiration = DateTime.UtcNow.AddMinutes(20);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token), // retorna no formato json em String
                Expiration = expiration,
            };
        }
    }
}
