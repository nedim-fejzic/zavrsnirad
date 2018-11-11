using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Zahtjev
    {
        public int Id { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }

        public string NazivUlice { get; set; }
        public string Broj { get; set; }
        public string JMBG { get; set; }

        public int OpcinaId { get; set; }
        public virtual Opcine Opcina { get; set; }

        public string Naselje { get; set; }

        public string Email { get; set; }
        public string Telefon { get; set; }

        public int TipKontaktaId { get; set; }
        public virtual TipKontakta TipKontakta { get; set; }

        public string NapomenaKorisnika { get; set; }

        public DateTime DatumPodnosenja { get; set; }

        public int ZahtjevstatusId { get; set; }
        public virtual ZahtjevStatus ZahtjevStatus { get; set; }

        public Boolean ZatvorenZahtjev { get; set; }
        public DateTime DatumZatvaranja { get; set; }
        public string Komentar { get; set; }

    }
}