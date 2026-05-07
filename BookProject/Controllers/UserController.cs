using BookProject.Interfaces;
using BookProject.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    [Authorize]
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(users.Select(u => u.ToReadDTO()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToReadDTO());
        }

        [HttpPut("{id}/promote")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PromoteToAdmin([FromRoute] int id)
        {
            var user = await _userRepo.PromoteToAdminAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToReadDTO());
        }

        [HttpPut("{id}/demote")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DemoteToUser([FromRoute] int id)
        {
            var user = await _userRepo.DemoteToUserAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToReadDTO());
        }
    }
}