using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime ModifiedDAte{ get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BGColor { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsReminder { get; set; }
        public bool IsTrash { get; set; }
        public virtual User User { get; set; }


    }
}
