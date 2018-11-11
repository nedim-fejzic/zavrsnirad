using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Uredjaj
    {
        public int Id { get; set; }
        public string Proizvodjac { get; set; }
        public string Naziv { get; set; }
        public string SerijskiBroj { get; set; }
        public DateTime DatumNabavke { get; set; }
        public bool Otpisan { get; set; }
        public bool UUpotrebi { get; set; }
    }
}