using BT.Core;
using BT.Data;
using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BT.Service.Employee
{
    /// <summary>
    /// 员工仓储实现类
    /// </summary>
    public class EmployeeRepository:BaseRepository<Employees, int>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context):base(context)
        {

        }

        /// <summary>
        /// 员工登录
        /// </summary>
        /// <param name="loginDto">登陆对象</param>
        /// <returns></returns>
        public async Task<Employees> LoginAsync(LoginDto loginDto)
        {
            var encryptPassword = MD5Encrypt.Encrypt(loginDto.Password);

            //md5  加密密码，与数据库密码进行比对

            var emp = await context.Set<Employees>()
                .FirstOrDefaultAsync(m=>m.Email== loginDto.Email
                && m.Password== encryptPassword
                && m.IsDel==0);

            return emp;
        }
    }
}
