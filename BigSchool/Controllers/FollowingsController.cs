using BigSchool.DTOs;
using BigSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers
{
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        public FollowingsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var userId = User.Identity.GetUserId();
            // status = true -> follow
            if (followingDto.Status)
            {
                if (_dbContext.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == followingDto.FolloweeId))
                    return BadRequest("Following already exists!");
                var following = new Following
                {
                    FollowerId = userId,
                    FolloweeId = followingDto.FolloweeId
                };
                _dbContext.Followings.Add(following);
            }
            else
            {
                var model = _dbContext.Followings.SingleOrDefault(x => x.FollowerId == userId && x.FolloweeId == followingDto.FolloweeId);
                
                _dbContext.Followings.Remove(model);
            }
                                    
            _dbContext.SaveChanges();
            return Ok();            
        }
        
    }
}