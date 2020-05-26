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
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CRMContext context;

        public ProductsController(IWrapperRepository wrapperRepository, IMapper mapper, IDistributedCache distributedCache, CRMContext context)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.context = context;
        }



        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Products>>> GetProductsAsync()
        {
            return await context.Products.ToListAsync();
        }

    }
}
