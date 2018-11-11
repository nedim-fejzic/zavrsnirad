using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class RacunPrintajVM
    {
        public int Id { get; set; }
        public DateTime PeriodOd { get; set; }
        public DateTime PeriodDo { get; set; }
        public DateTime RokPlacanja { get; set; }
        public DateTime DatumIzdavanja { get; set; }

        public string Placen { get; set; }

        public List<RacuniStavke> ListaStavki { get; set; }

        public string Sifra { get; set; }
        public string UkupnoPDV { get; set; }
        public string PDF { get; set; }
        public string Ukupno { get; set; }

        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public decimal PDV { get; set; }
    }
}