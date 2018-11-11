using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class ZahtjevIskljuciVM
    {
        public int ZahtjevId { get; set; }
        public int AktivnaUslugaId { get; set; }
        public AktivneUsluge AktivnaUsluga { get; set; }

        public DateTime Datum { get; set; }
        public string Napomena { get; set; }
        public string Razlog { get; set; }

    }
}