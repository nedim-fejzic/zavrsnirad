using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class AktivneUslugeAktivirajVM
    {
        public int KorisnikId { get; set; }
        public string Korisnik { get; set; }
        public int BrojUsluga { get; set; }

        [Display(Name = "Paketi")]
        [Required(ErrorMessage = "Odaberite paket!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite paket za uključenje!")]
        public int PaketId { get; set; }
        public List<Paket> ListaPaketa { get; set; }

        public string AdresaInstalacije { get; set; }

        [Required(ErrorMessage = "Datum je obavezno polje!")]
        [DataType(DataType.Date)]
        public DateTime ? DatumInstalacije { get; set; }


    }
}