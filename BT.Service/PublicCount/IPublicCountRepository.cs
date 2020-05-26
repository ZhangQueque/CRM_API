using BT.Data.Entities;
using BT.Data.Repository;
using BT.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace BT.Service.PublicCount
{
    public interface IPublicCountRepository : IBaseRepositoryT<PublicCounts>, IBaseRepositoryTID<PublicCounts, int>
    {
    }
}
