using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class Lable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LableId { get; set; }
        public int? UserId { get; set; }
        public int? NoteId { get; set; }
        public  User User { get; set; }
        public  Note Note { get; set; }
        public string LableName { get; set; }
    }
}
