﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("friends")]
    public class FriendController : ExtendedController
    {
        private readonly FriendService _friendService;
        private readonly ToolService _toolService;

        public FriendController(BirdyContext context)
        {
            _friendService = new FriendService(context);
            _toolService = new ToolService(context);
        }


        /// <summary>
        /// Send friend request
        /// </summary>
        /// <response code = "200">Request sent</response>
        /// <response code = "418">I'm a teapot! Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User by tag not found</response>
        [HttpPost]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 418, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]
        public IActionResult SendFriendRequest([FromBody] string userUniqueTag, [FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                int userId = _toolService.GetUserIdByUniqueTag(userUniqueTag);
                _friendService.SendFriendRequest(userId, currentUserId);
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Teapot(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Accept friend request
        /// </summary>
        /// <response code = "200">Request accepted</response>
        /// <response code = "418">I'm a teapot! Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User by tag not found</response>
        /// <response code = "409">There is no input request from this user</response>
        [HttpPatch]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 418, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]
        [ProducesResponseType(statusCode: 409, type: typeof(void))]
        public IActionResult AcceptFriendRequest([FromBody] string userUniqueTag, [FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                int userId = _toolService.GetUserIdByUniqueTag(userUniqueTag);
                _friendService.AcceptFriendRequest(userId, currentUserId);
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (NullReferenceException)
            {
                return Conflict();
            }   
            catch (Exception ex)
            {
                return Teapot(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Get current user friends
        /// </summary>
        /// <response code = "200">Return list of friends</response>
        /// <response code = "418">I'm a teapot! Unexpected Exception (only for debug)</response>
        /// <response code = "404">User by tag not found</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(List<UserFriend>))]
        [ProducesResponseType(statusCode: 418, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]
        public IActionResult GetFriends([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_friendService.GetFriends(currentUserId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Teapot(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Get user friends
        /// </summary>
        /// <response code = "200">Return list of friends</response>
        /// <response code = "418">I'm a teapot! Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User by tag not found</response>
        [HttpGet]
        [Route("{userUniqueTag}")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<UserFriend>))]
        [ProducesResponseType(statusCode: 418, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]
        public IActionResult GetUserFriends([FromHeader] Guid token, string userUniqueTag)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                int userId = _toolService.GetUserIdByUniqueTag(userUniqueTag);
                return Ok(_friendService.GetFriends(userId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Teapot(ex.SerializeAsResponse());
            }
        }


        /// <summary>
        /// Delete user from friend
        /// </summary>
        /// <response code = "200">Friend deleted</response>
        /// <response code = "418">I'm a teapot! Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User by tag not found</response>
        /// <response code = "403">User is not friend</response>
        [HttpDelete]
        [Route("{friendUniqueTag}")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 418, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]
        public IActionResult DeleteFriend(string friendUniqueTag, [FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                int friendId = _toolService.GetUserIdByUniqueTag(friendUniqueTag);
                _friendService.DeleteFriend(currentUserId, friendId);
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (DataException)
            {
                return Forbid();
            }
            catch(ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Teapot(ex.SerializeAsResponse());
            }
        }
    }
}
