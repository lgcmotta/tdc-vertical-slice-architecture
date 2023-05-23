﻿using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BankingApp.Application.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetRequestPath() => _httpContextAccessor.HttpContext.Request.Path;
        
    public string GetUserDisplayName() => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
}