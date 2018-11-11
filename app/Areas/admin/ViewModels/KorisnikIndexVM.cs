using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class KorisnikIndexVM
    {
        public List<KorisnikRed> KorisniciRedovi { get; set; }
    }

    public class KorisnikRed
    {

        public int Id { get; set; }
        public string ImePrezime { get; set; }
        public string JMBG { get; set; }
        public string Opcina { get; set; }


    }


}