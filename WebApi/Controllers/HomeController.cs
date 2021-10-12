using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController:ControllerBase
    {
        //http://localhost:5000/api/Home/GetHello
        [HttpGet]
        [Route("GetHello")]
        public string GetMessage()
        {
            return "Hello";
        }

        [HttpGet]
        [Route("Index")]
        public string GetIndex()
        {
            return "Index";
        }
    }
}
