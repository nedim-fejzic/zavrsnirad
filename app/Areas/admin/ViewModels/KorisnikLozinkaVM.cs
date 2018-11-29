using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class KorisnikLozinkaVM
    {
        public int KorisnikId { get; set; }
        public List<Korisnik> ListaKorisnika { get; set; }


        [Required(ErrorMessage = "Unesite lozinku!")]
        [RegularExpression("((?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20})", ErrorMessage = "Lozinka mora sadržavati barem jedan broj, malo i veliko slovo! Veličina lozinke mora biti u rasponu 6-20 karaktera.")]
        public string Lozinka { get; set; }

    }
}