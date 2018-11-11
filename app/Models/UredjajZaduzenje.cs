using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class UredjajZaduzenje
    {
        public int Id { get; set; }
        public int UredjajId { get; set; }
        public virtual Uredjaj Uredjaj { get; set; }

        public int KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }

        public DateTime DatumZaduzenja { get; set; }
        public DateTime ? DatumPovratka { get; set; }

        public bool Vracen { get; set; }
    }
}