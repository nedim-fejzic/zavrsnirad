using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class PaketiDetailsVM
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public string ImageUrl { get; set; }
        public string AltText { get; set; }

        public string DownloadSpeed { get; set; }
        public string UploadSpeed { get; set; }
        public string FlatRate { get; set; }

        public float CijenaBezPdv { get; set; }
        public float CijenaSaPdv { get; set; }

        public bool AktivnaUsluga { get; set; }

        public int TipUslugaId { get; set; }
        public string TipUsluge { get; set; }
    }
}