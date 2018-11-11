using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class VijestIndexVM
    {
        public IPagedList<Vijesti> ListaRezultata { get; set; }

    }
}