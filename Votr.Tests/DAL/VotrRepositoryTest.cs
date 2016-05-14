using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Votr.DAL;
using System.Collections.Generic;
using Votr.Models;
using Moq;
using System.Linq;
using System.Data.Entity;

namespace Votr.Tests.DAL
{
    [TestClass]
    public class VotrRepositoryTest
    {
        Mock<VotrContext> mock_context { get; set; }
        VotrRepository repo { get; set; } // Injects mocked (fake) VotrContext

        // Polls
        List<Poll> polls_datasource { get; set; } 
        Mock<DbSet<Poll>> mock_polls_table { get; set; } // Fake Polls table
        IQueryable<Poll> poll_data { get; set; }// Turns List<Poll> into something we can query with LINQ

        // Tags
        List<Tag> tags_datasource { get; set; }
        Mock<DbSet<Tag>> mock_tags_table { get; set; } // Fake Tags table
        IQueryable<Tag> tag_data { get; set; }

        // PollTags
        List<PollTag> polltags_datasource { get; set; }
        Mock<DbSet<PollTag>> mock_polltags_table { get; set; } // Fake Tags table
        IQueryable<PollTag> polltag_data { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            mock_context = new Mock<VotrContext> { CallBase = true};

            tags_datasource = new List<Tag>();
            polls_datasource = new List<Poll>();
            polltags_datasource = new List<PollTag>();


            mock_polls_table = new Mock<DbSet<Poll>>(); // Fake Polls table
            mock_tags_table = new Mock<DbSet<Tag>>();
            mock_polltags_table = new Mock<DbSet<PollTag>>();


            repo = new VotrRepository(mock_context.Object); // Injects mocked (fake) VotrContext
            poll_data = polls_datasource.AsQueryable(); // Turns List<Poll> into something we can query with LINQ
            tag_data = tags_datasource.AsQueryable();
            polltag_data = polltags_datasource.AsQueryable();

        }

        [TestCleanup]
        public void Cleanup()
        {
            polls_datasource = null;
        }

        void ConnectMocksToDatastore() // Utility method
        {
            // Telling our fake DbSet to use our datasource like something Queryable
            mock_polls_table.As<IQueryable<Poll>>().Setup(m => m.GetEnumerator()).Returns(poll_data.GetEnumerator());
            mock_polls_table.As<IQueryable<Poll>>().Setup(m => m.ElementType).Returns(poll_data.ElementType);
            mock_polls_table.As<IQueryable<Poll>>().Setup(m => m.Expression).Returns(poll_data.Expression);
            mock_polls_table.As<IQueryable<Poll>>().Setup(m => m.Provider).Returns(poll_data.Provider);

            // Tell our mocked VotrContext to use our fully mocked Datasource. (List<Poll>)
            mock_context.Setup(m => m.Polls).Returns(mock_polls_table.Object);


            // Telling our fake DbSet to use our datasource like something Queryable
            mock_polltags_table.As<IQueryable<PollTag>>().Setup(m => m.GetEnumerator()).Returns(polltag_data.GetEnumerator());
            mock_polltags_table.As<IQueryable<PollTag>>().Setup(m => m.ElementType).Returns(polltag_data.ElementType);
            mock_polltags_table.As<IQueryable<PollTag>>().Setup(m => m.Expression).Returns(polltag_data.Expression);
            mock_polltags_table.As<IQueryable<PollTag>>().Setup(m => m.Provider).Returns(polltag_data.Provider);

            // Tell our mocked VotrContext to use our fully mocked Datasource. (List<PollTag>)
            mock_context.Setup(m => m.PollTagRelations).Returns(mock_polltags_table.Object);

            // Telling our fake DbSet to use our datasource like something Queryable
            mock_tags_table.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(tag_data.GetEnumerator());
            mock_tags_table.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(tag_data.ElementType);
            mock_tags_table.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(tag_data.Expression);
            mock_tags_table.As<IQueryable<Tag>>().Setup(m => m.Provider).Returns(tag_data.Provider);

            // Tell our mocked VotrContext to use our fully mocked Datasource. (List<Tag>)
            mock_context.Setup(m => m.Tags).Returns(mock_tags_table.Object);



            // Hijack the call to the Add methods and put it the list using the List's Add method.
            mock_polls_table.Setup(m => m.Add(It.IsAny<Poll>())).Callback((Poll poll) => polls_datasource.Add(poll));
            mock_tags_table.Setup(m => m.Add(It.IsAny<Tag>())).Callback((Tag tag) => tags_datasource.Add(tag));
            mock_polltags_table.Setup(m => m.Add(It.IsAny<PollTag>())).Callback((PollTag poll_tag) => polltags_datasource.Add(poll_tag));

        }

        [TestMethod]
        public void RepoEnsureICanCreateInstance()
        {
            //VotrRepository repo = new VotrRepository();
            Assert.IsNotNull(repo);
        }

        [TestMethod]
        public void RepoEnsureIsUsingContext()
        {
            // Arrange 
            //VotrRepository repo = new VotrRepository();

            // Act

            // Assert
            Assert.IsNotNull(repo.context);
        }

        [TestMethod]
        public void RepoEnsureThereAreNoPolls()
        {
            // Arrange 
            ConnectMocksToDatastore();

            // Act
            List<Poll> list_of_polls = repo.GetPolls();
            List<Poll> expected = new List<Poll>();

            // Assert
            Assert.AreEqual(expected.Count, list_of_polls.Count);
        }

