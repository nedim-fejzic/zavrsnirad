using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Vijesti
    {
        public int Id { get; set; }
        public string Naslov { get; set; }



        public DateTime DatumObjave { get; set; }
        public DateTime? DatumPosljednjeIzmjene { get; set; }

        public bool Vidljivo { get; set; }
        public string Opis { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public string AltText { get; set; }


        public int VijestiKategorijaId { get; set; }
        public virtual VijestiKategorija VijestiKategorija { get; set; }

        //postavioid
    }
}