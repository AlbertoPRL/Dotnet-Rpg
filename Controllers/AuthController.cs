using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.DTOs.User;
using dotnet_rpg.Services.AuthenticationServices;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepo;
    private readonly IMapper _map;

    public AuthController(IMapper map, IAuthRepository authRepo)
    {
        _map = map;
        _authRepo = authRepo;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
    {
        var user = _map.Map<User>(request);
        var response = await _authRepo.Register(user, request.Password);

        if (!response.Succes)
            return BadRequest(response);

        return Ok(response);
    }
}
