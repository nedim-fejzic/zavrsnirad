using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class SmetnjeDetaljiVM
    {
        public int Id { get; set; }
        public string Sifra { get; set; }
        public Korisnik Korisnik { get; set; }
        public string DatumUocavanja { get; set; }
        public string Usluga { get; set; }

        public string DatumPodnosenja { get; set; }
        public string DatumZatvaranja { get; set; }

        public string StatusSmetnje { get; set; }
        public string Opis { get; set; }

        [Display(Name = "Odgovor")]
        [Required(ErrorMessage = "Odgovor mora sadrzavati tekst!")]
        public string Odgovor { get; set; }



        public List<SmetnjeOdgovori> ListaOdgovora { get; set; }

    }
}