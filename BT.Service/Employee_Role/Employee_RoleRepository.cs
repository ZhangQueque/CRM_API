using BT.Core;
using BT.Data;
using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Service.Employee_Role
{

    /// <summary>
    /// 员工_角色仓储实现类
    /// </summary>
    public class Employee_RoleRepository:BaseRepository<Employees_Roles, int>, IEmployee_RoleRepository
    {
        public Employee_RoleRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 根据员工ID获取对应的角色
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <returns></returns>
        public Task<IQueryable<Employees_Roles>> GetEmployees_RolesByEmployeeIDAsync(int employeeID)
        {
            return Task.FromResult(
                context.Set<Employees_Roles>()
               .Where(m => m.EmployeeID == employeeID && m.IsDel == 0)
            );
        }
    }
}
