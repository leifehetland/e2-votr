using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Votr.DAL;
using Votr.Models;

namespace Votr.Controllers
{
    public class VotesController : ApiController
    {
        private VotrContext db = new VotrContext();
        private VotrRepository Repo = new VotrRepository();

        // GET: api/Votes
        public IQueryable<Vote> GetVotes()
        {
            return db.Votes;
        }

        // GET: api/Votes/5
        [ResponseType(typeof(Vote))]
        public IHttpActionResult GetVote(int id)
        {
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }

        // PUT: api/Votes/5/
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutVote(int id, [FromUri]int optionselected)
        {

            bool success = Repo.CastVote(id, User.Identity.GetUserId(), optionselected);

            if (success)
            {
                return StatusCode(HttpStatusCode.NoContent);

            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

        }

        // POST: api/Votes
        [ResponseType(typeof(Vote))]
        public IHttpActionResult PostVote(Vote vote)
        {
            //Get User ID form the HTTP Context
            string user_id = User.Identity.GetUserId();
            ApplicationUser user = Repo.GetUser(user_id);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Votes.Add(vote);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vote.VoteId }, vote);
        }

        // DELETE: api/Votes/5
        [ResponseType(typeof(Vote))]
        public IHttpActionResult DeleteVote(int id)
        {
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return NotFound();
            }

            db.Votes.Remove(vote);
            db.SaveChanges();

            return Ok(vote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoteExists(int id)
        {
            return db.Votes.Count(e => e.VoteId == id) > 0;
        }
    }
}