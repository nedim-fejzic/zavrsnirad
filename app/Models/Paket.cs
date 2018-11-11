using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Paket
    {
        public int Id { get; set; }
        [Required]
        public string Naziv { get; set; }

        public string ImageUrl { get; set; }
        public string AltText { get; set; }

        public string DownloadSpeed { get; set; }
        public string UploadSpeed { get; set; }
        public bool FlatRate { get; set; }

        public float CijenaBezPdv { get; set; }
        public float CijenaSaPdv { get; set; }

        public bool AktivnaUsluga { get; set; }

        public int TipUslugaId { get; set; }
        public virtual TipUsluga TipUsluga { get; set; }
    }
}