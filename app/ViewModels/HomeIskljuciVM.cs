using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class HomeIskljuciVM
    {
        public int KorisnikId { get; set; }

        public int AktivnaUslugaId { get; set; }
        public AktivneUsluge Usluga { get; set; }


      [Required]
        public int RazlogId { get; set; }
        public List<Razlog> ListaRazloga { get; set; }

        public string Napomena { get; set; }
    }
}