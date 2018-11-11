using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class ZahtjevIndexVM
    {
        public List<ZahtjevStatus> ListaKategorija { get; set; }
        public int OdabranaKategorija { get; set; }

        public string Naziv { get; set; }

        public IPagedList<Zahtjev> ListaRezultata { get; set; }
    }

  
}