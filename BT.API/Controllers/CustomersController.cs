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
    [Route("api/customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CRMContext context;

        public CustomersController(IWrapperRepository wrapperRepository, IMapper mapper, IDistributedCache distributedCache, CRMContext context)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.context = context;
        }


        /// <summary>
        /// 下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("cate")]
        public async Task<ActionResult<List<CustomerSegmentations>>> GetCustomerSegmentationsAsync()
        {
            
            var list = await context.CustomerSegmentations.ToListAsync();

            return list;
        }

        /// <summary>
        /// 高级下属的员工成员
        /// </summary>
        /// <returns></returns>
        [HttpGet("childEmp")]     
        public async Task<IActionResult> GetCustomersAsync()
        {
            var id = Convert.ToInt32(User.Identity.Name);
            //查到登录的用户
            var employee = await context.Employees.FindAsync(id);

            var roles = await wrapperRepository.Employee_RoleRepository.GetEmployees_RolesByEmployeeIDAsync(employee.ID);

            string adminName = "";
            string managerName = "";
            string staffName = "";
            foreach (var item in roles)
            {
                if (item.RoleID == 1) //当角色id是管理员时
                {
                    adminName=item.RoleName;
                }
                if (item.RoleID == 2) //当角色id是经理时
                {
                    managerName = item.RoleName;
                }
                if (item.RoleID==3) //当角色id是员工时
                {
                    staffName = item.RoleName;
                }
            }

            if (string.IsNullOrEmpty(adminName) && string.IsNullOrEmpty(managerName) )  //如果管理员名称和经理名称为空，代表没有这俩个角色
            {
                return Ok(new { code = 1, msg = "该员工没有权限!" });
            }

            //否则查询到所属的员工
            var childEmployees = await context.Employees.Where(m => m.EmployeePID == employee.ID &&m.IsDel==0).ToListAsync();

            return Ok(new { code = 0, data= childEmployees});
        }


        /// <summary>
        /// 添加一个客户
        /// </summary>
        /// <param name="createDto">表单数据</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromForm] CustomerCreateDto createDto)
        {
            var id = Convert.ToInt32(User.Identity.Name);

            //Auto映射 
            var customer = mapper.Map<Customers>(createDto);

            //查询细分类名称
            var customerSegmentation = await context.CustomerSegmentations.FindAsync(customer.CustomerSegmentationID);

            customer.CreateID = id;
            customer.EmployeesID = id;
            customer.CreateTime = DateTime.Now;
            customer.CustomerSegmentationName = customerSegmentation.Name;
            customer.Image = "https://www.zhangqueque.top:5001/UserImg/1_1_1.png";
            if (createDto.UploadFile != null)
            {
                if (createDto.UploadFile.Length > 25165824)
                {
                    return Ok(new { code = 1, msg = "文件不能大于3M！" });
                }
                //文件名复杂，避免重复覆盖
                string fileName = customer.Name + customer.Email + customer.Phone + createDto.UploadFile.FileName;

                //设置文件存储的路劲
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerImg", fileName);

                //服务端保存文件
                using (var stream = System.IO.File.Create(filePath))
                {
                    await createDto.UploadFile.CopyToAsync(stream);
                }
                customer.Image = Request.Scheme + "://" + Request.Host + "/CustomerImg/" + fileName;
            }


            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            return Ok(new { code = 0, msg = "客户添加成功！" });
        }


        /// <summary>
        /// 潜在客户分页查询
        /// </summary>
        /// <param name="pageParameters">分页参数</param>
        /// <returns></returns>
        [HttpGet("noreal")]
        public async Task<IActionResult> GetPageListByCustomerByNoReal([FromQuery]PageParameters pageParameters)
        {
            var id = Convert.ToInt32(User.Identity.Name);
            IQueryable<Customers> source;

            #region 查询逻辑

            if (pageParameters.EmployeeIDByFind != 0)  //如果查询的员工ID不为0
            {
                source = context.Customers.Where(m => m.IsReal == pageParameters.Status && m.IsDel == 0 && m.EmployeesID == pageParameters.EmployeeIDByFind);

            }
            else   //如果为0就查询自己的
            {
                source = context.Customers.Where(m => m.IsReal == pageParameters.Status && m.IsDel == 0 && m.EmployeesID == id);
            }

            //分类类别的查询
            if (pageParameters.CustomerSegmentationIDByFind != 0)
            {
                source = source.Where(m => m.CustomerSegmentationID == pageParameters.CustomerSegmentationIDByFind);
            }

            //客户名称查询
            if (!string.IsNullOrEmpty(pageParameters.CustomerNameByFind))
            {
                source = source.Where(m => m.Name.Contains(pageParameters.CustomerNameByFind));
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

            var pagelist =await PageList<Customers>.CreatePageList(source,pageParameters.Page,pageParameters.Limit);

            //Auto映射
            var customerShowDtos = mapper.Map<List<CustomerShowDto>>(pagelist.Source);

            //返回符合Layui的数据格式
            return Ok(new { code=0,msg="",data= customerShowDtos, count=pagelist.Count});
        }



        /// <summary>
        /// 客户删除
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCustomersAsync(string ids)
        {
            List<Customers> customers = new List<Customers>();

            foreach (var item in ids.Split(","))
            {
                var customer =await context.Customers.FindAsync(Convert.ToInt32(item));
                customer.IsDel = 1;   //软删除
                customers.Add(customer);
            }
            context.Customers.UpdateRange(customers);  //删除多个

            await  context.SaveChangesAsync();

            return Ok(ids.Split(",").Length);
        }


        /// <summary>
        /// 根据id获取客户
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]  
        public async Task<ActionResult<Customers>> GetCustomerByIdAsync(int id)
        {
            var customer = await context.Customers.FindAsync(id);   //原先的对象
            if (customer == null )
            {
                return NotFound();
            }
            return customer;
        }



        /// <summary>
        /// 客户修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="editDto">修改对象</param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<IActionResult> EditCustomersAsync(string id, [FromForm] CustomerCreateDto editDto)
        {

            var oldCustomer = await context.Customers.FindAsync(Convert.ToInt32(id));   //原先的对象
            if (oldCustomer == null)
            {
                return NotFound();
            }

          

            //Auto映射   由于有的数据没有值默认是0 .在映射时都是0 ，所以的自己初始化一下
            var newCustomer = mapper.Map<Customers>(editDto);  //修改完的对象
            if (editDto.UploadFile == null)
            {
                newCustomer.Image = oldCustomer.Image;
            }
            newCustomer.ID = oldCustomer.ID;
            newCustomer.IsDel = oldCustomer.IsDel;
            newCustomer.CreateID = oldCustomer.CreateID;
            newCustomer.CreateTime = oldCustomer.CreateTime;
            newCustomer.EmployeesID = oldCustomer.EmployeesID;
            

            //文件上传
            {
                //查询细分类名称
                var customerSegmentation = await context.CustomerSegmentations.FindAsync(newCustomer.CustomerSegmentationID);
                newCustomer.CustomerSegmentationName = customerSegmentation.Name;
                if (editDto.UploadFile != null)
                {
                    if (editDto.UploadFile.Length > 25165824)
                    {
                        return Ok(new { code = 1, msg = "文件不能大于3M！" });
                    }
                    //文件名复杂，避免重复覆盖
                    string fileName = newCustomer.Name + newCustomer.Email + newCustomer.Phone + editDto.UploadFile.FileName;

                    //设置文件存储的路劲
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerImg", fileName);

                    //服务端保存文件
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await editDto.UploadFile.CopyToAsync(stream);
                    }
                    newCustomer.Image = Request.Scheme + "://" + Request.Host + "/CustomerImg/" + fileName;
                }




            }


            try
            {
                mapper.Map(newCustomer, oldCustomer, typeof(Customers), typeof(Customers));

                context.Customers.Update(oldCustomer);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Ok( );
        }


        /// <summary>
        /// 根据员工返回对应的客户列表
        /// </summary>
        /// <param name="name">查询客户的名称</param>
        /// <returns></returns>
        [HttpGet("norealCustomers")]
        public async Task<ActionResult<List<Customers>>> GetNoRealCustomersByEmployeeIDAsync(string name )
        {
            var id = Convert.ToInt32(User.Identity.Name);

            IQueryable<Customers> list =  context.Customers;
            if (!string.IsNullOrEmpty(name))
            {
                list =  list.Where(m=>m.Name.Contains(name) && m.IsDel== 0 && m.EmployeesID == id).OrderByDescending(m => m.CreateTime).Take(20);

                return await list.ToListAsync();
            }

            list =   list.Where(m => m.IsDel == 0 && m.EmployeesID == id).OrderByDescending(m => m.CreateTime).Take(20);
            return await list.ToListAsync();

        }
    }
}
