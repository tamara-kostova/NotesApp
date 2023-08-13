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
            //Note oldNoteModel = (Note)db.Notes.Find(note.NoteId);
            if (db.Notes.Find(note.NoteId)==null)
            {
                //Add new
                Note newNote = new Note();
                newNote.NoteId = Guid.NewGuid().ToString();
                newNote.Date = DateTime.UtcNow;
                newNote.NoteTitle = note.NoteTitle;
                newNote.NoteDetail = note.NoteDetail;
                db.Notes.Add(newNote);
                db.SaveChanges();
                return Json(newNote, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Update
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return Json(note, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult updateNoteById(string NoteId, string NoteTitle, string NoteDetail)
        {
            Note note = db.Notes.Find(NoteId);
            db.Entry(note).State = EntityState.Deleted;
            db.SaveChanges();
            Note newNote = new Note();
            newNote.NoteId = NoteId;
            newNote.Date = DateTime.UtcNow;
            newNote.NoteTitle = NoteTitle;
            newNote.NoteDetail = NoteDetail;
            db.Notes.Add(newNote);
            db.SaveChanges();
            return Json(newNote, JsonRequestBehavior.AllowGet);
        }
    }
}