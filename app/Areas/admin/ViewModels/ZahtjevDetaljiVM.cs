using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class ZahtjevDetaljiVM
    {
        public int Id { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Opcina { get; set; }
        public string Naselje { get; set; }
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string JMBG{ get; set; }

        public string Email { get; set; }
        public string Telefon { get; set; }
        public string TipKontakta { get; set; }

        public string Napomena { get; set; }
        public string DatumZahtjeva { get; set; }

        public int StatusId { get; set; }
        public string Status { get; set; }

        [Display(Name = "Komentar: ")]
        [Required(ErrorMessage = "Potrebno je unijeti komentar!")]
        public string Komentar { get; set; }


        public List<ZahtjevUsluge> ListaTraznihUsluga { get; set; }


       

}
}