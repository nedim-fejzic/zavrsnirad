using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Smetnje
    {
        public int Id { get; set; }
        [Required]
        public string BrojSmetnje { get; set; }
        public int? AktivnaUslugaId { get; set; }
        public virtual AktivneUsluge AktivnaUsluga { get; set; }
        public DateTime? DatumUocavanja { get; set; }
        public string Opis { get; set; }

        public int SmetnjeStatusId { get; set; }
        public virtual SmetnjeStatus SmetnjeStatus { get; set; }

        // korisnik id
        public int ? KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }

        public DateTime DatumOtvaranja { get; set; }
        public DateTime ? DatumZatvaranja { get; set; }

        public int ? UposlenikId { get; set; }
        public virtual Uposlenici  Uposlenici { get; set; }
        // datum zatvaranja
        // zatvorio id





    }
}