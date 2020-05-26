using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BT.Service.Permission
{
    public  interface IPermissionRepository : IBaseRepositoryT<Permissions>, IBaseRepositoryTID<Permissions, int>
    {
    }
}
