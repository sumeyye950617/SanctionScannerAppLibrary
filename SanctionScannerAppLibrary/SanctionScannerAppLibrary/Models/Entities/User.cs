using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class User
    {
        public User()
        {
            Punishments = new HashSet<Punishment>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]

        public string? Mail { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Picture { get; set; }
        [RegularExpression(@"^\+?(\d{1,3})?[-. ]?(\d{3})[-. ]?(\d{3})[-. ]?(\d{4})$", ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string? Phone { get; set; }
        public string? School { get; set; }
        public DateTime? InsertTime { get; set; }
        public string? InsertedBy { get; set; }

        public virtual ICollection<Punishment> Punishments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
