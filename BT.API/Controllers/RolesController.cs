using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BT.Data;
using BT.Data.Entities;
using BT.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;
using BT.Dto;
using System.IO;
using BT.Core.Pages;

namespace BT.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CRMContext context;

        public RolesController(IWrapperRepository wrapperRepository, IMapper mapper, IDistributedCache distributedCache, CRMContext context)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.context = context;
        }


        /// <summary>
        /// 角色下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Roles>>> GetRolesAsync()
        {
            return await context.Roles.Where(m => m.IsDel == 0).ToListAsync();
        }


        /// <summary>
        /// 角色显示
        /// </summary>
        /// <returns></returns>
        [HttpGet("rolesshow")]
        public async Task<IActionResult> GetRolesAllAsync([FromQuery]PageParameters pageParameters)
        {
            //var id = Convert.ToInt32(User.Identity.Name);
            IQueryable<Roles> source = context.Roles.Where(m => m.IsDel == 0);


            //角色名称查询
            if (!string.IsNullOrEmpty(pageParameters.RoleName))
            {
                source = source.Where(m => m.Name.Contains(pageParameters.RoleName) && m.IsDel == 0);
            }

            if (pageParameters.CreateTimeByFind != null)
            {
                source = source.Where(m => m.CreateTime >= pageParameters.CreateTimeByFind);
            }

            var pagelist = await PageList<Roles>.CreatePageList(source, pageParameters.Page, pageParameters.Limit);

            var roleShowDtos = mapper.Map<List<RolesShowDto>>(pagelist.Source);
            //var list = await context.Roles.ToListAsync();
            //return Ok(new { code = 0, data = list });

            return Ok(new { code = 0, data = roleShowDtos, msg = "", count = pagelist.Count });
        }


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleCreateDto roleDto)
        {
            var id = Convert.ToInt32(User.Identity.Name);
            var roles = mapper.Map<Roles>(roleDto);
            roles.CreateID = id;
            roles.CreateTime = DateTime.Now;
            await context.Roles.AddAsync(roles);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("delroles")]
        public async Task<IActionResult> DelRoleAsync(string ids)
        {
            List<Roles> roles = new List<Roles>();

            foreach (var item in ids.Split(","))
            {
                var role = await context.Roles.FindAsync(Convert.ToInt32(item));
                role.IsDel = 1;
                roles.Add(role);
            }
            context.UpdateRange(roles);
            await context.SaveChangesAsync();
            return Ok(ids);
        }

        /// <summary>
        /// 根据Id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<ActionResult<Roles>> GetRoleByIdAsync(int roleId)
        {
            return await context.Roles.FindAsync(roleId);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editRoleDto"></param>
        /// <returns></returns>
        [HttpPost("editrole")]
        public async Task<IActionResult> EditRoleAsync(int id, [FromBody] RoleCreateDto editRoleDto)
        {
            var oldrole = await context.Roles.FindAsync(id);
            if(oldrole == null)
            {
                return NotFound();
            }

            oldrole.Name = editRoleDto.Name;
            context.Roles.Update(oldrole);

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
