using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCNX.Api.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SCNX.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public HelloController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<TblUser> GetUsers()
        {
            try
            {
                var userList = _context.TblUser.ToList();
                if (userList == null)
                {
                    return BadRequest();
                }
                return Ok(userList);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{name}")]
        public ActionResult<TblUser> GetUsers(string name)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    return BadRequest();
                }

                string responseMessage = string.Empty;
                UserManagementResponse response = new UserManagementResponse();

                var userList = _context.TblUser.ToList();
                var user = userList?.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
                if (user == null)
                {
                    return BadRequest();
                }

                var currentDate = DateTime.Now;
                if (user.BirthDate.Month == currentDate.Month && user.BirthDate.Day == currentDate.Day)
                {
                    responseMessage = $"Hello, {user.Name}! Happy birthday!";
                }
                else if (user.BirthDate.Month >= currentDate.Month && user.BirthDate.Day > currentDate.Day)
                {
                    responseMessage = $"Hello, Your birthday is in {(user.BirthDate - currentDate).TotalDays:0.00} day(s)";
                }
                else if (user.BirthDate.Month <= currentDate.Month && user.BirthDate.Day < currentDate.Day)
                {
                    responseMessage = "Hello, Your birthday has ended already";
                }

                response.message = responseMessage;

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest();
            } 
        }

        // PUT api/<HelloController>/5
        [HttpPut("{name}")]
        public ActionResult<TblUser> PutTblUser(string name, [FromBody]UserManagementRequest userinfo)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    return BadRequest();
                }

                DateTime dt;
                string[] formats = { "yyyy-MM-dd" };
                if (!DateTime.TryParseExact(userinfo.dateOfBirth.ToShortDateString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    return BadRequest();
                }

                if (userinfo.dateOfBirth > DateTime.Now || (userinfo.dateOfBirth-DateTime.Now).Days==0)
                {
                    return BadRequest();
                }

                var userList = _context.TblUser.ToList();
                var user = userList?.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
                if (user == null)
                {
                    return BadRequest();
                }

                user.Name = name.Trim();
                user.BirthDate = Convert.ToDateTime(userinfo.dateOfBirth.ToShortDateString());

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        private bool UserExists(string name)
        {
            try
            {
                return _context.TblUser.Any(e => e.Name.ToLower() == name.ToLower());
            }
            catch(Exception e)
            {
                throw;
            }
        }

    }
}
