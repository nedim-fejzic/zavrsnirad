using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class OpremaRazduziVM
    {
        public int Id { get; set; }
        public int OpremaId { get; set; }

        public Korisnik user { get; set; }
        public Uredjaj device { get; set; }

        public DateTime DatumZaduzenja { get; set; }



        [Required(ErrorMessage = "Unesite datum povratka uređaja!")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DatumVracanja { get; set; }
    }
}