using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Models.Admin;

namespace Portal.Models.USYS
{


    public class Video
    {
        [Key]
        public int Id { get; set; } // Benzersiz kimlik numarası (Primary Key)

        // Videonun benzersiz GUID değeri (DataGUID)
        public Guid DataGuid { get; set; }

        // Videonun binary verisi (Data)
        public byte[]? Veri { get; set; } // Video içeriğini tutmak için byte array

        // Dosya adı (FileName)
        [StringLength(255)]
        public string? DosyaAdi { get; set; }

        // Dosya türü (FileType), bu tür LookUpList'ten gelecek.
        [ForeignKey("DosyaTuruId")]
        public int? DosyaTuruId { get; set; }
        public LookupList? DosyaTuru { get; set; } // Dosya türü ile ilişki

        // Oluşturulma tarihi (CreatedOn)
        public DateTime? OlusturmaTarihi { get; set; } = DateTime.Now;

        // Videoyu oluşturan kullanıcı bilgisi (CreatedBy)
        [ForeignKey("OlusturanKullaniciId")]
        public int OlusturanKullaniciId { get; set; }
        public User OlusturanKullanici { get; set; } // Kullanıcı ile ilişki

        public ICollection<AracDenetimGecmisiFotoVideo>? AracDenetimGecmisiFotoVideolari { get; set; } // Videolar koleksiyonu
    }
}