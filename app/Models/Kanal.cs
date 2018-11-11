using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class Kanal
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Sifra { get; set; }

        public int ZanrId { get; set; }
        public virtual Zanr Zanr { get; set; }

        public int KvalitetId { get; set; }
        public virtual Kvalitet Kvalitet { get; set; }

        public int JezikId { get; set; }
        public virtual Jezik Jezik { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}