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
    [Route("api/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {

        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CRMContext context;

        public PermissionsController(IWrapperRepository wrapperRepository, IMapper mapper, IDistributedCache distributedCache, CRMContext context)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.context = context;
        }



        /// <summary>
        /// 通过角色ID获取对应的权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        [HttpGet("{roleID}")]
        public async Task<IActionResult> GetPermissionsDtoesByRoleIDAsync(int roleID)
        {
            List<PermissionsDto> permissionsDtos;//权限集合 

            //拿取全部的权限
            var permissions = await context.Permissions.Where(m=>m.IsDel==0).ToListAsync();

            //Auto映射
            permissionsDtos = mapper.Map<List<PermissionsDto>>(permissions);


            if (roleID ==0)
            {              
                return Ok(new { code = 0, msg = "", count = permissionsDtos.Count(), data = permissionsDtos });
            }

            //根据角色ID获取对应的权限记录
            var roles_Permissions = await wrapperRepository.Role_PermissionRepository.GetRoles_PermissionsByRoleID(roleID);

            List<PermissionsDto> rolePermissionsDtos = new List<PermissionsDto>();

            //遍历权限集合和  角色权限记录表里的权限进行比对，一样的花就设置选中状态为true
            foreach (var permissionsDto in permissionsDtos)
            {
                PermissionsDto dto = mapper.Map<PermissionsDto>(permissionsDto); //Auto映射

                foreach (var roles_Permission in roles_Permissions)
                {
                    if (permissionsDto.ID == roles_Permission.PermissionID)  //如果角色包含这个权限就让选中状态为true
                    {
                        dto.LAY_CHECKED = true;
                    }
                }
                rolePermissionsDtos.Add(dto);

            }

            return Ok(new { code = 0, msg = "", count = rolePermissionsDtos.Where(m => m.IsDel == 0).Count(), data = rolePermissionsDtos.Where(m => m.IsDel == 0) });
        }




        /// <summary>
        /// 通过角色ID更改对应的权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="permissionIDs">权限ID字符串</param>
        /// <returns></returns>
        [HttpGet("edit/{roleID}")]
        public async Task<IActionResult> EditPermissionsDtoesByRoleIDAsync(int roleID,string permissionIDs)
        {
            if (roleID==0)
            {
                return Ok(new { code=1,msg="请先选中角色，再为其更改权限！"});
            }

            var tokenid = Convert.ToInt32(User.Identity.Name);

            //根据角色ID获取对应的权限记录
            var roles_Permissions = await wrapperRepository.Role_PermissionRepository.GetRoles_PermissionsByRoleID(roleID);

            //删除角色全部的权限记录
            context.Roles_Permissions.RemoveRange(roles_Permissions);
            await context.SaveChangesAsync();


            //查询到角色信息
            Roles role = await context.Roles.FindAsync(roleID);

            List<Roles_Permissions> roles_PermissionsAddList = new List<Roles_Permissions>();

            //遍历重新添加权限记录
            foreach (var id in permissionIDs.Split(","))
            {
                var permission = await context.Permissions.FindAsync(Convert.ToInt32(id));

                Roles_Permissions roles_Permission = new Roles_Permissions();


                roles_Permission.CreateID = tokenid;
                roles_Permission.CreateTime = DateTime.Now;
                roles_Permission.RoleID = roleID;
                roles_Permission.RoleName = role.Name;
                roles_Permission.PermissionID = permission.ID;
                roles_Permission.PermissionName = roles_Permission.PermissionName;
                roles_PermissionsAddList.Add(roles_Permission);
             }


            await context.Roles_Permissions.AddRangeAsync(roles_PermissionsAddList);

            await context.SaveChangesAsync();


            //请掉所有角色权限菜单的缓冲
            foreach (var item in await context.Roles.ToListAsync())
            {
                await distributedCache.RemoveAsync($"Role_Menu_{item.ID}");
            }
            return Ok(new { code=0,msg="修改权限成功！"});
        }


        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="permissionCreateDto">权限对象</param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> CreatePermissionAsync([FromBody]PermissionCreateDto permissionCreateDto)
        {

            var id = Convert.ToInt32(User.Identity.Name);

            //获取排序id
            int sortId = await context.Permissions.Where(m => m.PID == permissionCreateDto.PID).MaxAsync(m => m.Sort);

            var leaveObj = await context.Permissions.FirstOrDefaultAsync(m => m.PID == permissionCreateDto.PID);

            var permission = mapper.Map<Permissions>(permissionCreateDto);

            permission.CreateID = id;
            permission.CreateTime = DateTime.Now;
            permission.ActiveName = permission.Name;
            permission.Sort = sortId + 1;
            permission.Leave = leaveObj.Leave;

            await context.Permissions.AddAsync(permission);

            await context.SaveChangesAsync();

            return Ok();
                 
        }
 
    }
}
