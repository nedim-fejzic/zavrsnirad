using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class AktivneUslugeIndexVm
    {
        public List<Paket> ListaPaketa { get; set; }
        public List<TipUsluga> ListaTipUsluga { get; set; }
        public int OdabranaKategorija { get; set; }
        public int OdabranaUsluga { get; set; }
        public string ImePrezime { get; set; }

        public IPagedList<AktivneUsluge> ListaRezultata { get; set; }

    }
}