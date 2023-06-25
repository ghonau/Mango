﻿using Mango.Service.AuthAPI.Models;
using Mango.Service.AuthAPI.Models.Dto;
using Mango.Service.AuthAPI.Services.IServices;
using Mango.Services.AuthAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace Mango.Service.AuthAPI.Services
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user == null) {
                return false; 
            }
            else
            {
                if(! _roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exists 
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                   
                }
                await _userManager.AddToRoleAsync(user, roleName);  
                return true;
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower()  == loginRequestDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponseDto { UserDto = null, Token = string.Empty }; 
            }
            
            
            // user was found just need to generate jwt token

            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDto userDto = new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            }; 

            LoginResponseDto loginResponseDto = new LoginResponseDto() { UserDto = userDto, Token = token };
            return loginResponseDto; 
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpperInvariant(),
                Name = registrationRequestDto.Name,
                NormalizedUserName = registrationRequestDto.Name.ToUpperInvariant(),
                PhoneNumber = registrationRequestDto.PhoneNumber
            }; 
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, registrationRequestDto.Password);
                if(result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
                    UserDto userDto = new UserDto()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber

                    };
                    return string.Empty; 
                }
                else
                {
                    return result.Errors.First().Description; 
                }
            }
            catch (Exception ex) { 

            }
            return "Error encountered"; 
        }
    }
}
