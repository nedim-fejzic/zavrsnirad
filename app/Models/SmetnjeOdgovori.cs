using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class SmetnjeOdgovori
    {
        public int Id { get; set; }
        [Required]
        public int SmetnjaId { get; set; }

        public DateTime Datum { get; set; }
        public string Poruka { get; set; }

        public int ? KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }
        public int ? UposlenikId { get; set; }
        public virtual Uposlenici Uposlenik { get; set; }

    }
}