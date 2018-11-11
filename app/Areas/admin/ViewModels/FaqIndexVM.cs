using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class FaqIndexVM
    {
        public List<FaqRed> FaqRedovi { get; set; }
        public List<FAQKategorija> ListaKategorija { get; set; }
    }

    public class FaqRed
    {
        public int Id { get; set; }
        public string NazivKategorije { get; set; }
        public int KategorijaId { get; set; }
        public string Pitanje { get; set; }
        public string Odgovor { get; set; }
        public bool Vidljiv { get; set; }
    }
}