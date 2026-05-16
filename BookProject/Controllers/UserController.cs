using BookProject.Interfaces;
using BookProject.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BookProject.DTOs.UserDTOs;

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

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("userId")!);

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(users.Select(u => u.ToReadDTO()));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToReadDTO());
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userRepo.GetUserByIdAsync(GetCurrentUserId());
            if (user == null) return NotFound();
            return Ok(user.ToPrivateReadDTO());
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UserUpdateDTO dto)
        {
            var (user, error) = await _userRepo.UpdateProfileAsync(GetCurrentUserId(), dto);

            if (error != null) return BadRequest(error);
            if (user == null) return NotFound();

            return Ok(user.ToPrivateReadDTO());
        }

        [HttpPut("{id}/promote")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PromoteToAdmin([FromRoute] int id)
        {
            var user = await _userRepo.PromoteToAdminAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToPrivateReadDTO());
        }

        [HttpPut("{id}/demote")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DemoteToUser([FromRoute] int id)
        {
            var user = await _userRepo.DemoteToUserAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToPrivateReadDTO());
        }
    }
}