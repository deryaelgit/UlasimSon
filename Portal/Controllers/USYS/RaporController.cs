using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Portal.Controllers.Admin;
using Portal.Models;
using Portal.Models.Admin;
using Portal.Models.USYS;
using Portal.ViewModels.USYS;
using System.Linq;

namespace Portal.Controllers.USYS
{
    [Route("Rapor")]
    public class RaporController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public RaporController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("rapor")] // Bu satırla rota belirtiliyor
        public IActionResult Index()
        {
            var model = new RaporViewModel
            {
                IhlalTutanakSayisi = _context.Denetimler.Count(d => d.DenetimTuruId == 401),
                IhtarTutanakSayisi = _context.Denetimler.Count(d => d.DenetimTuruId == 402),
                DenetimSayisi = _context.AracDenetimleri.Count(),
                ToplamAracSayisi = _context.AracPlakalari.Count(),

                CezaSayilari = _context.DenetimCezalari
                    .Include(dc => dc.CezaYonetmelik)
                    .GroupBy(dc => dc.CezaYonetmelik.AnahtarKelime)
                    .Select(g => new CezaSayisi
                    {
                        YonetmelikAnahtarKelime = g.Key.ToString(),
                        CezaAdedi = g.Count()
                    })
                    .OrderByDescending(x => x.CezaAdedi)
                    .ToList(),

                AracDenetimSayilari = _context.AracDenetimleri
                    .Include(ad => ad.AracPlaka)
                    .GroupBy(ad => ad.AracPlaka.PlakaNumarasi)
                    .Select(g => new AracDenetimSayisi
                    {
                        PlakaNumarasi = g.Key,
                        DenetimAdedi = g.Count()
                    })
                    .OrderByDescending(x => x.DenetimAdedi)
                    .ToList(),

                KullaniciDenetimSayilari = _context.AracDenetimleri
                    .Include(ad => ad.Kullanici)
                    .GroupBy(ad => ad.Kullanici.FullName)
                    .Select(g => new KullaniciDenetimSayisi
                    {
                        KullaniciAdi = g.Key,
                        DenetimAdedi = g.Count()
                    })
                    .OrderByDescending(x => x.DenetimAdedi)
                    .ToList(),

                PlakaSahipler = _context.PlakaSahipleri.ToList(),
                PlakaCezaListesi = _context.PlakaCezaGenel.ToList()
            };

            return View(model);
        }
        [HttpGet("GetPlates")]
        public IActionResult GetPlates(string term)
        {
            var plakalar = _context.AracPlakalari
                .Where(p => p.PlakaNumarasi.Contains(term))
                .Select(p => new { value = p.Id, text = p.PlakaNumarasi })
                .ToList();

            return Json(plakalar);
        }
        [HttpGet]
        public IActionResult GetTutanakDataByPlaka(int plakaId)
        {
            var ihlalSayisi = _context.Denetimler
                .Where(d => d.AracPlakaId == plakaId && d.DenetimTuruId == 401).Count();
            var ihtarSayisi = _context.Denetimler
                .Where(d => d.AracPlakaId == plakaId && d.DenetimTuruId == 402).Count();
            var denetimSayisi = _context.AracDenetimleri
                .Where(d => d.AracPlakaId == plakaId).Count();

            var data = new { Ihlal = ihlalSayisi, Ihtar = ihtarSayisi, Denetim = denetimSayisi }; 
            
            return Json(data); 
        
        }


        [HttpGet("getTutanakVerileri")]
        public async Task<IActionResult> GetTutanakVerileri(string timeRange = null, string dataType = "ihlal")
        {
            var now = DateTime.Now;

            if (dataType == "ihlal")
            {
                IQueryable<Denetim> query = _context.Denetimler;
                query = query.Where(t => t.DenetimTuruId == 401); // 401 id'li İhlal verilerini alır

                // Zaman aralığına göre filtreleme
                if (!string.IsNullOrEmpty(timeRange) && timeRange != "all")
                {
                    if (timeRange == "year")
                    {
                        query = query.Where(t => t.OlusturmaTarihi >= now.AddYears(-1));
                    }
                    else if (timeRange == "month")
                    {
                        query = query.Where(t => t.OlusturmaTarihi >= now.AddMonths(-1));
                    }
                    else if (timeRange == "week")
                    {
                        query = query.Where(t => t.OlusturmaTarihi >= now.AddDays(-7));
                    }
                    else
                    {
                        return BadRequest("Geçersiz timeRange parametresi.");
                    }
                }

                var tutanakVerileri = await query
                    .GroupBy(t => t.OlusturmaTarihi.Date)
                    .Select(g => new
                    {
                        Tarih = g.Key,
                        Sayi = g.Count()
                    })
                    .ToListAsync();

                var labels = tutanakVerileri.Select(v => v.Tarih.ToString("yyyy-MM-dd")).ToArray();
                var values = tutanakVerileri.Select(v => v.Sayi).ToArray();

                return Ok(new { labels, values });
            }
            else if (dataType == "denetim")
            {
                IQueryable<AracDenetim> query = _context.AracDenetimleri;

                // Zaman aralığına göre filtreleme
                if (!string.IsNullOrEmpty(timeRange) && timeRange != "all")
                {
                    if (timeRange == "year")
                    {
                        query = query.Where(t => t.OlusturmaTarihi >= now.AddYears(-1));
                    }
                    else if (timeRange == "month")
                    {
                        query = query.Where(t => t.OlusturmaTarihi >= now.AddMonths(-1));
                    }
                    else if (timeRange == "week")
                    {
                        query = query.Where(t => t.OlusturmaTarihi >= now.AddDays(-7));
                    }
                    else
                    {
                        return BadRequest("Geçersiz timeRange parametresi.");
                    }
                }

                var denetimVerileri = await query
                    .GroupBy(t => t.OlusturmaTarihi.Date)
                    .Select(g => new
                    {
                        Tarih = g.Key,
                        Sayi = g.Count()
                    })
                    .ToListAsync();

                var labels = denetimVerileri.Select(v => v.Tarih.ToString("yyyy-MM-dd")).ToArray();
                var values = denetimVerileri.Select(v => v.Sayi).ToArray();

                return Ok(new { labels, values });
            }

            return BadRequest("Geçersiz dataType parametresi.");
        }


        // PlakaCeza verilerini almak için yeni bir metot
        [HttpGet("getPlakaCezaVerileri")]
        public IActionResult GetPlakaCezaVerileri()
        {
            var plakaCezaVerileri = _context.PlakaCezaGenel.ToList();
            return Ok(plakaCezaVerileri);
        }
    }
}




