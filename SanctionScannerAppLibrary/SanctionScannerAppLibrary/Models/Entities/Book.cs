using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class Book
    {
        public Book()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Kitap adı en fazla {1} karakter olmalıdır.")]

        public string? Name { get; set; }
        public byte? CategoryNo { get; set; }
        public int? WriterNo { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Yayın yılı dört haneli bir sayı olmalıdır.")]
        public string? PublicationYear { get; set; }

        [StringLength(100, ErrorMessage = "Yayıncı adı en fazla {1} karakter olmalıdır.")]

        public string? Publisher { get; set; }
        public string? Picture { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool? IsActive { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Sayfa sayısı yalnızca rakamlardan oluşmalıdır.")]
        public string? Page { get; set; }
        public DateTime? InsertTime { get; set; }
        public string? InsertedBy { get; set; }

        public virtual Category? CategoryNoNavigation { get; set; }
        public virtual Writer? WriterNoNavigation { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

    }
}
