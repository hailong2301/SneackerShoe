using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DataAccess.Repository.Products;
using DataAccess.Repository.Users;
using AutoMapper;
using Microsoft.AspNetCore.OData.Query;
using SneakerShoeStoreAPI.DTO;

namespace SneakerShoeStoreAPI.Controllers
{
    public class UsersController : ODataController
    {
        private readonly SneakerShoeStoreContext _context;
        private IUserRepository repository = new UserRepository();

        public UsersController(SneakerShoeStoreContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {

            return repository.GetUsers().AsQueryable().ToList();
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<User> Get(int key)
        {
            return repository.GetUserById(key);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDTO userDto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            var user = mapper.Map<UserDTO, User>(userDto);
            repository.AddUser(user);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put(int key, [FromBody] UserDTO userDto)
        {
            var user = repository.GetUserById(key);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            user = mapper.Map<UserDTO, User>(userDto);
            user.UserId = key;
            repository.UpdateUser(user);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(int key)
        {
            var user = repository.GetUserById(key);
            if (user == null)
            {
                return NotFound();
            }
            repository.DeleteUser(user);
            return NoContent();
        }


    }
}
