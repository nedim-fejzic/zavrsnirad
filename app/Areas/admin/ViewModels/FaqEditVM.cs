using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace app.Areas.admin.ViewModels
{
    public class FaqEditVM
    {
        public int Id { get; set; }

        [Display(Name = "Pitanje kategorija")]
        [Required(ErrorMessage = "Odaberite kategoriju pitanja!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite kategoriju pitanja!")]
        public int KategorijaId { get; set; }
        public List<FAQKategorija> ListaKategorija { get; set; }

        [Required(ErrorMessage = "Pitanje je obavezno polje!")]
        public string Pitanje { get; set; }
        [Required(ErrorMessage = "Odgovor je obavezno polje!")]
        [AllowHtml]
        public string Odgovor { get; set; }


        public bool Vidljiv { get; set; }
    }
}