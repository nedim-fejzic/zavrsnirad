using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Pitanje { get; set; }
        public string Odgovor { get; set; }
        public bool Vidljiv { get; set; }
        public int FAQKategorijaId { get; set; }
        public virtual FAQKategorija FAQKategorija { get; set; }

        public DateTime DatumDodavanja { get; set; }
    }
}