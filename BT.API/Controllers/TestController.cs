using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BT.API.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
 
        [HttpGet]
        public   IActionResult GetTokenData( )
        {
            var email= User.Claims.FirstOrDefault(m=>m.Type==ClaimTypes.Email).Value;
        
            return Ok(User.Identity.Name+ email);
        }


        [HttpGet("admin")]
        [Authorize(Roles ="管理员")]
        public IActionResult Admin()
        {
             
            return Ok("管理员才能看到的东西");
        }


        [HttpGet("dong")]
        [Authorize(Roles = "管理员")]
        [Authorize(Roles = "董事长")]  //并且
        public IActionResult Dong()
        {
            
            return Ok("管理员才能看到的东西");
        }



        [HttpGet("yg")]
        [Authorize(Roles = "员工,管理员")] //或
        public IActionResult YuanGong()
        {
 
            return Ok("员工才能看到的东西");
        }


    }
}