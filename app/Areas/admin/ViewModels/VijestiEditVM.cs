using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace app.Areas.admin.ViewModels
{
    public class VijestiEditVM
    {
        public int Id { get; set; }

        [Display(Name = "Naziv vijesti")]
        [Required(ErrorMessage = "Potrebno je unijeti naziv vijesti!")]
        public string Naziv { get; set; }


        [Display(Name = "Sadrzaj vijesti")]
        [Required(ErrorMessage = "Potrebno je unijeti sadrzaj vijesti!")]
        [AllowHtml]
        public string Opis { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }


        [Display(Name = "Vijest kategorija")]
        [Required(ErrorMessage = "Odaberite kategoriju vijesti!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite kategoriju vijesti!")]
        public int KategorijaId { get; set; }
        public List<VijestiKategorija> ListaKategorija { get; set; }

        [Required]
        public bool Vidljivo { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Photo")]
        public HttpPostedFileBase ImageUpload { get; set; }

        public string ImageUrl { get; set; }

        public string AltText { get; set; }
    }
}