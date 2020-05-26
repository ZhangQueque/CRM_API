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
    public interface IWrapperRepository
    {
        IEmployeeRepository EmployeeRepository { get; }

        IRoleRepository RoleRepository { get; }
        IEmployee_RoleRepository Employee_RoleRepository { get; }

        ILogCategoryRepository LogCategoryRepository { get; }

        ILogRepository LogRepository { get; }

        IRole_PermissionRepository Role_PermissionRepository { get; }

        IPermissionRepository PermissionRepository { get; }


        IPublicCountRepository PublicCountRepository { get; }
    }
}
