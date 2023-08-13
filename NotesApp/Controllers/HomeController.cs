using NotesApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult updateNote(Note note)
        {
            //string[] parts = noteIddetail.Split('&');
            //Note oldNoteModel =(Note)db.Notes.Find(parts[0]);
            Note oldNoteModel = (Note)db.Notes.Find(note.NoteId);

            if (oldNoteModel == null)
            {
                //Add new
                //Note note = new Note();
                note.NoteId = Guid.NewGuid().ToString();
                note.Date = DateTime.UtcNow;
                db.Notes.Add(note);
                db.SaveChanges();
                return Json(note, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Update
                oldNoteModel.Date = DateTime.UtcNow;
                oldNoteModel.NoteDetail = note.NoteDetail;
                db.SaveChanges();
                return Json(oldNoteModel, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult _note_list()
        {
            var result = db.Notes.OrderByDescending(n => n.Date).AsQueryable();
            return PartialView(result);
        }

        [HttpGet]
        public JsonResult getNoteDetailById(string noteId)
        {
            var note = db.Notes.Find(noteId);
            return Json(note, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult deleteNoteById(string noteId)
        {
            Note note = db.Notes.Find(noteId);
            db.Entry(note).State = EntityState.Deleted;
            db.SaveChanges();

            return Json("Successfully deleted.", JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult updateNoteById(string noteId, string noteDetail)
        //{
        //    Note note = db.Notes.Find(noteId);
        //    note.NoteDetail = noteDetail;
        //    db.Entry(note).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return Json(note, JsonRequestBehavior.AllowGet);
        //}
    }
}