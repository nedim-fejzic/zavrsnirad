using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class KorisnikLozinkaVM
    {
        public int KorisnikId { get; set; }
        public List<Korisnik> ListaKorisnika { get; set; }

        public string Lozinka { get; set; }

    }
}