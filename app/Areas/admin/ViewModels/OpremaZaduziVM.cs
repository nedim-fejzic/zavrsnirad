using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class OpremaZaduziVM
    {
        public int Id { get; set; }
        public int OpremaId { get; set; }

        public Uredjaj device { get; set; }


        [Display(Name = "Korisnik")]
        [Required(ErrorMessage = "Odaberite korisnika!")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite korisnika!")]
        public int KorisnikId { get; set; }
        public List<Korisnik> ListaKorisnika { get; set; }

        [DataType(DataType.DateTime) ]
        [Required(ErrorMessage = "Unesite datum zaduženja!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime   DatumZaduzenja { get; set; }

       

    }
}