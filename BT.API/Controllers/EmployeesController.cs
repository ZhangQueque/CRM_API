using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using BT.API.Models;
using BT.Core;
using BT.Core.Pages;
using BT.Data;
using BT.Data.Entities;
using BT.Dto;
using BT.Service;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly CRMContext context;
        private readonly CustomConfigOptions configOptions;
        public EmployeesController(IWrapperRepository wrapperRepository, IOptions<CustomConfigOptions> options
            , IMapper mapper,IDistributedCache distributedCache, CRMContext context)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.context = context;
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

        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("employee")]
        public async Task<ActionResult<List<Employees>>> GetEmployeesAsync()
        {
            return await context.Employees.Where(m => m.IsDel == 0).ToListAsync();
        }



        /// <summary>
        /// 员工分页查询
        /// </summary>
        /// <param name="pageParameters">分页参数</param>
        /// <returns></returns>
        [HttpGet("employeePage")]
        public async Task<IActionResult> GetPageListByEmployee([FromQuery]PageParameters pageParameters)
        {
            var id = Convert.ToInt32(User.Identity.Name);
            IQueryable<Employees> source;
            source = context.Employees.Where(m=>m.IsDel==0);
            #region 查询逻辑

            //if (pageParameters.EmployeeeSegmentationIDByFind != 0)  //如果查询的员工ID不为0
            //{
            //    source = context.Employees.Where(m => m.IsDel == 0 && m.EmployeesID == pageParameters.EmployeeIDByFind);

            //}
            //else   //如果为0就查询自己的
            //{
            //    source = context.Customers.Where(m => m.IsReal == pageParameters.Status && m.IsDel == 0 && m.EmployeesID == id);
            //}

            //分类类别的查询
            //if (pageParameters.CustomerSegmentationIDByFind != 0)
            //{
            //    source = source.Where(m => m.CustomerSegmentationID == pageParameters.CustomerSegmentationIDByFind);
            //}

            //客户名称查询
            if (!string.IsNullOrEmpty(pageParameters.EmployeeNameByFind))
            {
                source = source.Where(m => m.Name.Contains(pageParameters.EmployeeNameByFind));
            }

            //时间判断
            //开始时间
            if (pageParameters.StartTimeByFind != null)
            {
                source = source.Where(m => m.CreateTime >= pageParameters.StartTimeByFind);
            }


            //结束时间
            if (pageParameters.EndTimeByFind != null)
            {
                source = source.Where(m => m.CreateTime <= pageParameters.EndTimeByFind);
            }

            //中间时间
            if (pageParameters.StartTimeByFind != null && pageParameters.EndTimeByFind != null)
            {
                source = source.Where(m => m.CreateTime >= pageParameters.StartTimeByFind && m.CreateTime <= pageParameters.EndTimeByFind);
            }

            #endregion

            var pagelist = await PageList<Employees>.CreatePageList(source, pageParameters.Page, pageParameters.Limit);
            var employeeShowDto = mapper.Map<List<EmployeeShowDto>>(pagelist.Source);
            //返回符合Layui的数据格式
            return Ok(new { code = 0, msg = "", data = employeeShowDto, count = pagelist.Count });
        }

        /// <summary>
        /// 员工删除
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEmployeeAsync(string ids)
        {
            List<Employees> employees = new List<Employees>();

            foreach (var item in ids.Split(","))
            {
                var employee = await context.Employees.FindAsync(Convert.ToInt32(item));
                employee.IsDel = 1;   //软删除
                employees.Add(employee);
            }
            context.Employees.UpdateRange(employees);  //删除多个

            await context.SaveChangesAsync();

            return Ok(ids.Split(",").Length);
        }



        /// <summary>
        /// 添加一个员工
        /// </summary>
        /// <param name="createDto">表单数据</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeCreateDto createDto)
        {
            var id = Convert.ToInt32(User.Identity.Name);
            //生成密钥
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes("asdadhajhdkjahsdkjahdkj9au8d9adasidoad89asu813e"));
            //Auto映射 
            var employee = mapper.Map<Employees>(createDto);

            employee.CreateID = id;
            employee.Code = Guid.NewGuid().ToString();
            employee.CreateTime = DateTime.Now;
            employee.HeadImage = "https://www.zhangqueque.top:5001/UserImg/1_1_1.png";
            employee.IsDel = 0;
            employee.EmployeePID = 1;
            //employee.Password = "123456";
            employee.Password = MD5Encrypt.Encrypt(employee.Password);

           

            if (createDto.UploadFile != null)
            {
                if (createDto.UploadFile.Length > 25165824)
                {
                    return Ok(new { code = 1, msg = "文件不能大于3M！" });
                }
                //文件名复杂，避免重复覆盖
                string fileName = employee.Name + employee.Email + employee.Phone + createDto.UploadFile.FileName;

                //设置文件存储的路劲
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerImg", fileName);

                //服务端保存文件
                using (var stream = System.IO.File.Create(filePath))
                {
                    await createDto.UploadFile.CopyToAsync(stream);
                }
                employee.HeadImage = Request.Scheme + "://" + Request.Host + "/CustomerImg/" + fileName;
            }


            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();


            Employees_Roles employees_Roles = new Employees_Roles();
            employees_Roles.EmployeeID = employee.ID;
            employees_Roles.EmployeeName = employee.Name;
            employees_Roles.RoleID = 3;
            employees_Roles.RoleName = "员工";

            await context.Employees_Roles.AddAsync(employees_Roles);
            await context.SaveChangesAsync();

            return Ok(new { code = 0, msg = "员工添加成功！" });
        }

        /// <summary>
        /// 根据id获取员工
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employees>> GetEmployeeByIdAsync(int id)
        {
            var employee = await context.Employees.FindAsync(id);   //原先的对象
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }


        /// <summary>
        /// 员工修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="editDto">修改对象</param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<IActionResult> EditCustomersAsync(string id, [FromForm] EmployeeCreateDto editDto)
        {

            var oldEmployee = await context.Employees.FindAsync(Convert.ToInt32(id));   //原先的对象
            if (oldEmployee == null)
            {
                return NotFound();
            }



            //Auto映射   由于有的数据没有值默认是0 .在映射时都是0 ，所以的自己初始化一下
            if (editDto.UploadFile == null)
            {
                oldEmployee.HeadImage = oldEmployee.HeadImage;
            }
            oldEmployee.Name = editDto.Name;
            oldEmployee.Phone = editDto.Phone;
            oldEmployee.Email = editDto.Email;


            //文件上传
            {
                if (editDto.UploadFile != null)
                {
                    if (editDto.UploadFile.Length > 25165824)
                    {
                        return Ok(new { code = 1, msg = "文件不能大于3M！" });
                    }
                    //文件名复杂，避免重复覆盖
                    string fileName = oldEmployee.Name + oldEmployee.Email + oldEmployee.Phone + editDto.UploadFile.FileName;

                    //设置文件存储的路劲
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerImg", fileName);

                    //服务端保存文件
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await editDto.UploadFile.CopyToAsync(stream);
                    }
                    oldEmployee.HeadImage = Request.Scheme + "://" + Request.Host + "/CustomerImg/" + fileName;
                }




            }


            try
            {
                context.Employees.Update(oldEmployee);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok();
        }

    }
}