using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class OpremaIndexVM
    {

        public string device { get; set; }
        public string tip { get; set; }
        public IPagedList<Uredjaj> ListaRezultata { get; set; }

        public string SlobodnoUredajaja { get; set; }
        public string ZauzetoUredjaja { get; set; }
    }
}