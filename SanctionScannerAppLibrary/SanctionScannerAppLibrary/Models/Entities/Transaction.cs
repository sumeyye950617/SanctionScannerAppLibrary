using System;
using System.Collections.Generic;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class Transaction
    {
        public Transaction()
        {
            Punishments = new HashSet<Punishment>();
        }

        public int Id { get; set; }
        public int? BookNo { get; set; }
        public int? UserNo { get; set; }
        public byte? EmployeeNo { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? UserReturnBook { get; set; }
        public bool? IsTransaction { get; set; }
        public DateTime? InsertTime { get; set; }
        public string? InsertedBy { get; set; }

        public virtual Book? BookNoNavigation { get; set; }
        public virtual Employee? EmployeeNoNavigation { get; set; }
        public virtual User? UserNoNavigation { get; set; }
        public virtual ICollection<Punishment> Punishments { get; set; }
    }
}
