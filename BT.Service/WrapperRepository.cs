using BT.Data;
using BT.Data.Entities;
using BT.Service.Employee;
using BT.Service.Employee_Role;
using BT.Service.Log;
using BT.Service.LogCategory;
using BT.Service.Permission;
using BT.Service.PublicCount;
using BT.Service.Role;
using BT.Service.Role_Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Service
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly CRMContext context;

        public WrapperRepository(CRMContext  context)
        {
            this.context = context;
        }

         public IEmployeeRepository EmployeeRepository => new EmployeeRepository(context);

        public IRoleRepository RoleRepository => new RoleRepository(context);

        public IEmployee_RoleRepository Employee_RoleRepository => new Employee_RoleRepository(context);

        public ILogCategoryRepository LogCategoryRepository => new LogCategoryRepository(context);

        public ILogRepository LogRepository =>new LogRepository(context);

        public IRole_PermissionRepository Role_PermissionRepository => new Role_PermissionRepository(context);

        public IPermissionRepository PermissionRepository => new PermissionRepository(context);

        public IPublicCountRepository PublicCountRepository => new PublicCountRepository(context);

        //public IEmployeeRepository EmployeeRepository { get {return  new EmployeeRepository(context); } }
    }
}
