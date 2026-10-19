using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nokubico.API.Extensions;
using Nokubico.Application.DTOs;
using Nokubico.Application.Interfaces;

namespace Nokubico.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDTO>> GetMe(CancellationToken ct)
    {
        var user = await _userService.GetProfile(User.GetUserId(), ct);
        return Ok(user);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<UserDTO>> UpdateMe([FromBody] UserUpdateDTO dto, CancellationToken ct)
    {
        var user = await _userService.Update(User.GetUserId(), dto, ct);
        return Ok(user);
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe(CancellationToken ct)
    {
        await _userService.Delete(User.GetUserId(), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetById(Guid id, CancellationToken ct)
    {
        var user = await _userService.GetById(id, ct);
        return Ok(user);
    }
}