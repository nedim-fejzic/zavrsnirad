using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class KorisnikSearchVM
    {
        public string JMBG { get; set; }

        public string Sifra { get; set; }
        public string ImePrezime { get; set; }
        public IPagedList<Korisnik> ListaRezultata { get; set; }
    }
}