using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class ListaPaketaView
    {
        public Paket Paket { get; set; }
        public bool Izabran { get; set; }
    }
}