using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Votr.DAL;
using Votr.Models;

namespace Votr.Controllers
{
    public class PollController : Controller
    {
        private VotrRepository Repo = new VotrRepository();
        // GET: Poll
        public ActionResult Index()
        {
            //ViewBag.Polls = Repo.GetPolls();
            return View(Repo.GetPolls());
        }

        // GET: Poll/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Poll/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Poll/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Poll/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Poll found_poll = Repo.GetPollOrNull(id);
            if (found_poll == null)
            {
                return RedirectToAction("Index");
            }
            return View(found_poll);
        }

        // POST: Poll/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include ="PollId,Title,StartDate,EndDate")]Poll poll_to_edit)
        {
            if (ModelState.IsValid)
            {
                Repo.EditPoll(poll_to_edit);
            }
            return RedirectToAction("Index");
        }

        // GET: Poll/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Poll/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
