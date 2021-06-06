using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UseCases.Role.DTO;

namespace Api.Controllers
{
    public class RoleController : BaseApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewRole(RoleDto dto)
        {
            var isRoleExists = await _roleManager.RoleExistsAsync(dto.Name);
            if (isRoleExists) return BadRequest("This role is already exists");
            
            var role = await _roleManager.FindByNameAsync(dto.Name);

            if (role == null)
            {
                var newRole = await _roleManager.CreateAsync(new IdentityRole {Name = dto.Name});
                return Ok(newRole);
            }

            return BadRequest("Role with that name already exists");
        }

        [AllowAnonymous]
        [HttpPost("addToUser")]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserDto dto)
        {
            var isRoleExists = await _roleManager.RoleExistsAsync(dto.RoleName);
            if (!isRoleExists) return BadRequest("This role is not exists");
            
            var user = await _userManager.FindByIdAsync(dto.UserId);
            var roleForUser = await _roleManager.FindByNameAsync(dto.RoleName);

            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains(roleForUser.Name))
            {
                var result = await _userManager.AddToRoleAsync(user, roleForUser.Name);
                return Ok(result);
            }
            return BadRequest("The user already has this role");
        }
    }
}