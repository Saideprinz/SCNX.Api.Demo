using Microsoft.AspNetCore.Mvc;
using SCNX.Api.Controllers;
using SCNX.Api.Models;
using System;
using Xunit;

namespace SCNX.Api.Test
{
    public class HelloControllerTest
    {
        HelloController helloController;
        
        [Fact]
        public void GetUser()
        {
            using(var _context=new CoreDbContext())
            {
                helloController = new HelloController(_context);
                string validUser = "John";
                string invalidUser = "John@123";

                var okResult = helloController.GetUsers(validUser);
                var badrequestResult = helloController.GetUsers(invalidUser);

                Assert.IsType<ActionResult<TblUser>>(okResult);              
                Assert.IsType<ActionResult<TblUser>>(badrequestResult);
            }           
        }

        //[Fact]
        //public void GetUser()
        //{
        //    using (var _context = new CoreDbContext())
        //    {
        //        helloController = new HelloController(_context);
        //        string validUser = "John";
        //        string invalidUser = "John@123";

        //        var okResult = helloController.GetUsers(validUser);
        //        var badrequestResult = helloController.GetUsers(invalidUser);

        //        Assert.IsType<ActionResult<TblUser>>(okResult);
        //        Assert.IsType<ActionResult<TblUser>>(badrequestResult);
        //    }
        //}

    }
}
