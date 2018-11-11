using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class UputstvaDetaljiVM
    {

        public int Id { get; set; }
        public string Naslov { get; set; }

        public string DatumDodavanja { get; set; }
        public string NazivDokumenta { get; set; }
        public string TipDokumenta { get; set; }
        public string Putanja { get; set; }

        public string Vidljivo { get; set; }

        public string Kategorija { get; set; }



    }
}