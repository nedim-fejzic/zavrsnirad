using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class UputstvoIndexVM
    {
        public List<Uputstva> ListaUputstva { get; set; }
        public List<UputstvaKategorije> ListaKategorija { get; set; }
    }
}