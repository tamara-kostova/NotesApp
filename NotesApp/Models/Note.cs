using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesApp.Models
{
    public class Note
    {
        [Key]
        public string NoteId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteDetail { get; set; }
        public DateTime Date { get; set; }
    }
}