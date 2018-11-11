using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class HomeFaqVM
    {
        public List<FaqRed> FaqRedovi { get; set; }
    }

    public class FaqRed
    {
        public int Id { get; set; }
        public int KategorijaId { get; set; }
        public string Pitanje { get; set; }
        public string Odgovor { get; set; }
    }
}