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

        public void AddPoll(string title, DateTime start_time, DateTime end_time, ApplicationUser user, List<string> options)
        {
            List<Option> list_of_options = new List<Option>();

            if (options.Count() < 2)
            {
                throw new NotEnoughOptionsException();
            }

            foreach (var option_item in options)
            {
                list_of_options.Add(new Option { Content = option_item });
            }
            Poll new_poll = new Poll { Title = title, EndDate = end_time, StartDate = start_time, CreatedBy = user, Options = list_of_options};
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

        public void AddTag(string new_tag_name)
        {
            Tag new_tag = new Tag{ Name = new_tag_name };
            context.Tags.Add(new_tag);
            context.SaveChanges();
        }

        public bool TagExists(string tag_name)
        {
            Tag found_tag = context.Tags.FirstOrDefault(i => i.Name == tag_name.ToLower());
            return found_tag == null ? false : true; // Teriary IF Statement
        }

        public int FindTagByName(string tag_name)
        {
            Tag found_tag = context.Tags.FirstOrDefault(i => i.Name == tag_name.ToLower());
            return found_tag == null ? -1 : found_tag.TagId;
        }

        public void AddTagToPoll(int poll_id, string tag_name)
        {
            Poll found_poll = GetPoll(poll_id);

            int tag_id;
            if (TagExists(tag_name))
            {
                tag_id = FindTagByName(tag_name);
            }
            else
            {
                AddTag(tag_name);
                tag_id = FindTagByName(tag_name);
            }
            context.PollTagRelations.Add(new PollTag { Poll = found_poll, Tag = context.Tags.FirstOrDefault(i => i.TagId == tag_id) });
            
        }

        public List<string> GetTags(int pollId)
        {
            List<string> tags = new List<string>();
            foreach (var item in context.PollTagRelations)
            {
                if (item.Poll.PollId == pollId)
                {
                    tags.Add(item.Tag.Name);
                }
            }
            return tags;
        }


        // Create a Poll

        // Delete a Poll

        // Vote
    }
}