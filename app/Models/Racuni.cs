using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Racuni
    {
        public int Id { get; set; }
        public int KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }

        public string Sifra { get; set; }


        public DateTime ObracunskiPeriodOD { get; set; }
        public DateTime ObracunskiPeriodDO { get; set; }


        public DateTime DatumIzdavanja { get; set; }
        public DateTime RokPlacanja { get; set; }

        public double UkupnoSaPDV { get; set; }
        public double UkupnoBezPDV { get; set; }



        public Boolean Placen { get; set; }
        public DateTime ?  DatumPlacanja { get; set; }









    }
}