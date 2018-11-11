using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class VijestDetaljiVM
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string DatumObjave { get; set; }
        public string Kategorija { get; set; }
        public string Opis { get; set; }
        public string ImageUrl { get; set; }
    }
}