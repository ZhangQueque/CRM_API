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
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace BT.API.Controllers
{
    [Route("api/employees_roles")]
    [ApiController]
    public class Employees_RolesController : ControllerBase
    {
        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CRMContext context;

        public Employees_RolesController(IWrapperRepository wrapperRepository, IMapper mapper, IDistributedCache distributedCache, CRMContext context)
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
        [HttpGet("Role")]
        public async Task<ActionResult<List<Roles>>> GetRoles()
        {
            return await context.Roles.Where(m => m.IsDel == 0).ToListAsync();
        }


        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="pageParameters"></param>
        /// <param name="employee_RoleIDByFind"></param>
        /// <returns></returns>
        [HttpGet("employees_role")]
        public async Task<IActionResult> GetPageListByEmployee_Roles([FromQuery]PageParameters pageParameters,string employee_RoleIDByFind)
        {
            var id = Convert.ToInt32(User.Identity.Name);
            IQueryable<Employees_Roles> source;
            source = context.Employees_Roles.Where(m => m.IsDel == 0);
            //角色名称查询
            if (!string.IsNullOrEmpty(employee_RoleIDByFind))
            {
                var ent = Convert.ToInt32(employee_RoleIDByFind);
                source = source.Where(m => m.RoleID.Equals(ent) && m.IsDel == 0);
            }
            if (!string.IsNullOrEmpty(pageParameters.CustomerNameByFind))
            {
                source = source.Where(m => m.EmployeeName.Contains(pageParameters.CustomerNameByFind) && m.IsDel == 0);
            }

            var pagelist = await PageList<Employees_Roles>.CreatePageList(source, pageParameters.Page, pageParameters.Limit);
            var employee_roleShowDto = mapper.Map<List<Employees_RolesShowDto>>(pagelist.Source);
            //返回符合Layui的数据格式
            return Ok(new { code = 0, msg = "", data = employee_roleShowDto, count = pagelist.Count });
        }



        /// <summary>
        /// 添加角色分配
        /// </summary>
        /// <param name="employees_Roles">添加对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMap(Employees_Roles employees_Roles)
        {
            var map = await context.Employees_Roles.FirstOrDefaultAsync(m => m.EmployeeID == employees_Roles.EmployeeID && m.RoleID == employees_Roles.RoleID);
            if (map != null )
            {
                return Ok(new { code=1,msg="该用户角色已存在！请重新选择！"});

            }
            var emp = await context.Employees.FindAsync(employees_Roles.EmployeeID);

            var role = await context.Roles.FindAsync(employees_Roles.RoleID);

            employees_Roles.EmployeeName = emp.Name;
            employees_Roles.RoleName = role.Name;
            employees_Roles.CreateTime = DateTime.Now;

            await context.Employees_Roles.AddAsync(employees_Roles);

            await context.SaveChangesAsync();

            return Ok(new { code = 0, msg = "角色分配成功！" });
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        [HttpGet("delete")]
        public async Task<IActionResult> DeleteMap(string ids)
        {
            List<Employees_Roles> employees_Roles = new List<Employees_Roles>();
            foreach (var item in ids.Split(","))
            {
                var map = await context.Employees_Roles.FindAsync(Convert.ToInt32(item));
                employees_Roles.Add(map);
            }

            context.Employees_Roles.RemoveRange(employees_Roles);
            await context.SaveChangesAsync();

            return Ok(ids.Split(",").Length);
        }
    }
}