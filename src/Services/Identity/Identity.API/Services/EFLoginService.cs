﻿using Microsoft.AspNetCore.Identity;
using Identity.API.Models;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public class EFLoginService : ILoginService<ApplicationUser>
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;

        public EFLoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindByUsername(string user)
        {
            return await _userManager.FindByEmailAsync(user);
        }

        public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Task SignIn(ApplicationUser user) {
            return _signInManager.SignInAsync(user, true);
        }
    }
}
