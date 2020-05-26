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
using StackExchange.Redis;

namespace BT.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]

    public class OrdersController : ControllerBase
    {
        private readonly IWrapperRepository wrapperRepository;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly CRMContext context;

        public OrdersController(IWrapperRepository wrapperRepository, IMapper mapper, IDistributedCache distributedCache, CRMContext context)
        {
            this.wrapperRepository = wrapperRepository;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
            this.context = context;
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="dto">传输对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] Orders_ProductsCreateDto dto)
        {

            var id = Convert.ToInt32(User.Identity.Name);
            //新增订单
            var order = mapper.Map<Orders>(dto.OrderCreateDto);

            order.CreateID = id;
            order.CreateTime = DateTime.Now;
            order.OrderNo = Guid.NewGuid().ToString();

            await context.Orders.AddAsync(order);

            await context.SaveChangesAsync();


            var customer = await context.Customers.FindAsync(order.CustomerID);

            if (customer==null)
            {
                return NotFound();
            }
            customer.IsReal = 1;

              context.Customers.Update(customer);

            await context.SaveChangesAsync();

            //新增订单产品记录

            List<Orders_Products> orders_Products = new List<Orders_Products>();
            foreach (var item in dto.ProductsCreateDtos)
            {
                Orders_Products orders_Products1 = new Orders_Products();
                orders_Products1.ProductID = item.ID;
                orders_Products1.ProductTitle = item.Title;
                orders_Products1.Num = item.Number;
                orders_Products1.CreateID = id;
                orders_Products1.CreateTime = DateTime.Now;
                orders_Products1.OrderID = order.ID;
                orders_Products.Add(orders_Products1);
             
            }

            try
            {
                await context.Orders_Products.AddRangeAsync(orders_Products);

                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }
    }
}
