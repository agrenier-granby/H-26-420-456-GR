using BackgroundTasks.Web.Exemple.Extensions;
using BackgroundTasks.Web.Exemple.Models;
using BackgroundTasks.Web.Exemple.Requests;
using BackgroundTasks.Web.Exemple.Services;
using BackgroundTasks.Web.Exemple.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTasks.Web.Exemple.Controllers
{
    public class UserController(UserService<User> userService) : Controller
    {
        private readonly UserService<User> _userService = userService;

        [HttpPost("users")]
        public async Task<IActionResult> Create([FromBody] UserVM request)
        {
            var user = request.ToUser();
            var result = await _userService.CreateAsync(user);

            if (!result.Success || result.Entity is null)
            {
                return BadRequest(result.Message);
            }

            var response = result.Entity.ToUserResponse();

            return CreatedAtAction(nameof(Get), new { response.Id }, response);
        }

        [HttpGet("users/{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var user = await _userService.GetAsync(id);

            if (user is null)
            {
                return NotFound(id);
            }

            var response = user.ToUserResponse();
            return Ok(response);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            var usersResponse = users.ToUserResponse();
            return Ok(usersResponse);
        }

        // L'utilisation de paramètres [FromRoute] et [FromQuery] aurait pu être utilisée à la place de [FromMultiSource]
        [HttpPut("users/{id:guid}")]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ASP0018:Unused route parameter", Justification = "Used in the data annotation FromMultiSource")]
        public async Task<IActionResult> Update(
            [FromMultiSource] UserUpdateVM vm)
        {
            if (vm.User == null || vm.User.Id != vm.Id)
            {
                return BadRequest();
            }

            var existingUser = await _userService.GetAsync(vm.Id);

            if (existingUser is null)
            {
                return NotFound();
            }

            var user = vm.User.ToUser();
            await _userService.UpdateAsync(user);

            var response = user.ToUserResponse();
            return Ok(response);
        }

        [HttpDelete("users/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
