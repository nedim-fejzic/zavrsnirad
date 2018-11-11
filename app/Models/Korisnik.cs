using System;
using System.Collections.Generic;
using System.Linq;  
using System.Web;

namespace app.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Naselje { get; set; }
        public string JMBG { get; set; }
        public string Adresa{ get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public DateTime DatumUnosa { get; set; }
        public DateTime DatumIzmjene { get; set; }

        public int OpcinaId { get; set; }
        public virtual Opcine Opcina { get; set; }

        public string  Username { get; set; }
        public string Password { get; set; }

        // username
        // password


    }
}