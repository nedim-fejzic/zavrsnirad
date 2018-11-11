using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class RacuniStavke
    {
        public int Id { get; set; }

        public int RacunId { get; set; }
        public virtual Racuni Racun { get; set; }

        public double IznosBezPDV { get; set; }
        public double IznosSaPDV { get; set; }

        public DateTime DatumPocetka { get; set; }
        public DateTime DatumKraja { get; set; }


        public int ? AktivneUslugeId { get; set; }
        public virtual AktivneUsluge AktivnaUsluga { get; set; }

    }
}