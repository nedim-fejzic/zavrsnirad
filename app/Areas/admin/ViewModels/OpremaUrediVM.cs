using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class OpremaUrediVM
    {


        public int Id { get; set; }

        [Display(Name = "Naziv uređaja: ")]
        [Required(ErrorMessage = "Potrebno je unijeti naziv uređaja!")]
        public string Naziv { get; set; }

        [Display(Name = "Naziv proizvodžač: ")]
        [Required(ErrorMessage = "Potrebno je unijeti naziv proizvodžača!")]
        public string Proizvodzac { get; set; }

        [Display(Name = "Serijski broj: ")]
        [Required(ErrorMessage = "Potrebno je unijeti serijski broj uređaja!")]
        public string SerijskiBroj { get; set; }

        [Required(ErrorMessage = "Potrebno je unijeti datum nabavke!")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime DatumNabavke { get; set; }

        public string DatumString { get; set; }

   



    }
}