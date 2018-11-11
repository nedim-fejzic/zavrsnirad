using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class OpremaDodajVM
    {
        public int Id { get; set; }

        [Display(Name = "Naziv uređaja: ")]
        [Required(ErrorMessage = "Potrebno je unijeti naziv uređaja!")]
        public string Naziv { get; set; }

        [Display(Name = "Naziv proizvodžač: ")]
        [Required(ErrorMessage = "Potrebno je unijeti naziv proizvodžača!")]
        public string Proizvodzac { get; set; }

        public string SerijskiBroj { get; set; }

        [Required(ErrorMessage = "Potrebno je unijeti datum nabavke!")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime DatumNabavke { get; set; }

        public string DatumString { get; set; }

        [Display(Name = "Količina: ")]
        [Required(ErrorMessage = "Potrebno je unijeti broj nabavljenih uređaja!")]
        [Range(1, 1000, ErrorMessage = "Potrebno je unijeti količinu nabavljenih uređaja!")]
        public int Kolicina { get; set; }


        public List<string> ListaSerijskihBrojeva { get; set; }

    }
}