        [TestMethod]
        public void RepoEnsurePollCountIsZero()
        {
            // Arrange 
            ConnectMocksToDatastore();

            // Act
            int expected = 0;
            int actual = repo.GetPollCount();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RepoEnsureICanAddPoll()
        {
            // Arrange
            ConnectMocksToDatastore();

            
            // Act
            ApplicationUser user = null;
            repo.AddPoll("Some Title", DateTime.Now, DateTime.Now, user); // Not there yet.
            int actual = repo.GetPollCount(); 
            int expected = 1;

            // Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void RepoEnsureICanNotFindOrNull()
        {
            // Arrange
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            Poll poll_in_db_2 = new Poll { PollId = 2, Title = "Some Title 2", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);
            polls_datasource.Add(poll_in_db_2);

            polls_datasource.Remove(poll_in_db_2);

            ConnectMocksToDatastore();

            // Act
            Poll found_poll = repo.GetPollOrNull(5);

            // Assert
            Assert.IsNull(found_poll);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void RepoEnsureICanNotFind()
        {
            // Arrange
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            Poll poll_in_db_2 = new Poll { PollId = 2, Title = "Some Title 2", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);
            polls_datasource.Add(poll_in_db_2);

            polls_datasource.Remove(poll_in_db_2);

            ConnectMocksToDatastore();

            // Act
            repo.GetPoll(5);
        }

        [TestMethod]
        public void RepoEnsureICanDeletePoll()
        {
            // Arrange
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            Poll poll_in_db_2 = new Poll { PollId = 2, Title = "Some Title 2", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);
            polls_datasource.Add(poll_in_db_2);

            ConnectMocksToDatastore();
            mock_polls_table.Setup(m => m.Remove(It.IsAny<Poll>())).Callback((Poll poll) => polls_datasource.Remove(poll));


            // Act
            repo.RemovePoll(1);

            // Assert
            int expected_count = 1;
            Assert.AreEqual(expected_count, repo.GetPollCount());

            try
            {
                repo.GetPoll(1);
                Assert.Fail();
            } catch (Exception) { }
            
        }

        [TestMethod]
        public void RepoEnsureICanGetAPoll()
        {
            // Arrange
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            Poll poll_in_db_2 = new Poll { PollId = 2, Title = "Some Title 2", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);
            polls_datasource.Add(poll_in_db_2);

            ConnectMocksToDatastore();

            // Act
            Poll found_poll = repo.GetPoll(1);

            // Assert
            Assert.IsNotNull(found_poll);
            Assert.AreEqual(poll_in_db, found_poll);
        }

        [TestMethod]
        public void RepoEnsureICanEditPoll()
        {
            // Arrange
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            Poll poll_in_db_2 = new Poll { PollId = 2, Title = "Some Title 2", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);
            polls_datasource.Add(poll_in_db_2);

            ConnectMocksToDatastore();

            // Act
            int poll_to_edit_id = 1;
            Poll poll_to_edit = repo.GetPollOrNull(poll_to_edit_id); // Happens when /Poll/Edit/1 is called
            poll_to_edit.Title = "Changed";

            repo.EditPoll(poll_to_edit);

            // Assert
            Poll edited_poll = repo.GetPollOrNull(poll_to_edit_id);
            Assert.AreEqual(edited_poll.Title, "Changed");
        }

        [TestMethod]
        public void RepoFindTagByName()
        {
            // Arrange
            Tag tag_1 = new Tag { TagId = 1, Name = "foo" };
            tags_datasource.Add(tag_1);
            ConnectMocksToDatastore();

            // Act
            int tag_id = repo.FindTagByName("foo");

            // Assert
            Assert.AreEqual(1, tag_id);
        }

        [TestMethod]
        public void RepoFindTagByNameNotThere()
        {
            // Arrange
            Tag tag_1 = new Tag { TagId = 1, Name = "foo" };
            tags_datasource.Add(tag_1);
            ConnectMocksToDatastore();

            // Act
            int tag_id = repo.FindTagByName("bar");

            // Assert
            Assert.AreEqual(-1,tag_id);
        }

        [TestMethod]
        public void RepoEnsureTagExists()
        {
            // Arrange
            Tag tag_1 = new Tag { TagId = 1, Name = "foo" };
            tags_datasource.Add(tag_1);
            ConnectMocksToDatastore();

            // Act
            bool tag_exists = repo.TagExists("foo");

            // Assert
            Assert.IsTrue(tag_exists);

        }

        [TestMethod]
        public void RepoEnsureICanCreateATag()
        {
            // Arrange
            ConnectMocksToDatastore();

            // Act
            repo.AddTag("footag");


            // Assert
            Assert.IsTrue(repo.TagExists("footag")); 
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void RepoEnsureCanNotAddTagToMissingPoll()
        {
            // Arrange
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);

            ConnectMocksToDatastore();

            // Act
            repo.AddTagToPoll(2, "foo");
        }

        [TestMethod]
        public void RepoEnsureICanGetAListOfTags()
        {
            // Arrange 
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);

            Tag tag_in_db = new Tag { TagId = 1, Name = "foo" };
            tags_datasource.Add(tag_in_db);

            PollTag polltag_in_db = new PollTag { PollTagId = 1, Poll = poll_in_db, Tag = tag_in_db };
            polltags_datasource.Add(polltag_in_db); 
            ConnectMocksToDatastore();

            // Act
            List<string> actual_tags = repo.GetTags(poll_in_db.PollId);
            List<string> expected_tags = new List<string> { "foo" };

            // Assert
            CollectionAssert.AreEqual(expected_tags, actual_tags);
        }

        [TestMethod]
        public void RepoEnsureICanCreateAPollWithTag()
        {
            // Arrange
            Poll poll_in_db = new Poll { PollId = 1, Title = "Some Title", StartDate = DateTime.Now, EndDate = DateTime.Now };
            polls_datasource.Add(poll_in_db);
            ConnectMocksToDatastore();

            // Act
            int poll_id = 1;

            repo.AddTagToPoll(poll_id, "footag");

            // Assert
            

        }
    }
}
