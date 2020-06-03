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
using BT.Core;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.Json;

namespace BT.API.Controllers
{
    [Route("api/excels")]
    [ApiController]
    public class ExcelsController : ControllerBase
    {
        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CRMContext context;

        public ExcelsController(IWrapperRepository wrapperRepository, IMapper mapper, IDistributedCache distributedCache, CRMContext context)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.context = context;
        }


        /// <summary>
        /// 员工批量上传
        /// </summary>
        /// <param name="excelUploadDto">上传的Excel文件</param>
        /// <returns></returns>
        [HttpPost("employee_excel")]
        public async Task<IActionResult> EmployeeExcelUploadAsync([FromForm] ExcelUploadDto excelUploadDto)
        {
            var id = Convert.ToInt32(User.Identity.Name);
            if (excelUploadDto.UploadFile == null)
            {
                return NotFound();
            }
            try
            {
                var list = await EPPlusHelper.UploadExcel<EmployeeExcelDto>(excelUploadDto.UploadFile);
                // NPOI     EPPlus   
                List<Employees> createList = new List<Employees>();
                foreach (var item in list)
                {
                    Employees employee = new Employees();
                    employee = mapper.Map<Employees>(item);
                    employee.CreateTime = DateTime.Now;
                    employee.CreateID = id;
                    employee.IsParent = 0;
                    employee.EmployeePID = id;
                    employee.Code = Guid.NewGuid().ToString();
                    employee.Password = MD5Encrypt.Encrypt("123456");
                    createList.Add(employee);
                }

                string json = JsonSerializer.Serialize(createList);

                await distributedCache.SetStringAsync($"excel_employees_List_ByUser_{id}", json);

                return Ok(new { code = 0, data = createList });
            }
            catch (System.Exception ex)
            {
                return Ok(new { code = 1, data = ex.Message });
            }
        }


        /// <summary>
        /// 获取Excel的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("employee_excel_layui")]
        public async Task<IActionResult> GetExcelToLayuiEmployeesAsync( int page, int limit)
        {

            var id = Convert.ToInt32(User.Identity.Name);


            List<Employees> employees = JsonSerializer.Deserialize<List<Employees>>(await distributedCache.GetStringAsync($"excel_employees_List_ByUser_{id}"));

            return Ok(new { code = 0, data = employees.Skip((page - 1) * limit).Take(limit), msg = "", count = employees.Count });

        }


        /// <summary>
        /// Excel员工批量添加
        /// </summary>
        /// <returns></returns>
        [HttpGet("employee_excel_create")]
        public async Task<IActionResult> CreateRangeEmployeesAsync()
        {
            var id = Convert.ToInt32(User.Identity.Name);

            try
            {
                List<Employees> employees = JsonSerializer.Deserialize<List<Employees>>(await distributedCache.GetStringAsync($"excel_employees_List_ByUser_{id}"));


                await context.Employees.AddRangeAsync(employees);

                await context.SaveChangesAsync();
                return Ok(new { code = 0, msg = "批量添加成功！" });
            }
            catch (Exception)
            {
                return Ok(new { code = 1, msg = "添加失败，请联系管理员！" });

            }
        }


        /// <summary>
        /// 员工模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("employee_excel_down")]
        public IActionResult 员工Excel导入模板()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", "员工Excel导入模板.xlsx");

            return PhysicalFile(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}
