using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class VijestiDetaljiVM
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public DateTime DatumObjave { get; set; }
        public DateTime DatumIzmjene { get; set; }

        public string Kategorija { get; set; }

        public string PrikazDatumObjave { get; set; }
        public string PrikazDatumIzmjene { get; set; }
        public bool Vidljivo { get; set; }
        public string Opis { get; set; }
        public string ImageUrl { get; set; }
    }
}