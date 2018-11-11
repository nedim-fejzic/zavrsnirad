using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class SmetnjeIndexVM
    {
        public List<SmetnjeStatus> ListaStatusa { get; set; }
        public int OdabraniStatus { get; set; }

        public List<Korisnik> ListaKorisnika { get; set; }
        public int OdabraniKorisnik { get; set; }

        public string Sifra { get; set; }

        public IPagedList<Smetnje> ListaRezultata { get; set; }

        public string BrojSmetnji { get; set; }
        public string BrojAktivnihSmetnji { get; set; }


    }
}