using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class OpremaDodaj2VM
    {
        public OpremaDodajVM modelOpreme { get; set; }

        [Display(Name = "Serijski broj: ")]
        [Required(ErrorMessage = "Potrebno je unijeti serijski broj uređaja!")]
        public List<string> ListaSerijskihBrojeva { get; set; }
    }
}