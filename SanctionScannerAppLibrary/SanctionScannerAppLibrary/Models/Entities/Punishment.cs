using System;
using System.Collections.Generic;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class Punishment
    {
        public int Id { get; set; }
        public int? UserNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Money { get; set; }
        public int? TransactionNo { get; set; }
        public DateTime? InsertTime { get; set; }
        public string? InsertedBy { get; set; }

        public virtual Transaction? TransactionNoNavigation { get; set; }
        public virtual User? UserNoNavigation { get; set; }
    }
}
