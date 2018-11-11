using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class AktivneUslugeDetaljiVM
    {
        public int Id { get; set; }
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public List<AktivneUsluge> ListaAktivnihUsluga { get; set; }

    }
}