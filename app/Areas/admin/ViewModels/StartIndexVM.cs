using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class StartIndexVM
    {
        public string ProdajaKorisnici { get; set; }
        public string ProdajaZahtjevi { get; set; }
        public string ProdajaRacuni { get; set; }

        public string PodrskaSmetnje { get; set; }
        public string PodrskaUputstva { get; set; }
        public string PodrskaLozinka { get; set; }

        public string MenadzerVijesti { get; set; }
        public string MenadzerFaq { get; set; }
        public string MenadzerOprema { get; set; }
        public string MenadzerPaketi { get; set; }
        public string MenadzerAktivneUsluge { get; set; }



    }
}