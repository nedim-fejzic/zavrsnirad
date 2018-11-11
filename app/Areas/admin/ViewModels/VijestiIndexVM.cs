using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class VijestiIndexVM
    {

        public string Naziv { get; set; }

        public List<VijestiKategorija> ListaKategorija { get; set; }
        public int OdabranaKategorija { get; set; }


        public IPagedList<Vijesti> ListaRezultata { get; set; }



    }

   
}