﻿using Microsoft.Extensions.Configuration;
using DutchTreat.Data;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DutchTreat.controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> signInManager;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager, UserManager<StoreUser> userManager,IConfiguration configuration)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        public IActionResult Login()
        {

            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {

                    if (Request.Query.ContainsKey("ReturnUrl"))
                    {

                        return Redirect(Request.Query["ReturnUrl"].First());

                    }
                    else
                    {
                        return RedirectToAction("Shop", "App");
                    }


                }

            }

            ModelState.AddModelError("", "Failed to Login");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                
                if (user != null) {
                    //CheckPasswordSignInAsync checks whether the password is Okay
                    
                    var result = await signInManager.CheckPasswordSignInAsync(user,model.Password,false);
                        
                        if (result.Succeeded) {
                        //create the token

                        //By getting the claims, they are set of properties with 
                        // Well known values in them, they are stored in thetoken 
                        //and can be ui

                        var claims = new[] {

                        new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
                        
                        };

                        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
                        var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            configuration["Tokens:Issuer"],
                            configuration["Tokens:Audience"],
                            claims,
                            expires:DateTime.UtcNow.AddMinutes(30),
                            signingCredentials:creds

                            ) ;

                        var results = new {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", results);

                    }

                 }
            }
            return BadRequest();
        }
    }
}
