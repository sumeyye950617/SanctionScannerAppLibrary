using System;
using System.Collections.Generic;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            Transactions = new HashSet<Transaction>();
        }

        public byte Id { get; set; }
        public string? Employee1 { get; set; }
        public DateTime? InsertTime { get; set; }
        public string? InsertedBy { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
