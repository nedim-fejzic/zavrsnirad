using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class KorisnikDodajVM
    {
        [Display(Name = "Ime korisnika")]
        [Required(ErrorMessage = "Potrebno je unijeti ime korisnika!")]
        public string Ime { get; set; }

        [Display(Name = "Prezime korisnika")]
        [Required(ErrorMessage = "Potrebno je unijeti prezime korisnika!")]
        public string Prezime { get; set; }

        [Display(Name = "Naselje korisnika")]
        [Required(ErrorMessage = "Potrebno je unijeti trenutno naselje korisnika!")]
        public string Naselje { get; set; }

        [Display(Name = "JMBG korisnika")]
        [Required(ErrorMessage = "Potrebno je unijeti JMBG korisnika!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Unesite validan JMBG")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG mora sadrzati 13 brojeva")]
        public string JMBG { get; set; }

        [Display(Name = "Adresa korisnika")]
        [Required(ErrorMessage = "Potrebno je unijeti adresu korisnika!")]
        public string Adresa { get; set; }

        [Display(Name = "Email adresa: ")]
        [Required(ErrorMessage = "Potrebno je unijeti email adresu")]
        [EmailAddress(ErrorMessage = "Niste unijeli validnu email adresu")]
        public string Email { get; set; }

        [Display(Name = "Telefon: ")]
        [Required(ErrorMessage = "Unesite broj telefona!")]
        public string Telefon { get; set; }

        public DateTime DatumUnosa { get; set; }

        public DateTime DatumIzmjene { get; set; }


        [Display(Name = "Opcina: ")]
        [Required(ErrorMessage = "Odaberite opcinu prebivališta!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite opcinu prebivališta!")]
        public int OpcinaId { get; set; }
        public List<Opcine> ListaOpcina { get; set; }


    }
}