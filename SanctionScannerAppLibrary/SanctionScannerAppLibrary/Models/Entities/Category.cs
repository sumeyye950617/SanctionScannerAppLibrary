using System;
using System.Collections.Generic;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        public byte Id { get; set; }
        public string? Name { get; set; }
        public DateTime? InsertTime { get; set; }
        public string? InsertedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
