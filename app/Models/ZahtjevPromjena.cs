using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class ZahtjevPromjena
    {

        public int Id { get; set; }

        public int ? KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }

        public int AktivneUslugeId { get; set; }
        public virtual AktivneUsluge AktivneUsluge { get; set; }

        public DateTime Datum { get; set; }

        public bool ZatvorenZahtjev { get; set; }
        public DateTime ? DatumZatvaranja { get; set; }

        public int ZahtjevstatusId { get; set; }
        public virtual ZahtjevStatus ZahtjevStatus { get; set; }

        // ako je null onda je iskljucenje postojece usluge
        public int ? PaketId { get; set; }
        public virtual Paket Paket { get; set; }

        public string Napomena { get; set; }
        public string Razlog { get; set; }

    }
}