﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BirdyAPI.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserContext context, IConfiguration configuration)
        {
            _userService = new UserService(context, configuration);
        }
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserAccountDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult GetUserInfo([FromQuery] UserSessionDto user)
        {
            try
            {
                return Ok(_userService.SearchUserInfo(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpGet]
        [Route("show")]
        [Produces(typeof(List<User>))]
        public IEnumerable<User> GetUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpPost]
        [Route("setAvatar")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult SetAvatar([FromQuery]int id, [FromBody] byte[] photoBytes)
        {
            try
            {
                _userService.SetProfileAvatar(id, photoBytes);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
