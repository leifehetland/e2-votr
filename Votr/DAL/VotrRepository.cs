using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Votr.Models;

namespace Votr.DAL
{
    public class VotrRepository
    {
        public VotrContext context { get; set; }

        public VotrRepository()
        {
            // We need an instance of a Context
            context = new VotrContext();
        }

        public VotrRepository(VotrContext _context)
        {
            context = _context;
        }

        public int GetPollCount()
        {
            //return GetPolls().Count;
            // Another way
            return context.Polls.Count();
        }

        public List<Poll> GetPolls()
        {
            return context.Polls.ToList<Poll>();
        }

        public void AddPoll(string title, DateTime start_time, DateTime end_time)
        {
            Poll new_poll = new Poll { Title = title, EndDate = end_time, StartDate = start_time};
            context.Polls.Add(new_poll);
            context.SaveChanges();
        }

        public Poll GetPoll(int _poll_id)
        {
            //return context.Polls.Find(_poll_id); // Requires explicit mocking of the DbSet.Find method
            Poll poll;
            try
            {
                poll = context.Polls.First(i => i.PollId == _poll_id);
            } catch (Exception)
            {
                throw new NotFoundException();
            }
            return poll; // ConnectMockstoDatastore made this possible
        }

        public void RemovePoll(int _poll_id)
        {
            Poll some_poll = context.Polls.First(i => i.PollId == _poll_id);

            context.Polls.Remove(some_poll);
            context.SaveChanges();
        }

        public Poll GetPollOrNull(int _poll_id)
        {
            return context.Polls.FirstOrDefault(i => i.PollId == _poll_id);
        }

        public void EditPoll(Poll poll_to_edit)
        {
            context.Entry(poll_to_edit).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
        // Create a Poll

        // Delete a Poll

        // Vote
    }
}