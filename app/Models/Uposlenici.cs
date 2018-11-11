using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Uposlenici
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public Boolean Aktivan { get; set; }

        public int UlogaId { get; set; }
        public virtual Uloga Uloga{ get; set; }


    }
}