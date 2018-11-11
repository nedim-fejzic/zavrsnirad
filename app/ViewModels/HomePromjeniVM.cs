using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class HomePromjeniVM
    {
        public int KorisnikId { get; set; }

        public int AktivnaUslugaId { get; set; }
        public AktivneUsluge Usluga { get; set; }

        [Display(Name = "Trazena usluga: ")]
        [Required(ErrorMessage = "Molimo izaberite novu uslugu!")]
        public int IzabranaId{ get; set; }
        public List<Paket> ListaNovihUsluga{ get; set; }


        public string Napomena { get; set; }

    }
}