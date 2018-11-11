using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class KanaliPaketi
    {
        public int Id { get; set; }
        public DateTime DatumDodavanja { get; set; }

        public int PaketId { get; set; }
        public virtual Paket Paket { get; set; }

        public int KanalId { get; set; }
        public virtual Kanal Kanal { get; set; }
    }
}