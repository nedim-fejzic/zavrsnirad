using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class UputstvoUrediVM
    {

        public int Id { get; set; }

        [Display(Name = "Naziv uputstva: ")]
        [Required(ErrorMessage = "Potrebno je unijeti naziv uputstva!")]
        public string Naslov { get; set; }

        public DateTime DatumDodavanja { get; set; }
        public string NazivDokumenta { get; set; }
        public string TipDokumenta { get; set; }

        public bool Vidljivo { get; set; }

       

        public string Putanja { get; set; }


        [Display(Name = "Uputstvo kategorija")]
        [Required(ErrorMessage = "Odaberite kategoriju uputstvo!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite kategoriju uputstvo!")]
        public int UputstvoKategorijaID { get; set; }
        public List<UputstvaKategorije> ListaKategorija { get; set; }


        [DataType(DataType.Upload)]
        [Display(Name = "Dokumment")]
        public HttpPostedFileBase FileUpload { get; set; }



    }
}