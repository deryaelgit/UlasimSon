using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{
    public class DenetimGecmisi
    {
          [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası

        // MinutesId ile ilişkilendirilecek. Denetim tablosundaki Id ile bağlantılı olacak.
        [Required]
        public int DenetimId { get; set; } // MinutesId
        public Denetim Denetim { get; set; } // Denetim tablosu ile ilişki

        // Yorum (Comment) alanı
        public string? Yorum { get; set; } // Comment

        // StatusId ile ilişkilendirilecek. Status bilgisi LookupList tablosundan gelecek
        [ForeignKey("LookupList")]
        public int CezaDurumId { get; set; } // StatusId
        public LookupList CezaDurum { get; set; } // Durum bilgisi, LookupList'ten gelecek

        // Geçmişin oluşturulma tarihi
        [Required]
        public DateTime OlusturmaTarihi { get; set; } // CreatedOn

        // Geçmişi oluşturan kullanıcı, User tablosu ile ilişkilendirilecek
        [ForeignKey("User")]
        public int OlusturanKullaniciId { get; set; } // CreatedBy
        public User OlusturanKullanici { get; set; } // Oluşturan kullanıcı bilgisi, User tablosundan gelecek

        // Geçmiş kaydının durum tarihi
        public DateTime? DurumTarihi { get; set; } // StatusDate

        // Uyarı süresi (WarningDuration)
        public int? UyariSuresi { get; set; } // WarningDuration
                public ICollection<DenetimGecmisiFoto>? DenetimGecmisiFotolari { get; set; } // Fotoğraflar koleksiyonu

        
    }
}