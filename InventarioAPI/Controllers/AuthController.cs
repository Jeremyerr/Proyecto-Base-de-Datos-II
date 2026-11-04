using InventarioAPI.Dto;
using InventarioAPI.Models;
using InventarioAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace InventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<ApplicationUser> _usuarios;
        private readonly TokenServices _tokenServices;

        public AuthController(IMongoDatabase database, TokenServices tokenServices)
        {
            _usuarios = database.GetCollection<ApplicationUser>("usuarios");
            _tokenServices = tokenServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _usuarios.Find(u => u.Email == loginDto.Email && u.Password == loginDto.Password).FirstOrDefaultAsync();
            if (user == null) return Unauthorized("Usuario o contraseña incorrectos");

            var token = _tokenServices.GenerarToken(user);
            return Ok(new { token });
        }
    }
}