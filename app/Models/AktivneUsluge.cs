using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class AktivneUsluge
    {

        public int Id { get; set; }
        public int KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }



        public int PaketId { get; set; }
        public virtual Paket Paket { get; set; }

        public int ZahtjevId { get; set; }
        public string AdresaInstalacije { get; set; }
        public DateTime DatumInstalacije { get; set; }
        public DateTime DatumAktivacije { get; set; }
        public DateTime ? DatumZatvaranja { get; set; }
        public bool AktivnaUsluga { get; set; }


    }
}