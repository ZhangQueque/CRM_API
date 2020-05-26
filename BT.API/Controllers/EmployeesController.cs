using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using BT.API.Models;
using BT.Core;
using BT.Data.Entities;
using BT.Dto;
using BT.Service;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BT.API.Controllers
{
    /// <summary>
    /// 员工API
    /// </summary>
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CustomConfigOptions configOptions;
        public EmployeesController(IWrapperRepository wrapperRepository, IOptions<CustomConfigOptions> options
            , IMapper mapper,IDistributedCache distributedCache)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.configOptions = options.Value;
        }


        /// <summary>
        /// 员工登录
        /// </summary>
        /// <param name="loginDto">登录对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EmployeeLoginAsync([FromBody] LoginDto loginDto)
        {
            var loginEmp = await wrapperRepository.EmployeeRepository.LoginAsync(loginDto);

            if (loginEmp == null)
            {
                return NotFound();
            }

            //生成密钥
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes("asdadhajhdkjahsdkjahdkj9au8d9adasidoad89asu813e"));

            //生成签名
            var sig = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //声明你是谁
            List<Claim> claimList = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,loginEmp.ID.ToString()),  //type  value 
                new Claim(ClaimTypes.Email,loginEmp.Email),  //type  value 
                new Claim(JwtClaimTypes.PhoneNumber,loginEmp.Phone),  //type  value 
                new Claim(JwtClaimTypes.NickName,loginEmp.Name),  //type  value 
               
            };

            //获取员工对应的角色信息

            var roles = await wrapperRepository.Employee_RoleRepository.GetEmployees_RolesByEmployeeIDAsync(loginEmp.ID);

            //声明员工角色
            foreach (var item in roles)
            {
                claimList.Add(new Claim(ClaimTypes.Role, item.RoleName));
            }



            //创建一个jwt安全对象
            var jwtObj = new JwtSecurityToken(
                  issuer: "www.zhangquque.com",   //发行人
                  audience: "www.gaozijian.com",  //签收者
                  claims: claimList,
                  signingCredentials: sig,
                  expires: DateTime.Now.AddHours(2)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtObj);  //书写Token

            //发送邮箱登录提醒
            EmailSendHelper.SendLoginEmail(loginEmp.Email, configOptions.EmailPassword);

            //计入登录日志
            await wrapperRepository.LogRepository.AddAsync(
                new Logs
                {
                    Content = $" “{loginEmp.Name}”在{DateTime.Now}上线了！",
                    CreateTime = DateTime.Now,
                    CreateID = loginEmp.ID,
                    CategoryID = 1
                }
                );

            await wrapperRepository.LogRepository.SaveAsync();

            return Ok(new { token = token });
        }



        /// <summary>
        /// 员工登录获取相应信息，和权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<EmployeeInfoDto>> GetEmployeeInfoAsync()
        {
            EmployeeInfoDto info = new EmployeeInfoDto();
            var empId = Convert.ToInt32(User.Identity.Name);
            //拿到员工的所有角色
            IQueryable<Employees_Roles> roles = await wrapperRepository.Employee_RoleRepository.GetEmployees_RolesByEmployeeIDAsync(empId); //角色集合

            string json = await distributedCache.GetStringAsync($"Role_Menu_{roles.FirstOrDefault().ID}");

            //当缓冲为空的时候
            if (string.IsNullOrEmpty(json))
            {
                var employee = await wrapperRepository.EmployeeRepository.GetByIdAsync(empId); 

                if (employee == null)
                {
                    return NotFound();
                }

                info = mapper.Map<EmployeeInfoDto>(employee);

              

                List<Roles_Permissions> roles_PermissionList = new List<Roles_Permissions>(); //权限记录集合
                                                                                              //遍历查询到所有的权限记录
                foreach (Employees_Roles item in roles.ToList())
                {
                    IQueryable<Roles_Permissions> roles_Permissions = await wrapperRepository.Role_PermissionRepository.GetRoles_PermissionsByRoleID(item.RoleID);
                    roles_PermissionList.AddRange(roles_Permissions.ToList().ToArray());
                }

                List<Permissions> permissionList = new List<Permissions>();  //权限集合

                //去重
                var distinctPeople = roles_PermissionList
                                     .GroupBy(m => m.PermissionID)
                                     .Select(m => m.First())
                                     .ToList();

                //遍历权限记录（去重） 拿到相应的权限
                foreach (Roles_Permissions item in distinctPeople)
                {
                    Permissions permission = await wrapperRepository.PermissionRepository.GetByIdAsync(item.PermissionID);
                    if (permission.IsDel == 1)
                    {
                        continue;
                    }
                    permissionList.Add(permission);
                }


                info.Permissions = permissionList.OrderBy(m=>m.Sort).ToList();

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1)
                };


                //序列化为字符串存到缓冲
                string jsonCache = JsonSerializer.Serialize<EmployeeInfoDto>(info); 

                //存缓存
                await distributedCache.SetStringAsync($"Role_Menu_{roles.FirstOrDefault().ID}", jsonCache, options);

                return Ok(info);
            }


            //不为空怎么做？？？

            //把反序列化成对象
            info = JsonSerializer.Deserialize<EmployeeInfoDto>(json);

            return Ok(info);
        }

    }
}