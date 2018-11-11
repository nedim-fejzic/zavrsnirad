using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class ZahtjevUsluge
    {

        public int Id { get; set; }
        public int ZahtjevId { get; set; }
        public virtual Zahtjev Zahtjev { get; set; }


        public int PaketId { get; set; }
        public virtual Paket Paket { get; set; }


    }
}