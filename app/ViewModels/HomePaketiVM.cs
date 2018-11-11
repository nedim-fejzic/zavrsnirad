using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class HomePaketiVM
    {
        public List<PaketRed> PaketRedovi { get; set; }
        public List<TipUsluga> KategorijeL { get; set; }
    }

    public class PaketRed
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string SlikaPutanja { get; set; }
        public string DownSpeed { get; set; }
        public string UpSpeed { get; set; }
        public string Flat { get; set; }
        public string CijenaSaPDV { get; set; }
        public int KategorijaId { get; set; }
        public string Kategorija { get; set; }
    }

}