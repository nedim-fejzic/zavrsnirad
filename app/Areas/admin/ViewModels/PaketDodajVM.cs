using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class PaketDodajVM
    {

        [Display(Name = "Naziv paketa")]
        [Required(ErrorMessage = "Potrebno je unijeti naziv paketa!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Odaberite sliku!")]
        [DataType(DataType.Upload)]
        [Display(Name = "Photo")]
        public HttpPostedFileBase ImageUpload { get; set; }

        public string ImageUrl { get; set; }

        public string AltText { get; set; }

        [Display(Name = "Download speed")]
        public string DownloadSpeed { get; set; }

        [Display(Name = "Upload speed")]
        public string UploadSpeed { get; set; }

        [Display(Name = "Neograničeni promet")]
        [Required(ErrorMessage = "Potrebno je izabrati da li promet neogranicen ili ne!")]
        public bool FlatRate { get; set; }

        [Display(Name = "Cijena bez PDV")]
        [Required(ErrorMessage = "Unesite cijenu bez PDV")]
        [Range(0.1, float.MaxValue, ErrorMessage = "Unesite validan iznos")]
        public float CijenaBezPdv { get; set; }

        [Display(Name = "Cijena bez PDV")]
        [Range(0.1, float.MaxValue, ErrorMessage = "Unesite validan iznos")]
        [Required(ErrorMessage = "Unesite cijenu bez PDV")]
        public float CijenaSaPdv { get; set; }

        [Display(Name = "Aktivna usluga")]
        [Required(ErrorMessage = "Potrebno je izabrati da li usluga aktivna ili ne!")]
        public bool AktivnaUsluga { get; set; }


        [Display(Name = "Kategorija usluga")]
        [Required(ErrorMessage = "Odaberite kategoriju paketa!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite kategoriju paketa!")]
        public int TipUslugaId { get; set; }
        public List<TipUsluga> ListaTipovaUsluga { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]        public DateTime Datum { get; set; }
    }
}