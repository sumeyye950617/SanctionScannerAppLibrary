using System;
using System.Collections.Generic;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class Writer
    {
        public Writer()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Detail { get; set; }
        public DateTime? InsertTime { get; set; }
        public string? InsertedBy { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
