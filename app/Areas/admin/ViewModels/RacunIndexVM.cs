using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class RacunIndexVM
    {

        public string Naziv { get; set; }

        public List<Korisnik> ListaKorisnika { get; set; }
        public int OdabranKorisnik { get; set; }


        public IPagedList<Racuni> ListaRezultata { get; set; }


        public string tip { get; set; }
        public string PlacenoRacuna { get; set; }
        public string NijePlacenoRacuna { get; set; }

    }
}