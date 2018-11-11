using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class ProfilIndexVM
    {
        public int Id { get; set; }
        public string  ImePrezime { get; set; }
        public string  JBMG { get; set; }
        public string  Opcina { get; set; }
        public string Naselje { get; set; }
        public string Adresa { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }

        public List<AktivneUsluge> ListaAktivnihUsluga { get; set; }

    }
}