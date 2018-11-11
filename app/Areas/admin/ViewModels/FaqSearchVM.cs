using app.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class FaqSearchVM
    {   
        public List<FAQKategorija> ListaKategorija { get; set; }
        public int OdabranaKategorija { get; set; }
        public IPagedList<FAQ> ListaRezultata { get; set; }
    }
}

