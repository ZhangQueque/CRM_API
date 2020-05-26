using BT.Data.Entities;
using BT.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Service.PublicCount
{
    public class PublicCountRepository : BaseRepository<PublicCounts, int>, IPublicCountRepository
    {
        public PublicCountRepository(DbContext context) :base(context)
        {

        }
    }
}
