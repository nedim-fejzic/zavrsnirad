using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class ZahtjevDodajVM
    {

        [Display(Name = "Ime: ")]
        [Required(ErrorMessage = "Potrebno je unijeti ime korisnika!")]
        public string Ime { get; set; }

        [Display(Name = "Prezime: ")]
        [Required(ErrorMessage = "Potrebno je unijeti prezime korisnika!")]
        public string Prezime { get; set; }

        [Display(Name = "JMBG:")]
        [Required(ErrorMessage = "Potrebno je unijeti JMBG korisnika!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Unesite validan JMBG")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG mora sadrzati 16 brojeva")]
        public string JMBG { get; set; }

        [Display(Name = "Mjesto priključka: ")]
        [Required(ErrorMessage = "Odaberite mjesto prebivališta!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite mjesto prebivališta!")]
        public int OpcinaId { get; set; }
        public List<Opcine> ListaOpcina { get; set; }

        [Display(Name = "Naselje: ")]
        [Required(ErrorMessage = "Potrebno je Vaše naselje!")]
        public string Naselje { get; set; }

        [Display(Name = "Ulica: ")]
        [Required(ErrorMessage = "Potrebno je unijeti ulicu!")]
        public string NazivUlice { get; set; }

        [MaxLength(5)]
        [Display(Name = "Broj: ")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Kucni broj je potrebno da bude broj!")]
        public string Broj { get; set; }



        [Display(Name = "Email adresa: ")]
        [Required(ErrorMessage = "Potrebno je unijeti email adresu")]
        [EmailAddress(ErrorMessage = "Niste unijeli validnu email adresu")]
        public string Email { get; set; }

        [Display(Name = "Telefon: ")]
        [Required(ErrorMessage = "Unesite broj telefona!")]
        public string Telefon { get; set; }

        [Display(Name = "Kako želite da Vas kontaktiramo: ")]
        [Required(ErrorMessage = "Izaberite kako želite da Vas kontaktiramo")]
        public int TipKontaktaId { get; set; }
        [Display(Name = "Kako želite da Vas kontaktiramo: ")]
        public List<TipKontakta>  ListaTipKontakta { get; set; }


        public int PaketId { get; set; }
        public List<Paket>  ListaPaket { get; set; }
        public List<ListaPaketaView>  ListaPaketica { get; set; }

        [Required(ErrorMessage = "Izaberite kako usluge želite aktivirati: ")]
        public List<int> IzabraniPaketi { get; set; }


        [Display(Name = "Napomena korisnika: ")]
        public string NapomenaKorisnika { get; set; }



    }
}