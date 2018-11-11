using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Uputstva
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public bool Vidljivo { get; set; }

        public string NazivDokumenta { get; set; }
        public string TipDokumenta { get; set; }

        public string Putanja { get; set; }

        public int UputstvoKategorijaID { get; set; }
        public virtual UputstvaKategorije UputstvaKategorije { get; set; }


    }
}